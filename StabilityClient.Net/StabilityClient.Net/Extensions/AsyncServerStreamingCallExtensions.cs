using Google.Protobuf.Collections;
using Gooseai;
using Grpc.Core;
using StabilityClient.Net.Models;

namespace StabilityClient.Net.Extensions;

public static class AsyncServerStreamingCallExtensions {
    /// <summary>
    /// Save images from response to file in expected directory.
    /// </summary>
    /// <param name="response">extended type</param>
    /// <param name="directoryPath">path to directory where images are expected to save</param>
    /// <param name="token">cancellation token</param>
    /// <returns>IEnumerable of GenerateResponseSaveResult with FullPath for all saved files</returns>
    /// <exception cref="ArgumentException">throw when directory path is null or empty</exception>
    public static async Task<IEnumerable<GenerateResponseSaveResult>> SaveImagesToAsync(
        this AsyncServerStreamingCall<Answer> response, string directoryPath, CancellationToken token = default) {
        if (string.IsNullOrEmpty(directoryPath)) {
            throw new ArgumentException(
                $"Directory path cannot be null or empty, was: {directoryPath}. Change value of {nameof(directoryPath)}.",
                nameof(directoryPath));
        }

        var streamReader = response.ResponseStream;
        var result = new List<GenerateResponseSaveResult>();

        while (await streamReader.MoveNext(token)) {
            var answer = streamReader.Current;
            var saveResult = await SaveArtifacts(directoryPath, answer.Artifacts, token);
            result.AddRange(saveResult);
        }

        return result;
    }

    private static async Task<List<GenerateResponseSaveResult>> SaveArtifacts(string directoryPath,
        RepeatedField<Artifact> artifacts, CancellationToken token) {
        var tasks = new List<Task>();
        var result = new List<GenerateResponseSaveResult>();

        foreach (var artifact in artifacts) {
            if (artifact.Type != ArtifactType.ArtifactImage) continue;

            var task = SaveImage(directoryPath, artifact.Binary.ToByteArray(), token)
                .ContinueWith(saveImageTask => result.Add(CreateResponse(saveImageTask.Result)), token);
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
        return result;
    }

    private static async Task<string> SaveImage(string directoryPath, byte[] content, CancellationToken token) {
        var fileName = $"{DateTime.Now:MM-dd-yy_HH-mm-ss}-{Guid.NewGuid()}.png";
        var fullPath = Path.Combine(Path.GetFullPath(directoryPath), fileName);

        await File.WriteAllBytesAsync(fullPath, content, token);

        return fullPath;
    }

    private static GenerateResponseSaveResult CreateResponse(string path) => new() {
        FullPath = path
    };
}

using Google.Protobuf.Collections;
using Gooseai;
using Grpc.Core;
using StabilityClient.Net.Models;

namespace StabilityClient.Net.Extensions;

public static class AsyncServerStreamingCallExtensions {
    public static async Task<IEnumerable<GenerateResponseSaveResult>> SaveImagesToAsync(
        this AsyncServerStreamingCall<Answer> response, string directoryPath, CancellationToken token = default) {
        if (string.IsNullOrEmpty(directoryPath)) {
            throw new ArgumentException(
                $"Directory path cannot be null or empty, was: {directoryPath}. Change value of {nameof(directoryPath)}.");
        }
        var streamReader = response.ResponseStream;
        var generateResponseSaveResult = new List<GenerateResponseSaveResult>();
        while (await streamReader.MoveNext(token)) {
            var answer = streamReader.Current;
            var tasks = new List<Task>();
            SaveArtifacts(directoryPath, answer.Artifacts, generateResponseSaveResult, tasks, token);

            await Task.WhenAll(tasks);
        }

        return generateResponseSaveResult;
    }

    private static void SaveArtifacts(string directoryPath, RepeatedField<Artifact> artifacts,
        List<GenerateResponseSaveResult> generateResponseSaveResult, List<Task> tasks, CancellationToken token) {
        foreach (var artifact in artifacts) {
            if (artifact.Type == ArtifactType.ArtifactImage) {
                SaveImage(directoryPath, artifact, generateResponseSaveResult, tasks, token);
            }
        }
    }

    private static void SaveImage(string directoryPath, Artifact artifact,
        List<GenerateResponseSaveResult> generateResponseSaveResults, List<Task> tasks, CancellationToken token) {
        var content = artifact.Binary.ToByteArray();
        var fileName = $"{DateTime.Now:MM-dd-yy HH_mm_ss}-{Guid.NewGuid()}.png";
        var path = Path.Combine(Path.GetFullPath(directoryPath), fileName);
        var task = File.WriteAllBytesAsync(path, content, token)
            .ContinueWith(_ => generateResponseSaveResults.Add(CreateResponse(path)), token);
        tasks.Add(task);
    }

    private static GenerateResponseSaveResult CreateResponse(string path) => new() {
        FullPath = path
    };
}
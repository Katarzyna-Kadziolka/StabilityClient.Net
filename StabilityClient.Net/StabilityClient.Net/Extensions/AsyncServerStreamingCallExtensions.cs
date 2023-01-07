using Gooseai;
using Grpc.Core;
using StabilityClient.Net.Models;

namespace StabilityClient.Net.Extensions; 

public static class AsyncServerStreamingCallExtensions {
    public static async Task<IEnumerable<GenerateResponseSaveResult>> SaveImagesToAsync(this AsyncServerStreamingCall<Answer> response, string directoryPath, CancellationToken token = default) {
        var streamReader = response.ResponseStream;
        var generateResponseSaveResult = new List<GenerateResponseSaveResult>();
        while (await streamReader.MoveNext(token)) {
            var answer = streamReader.Current;
            var tasks = new List<Task>();
            foreach (var artifact in answer.Artifacts) {
                if (artifact.Type == ArtifactType.ArtifactImage) {
                    var content = artifact.Binary.ToByteArray();
                    var fileName = $"{DateTime.Now:MM-dd-yy HH_mm_ss}-{Guid.NewGuid()}";
                    var task = File.WriteAllBytesAsync(  $"{directoryPath}/{fileName}.png", content, token)
                        .ContinueWith(_ =>generateResponseSaveResult.Add(CreateResponse(fileName)), token);
                    tasks.Add(task);
                }
            }

            await Task.WhenAll(tasks);
        }

        return generateResponseSaveResult;
    }

    private static GenerateResponseSaveResult CreateResponse(string fileName) => new GenerateResponseSaveResult {
        FullPath = Path.GetFullPath(fileName)
    };
    
}
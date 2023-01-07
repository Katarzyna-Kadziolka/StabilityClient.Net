using Google.Protobuf;
using Gooseai;
using Grpc.Core;
using NSubstitute;

namespace StabilityClient.Net.Tests.Unit.Factories;

public static class AsyncServerStreamingCallFactory {
    public static AsyncServerStreamingCall<Answer> Create() {
        var asyncStreamReaderSubstitute = Substitute.For<IAsyncStreamReader<Answer>>();
        var answers = new List<Answer> {
            new() {
                Artifacts = {
                    new Artifact {
                        Type = ArtifactType.ArtifactImage,
                        Binary = ByteString.Empty
                    }
                }
            }
        };
        
        var enumerator = answers.GetEnumerator();
        
        asyncStreamReaderSubstitute
            .MoveNext(Arg.Any<CancellationToken>())
            .Returns(_ => Task.FromResult(enumerator.MoveNext()));
        
        asyncStreamReaderSubstitute
            .Current
            .Returns(_ => enumerator.Current);
        
        var responseHeadersAsync = (object o) => Task.FromResult(new Metadata());
        var getStatusFunc = (object o) => Status.DefaultSuccess;
        var getTrailersFunc = (object o) => new Metadata();
        var disposeAction = (object o) => { enumerator.Dispose(); };
        object state = new object();

        return new AsyncServerStreamingCall<Answer>(
            asyncStreamReaderSubstitute, 
            responseHeadersAsync, 
            getStatusFunc,
            getTrailersFunc, 
            disposeAction, 
            state);
    }
}
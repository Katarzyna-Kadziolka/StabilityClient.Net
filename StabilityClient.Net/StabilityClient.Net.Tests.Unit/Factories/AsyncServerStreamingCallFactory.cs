using Google.Protobuf;
using Gooseai;
using Grpc.Core;
using NSubstitute;

namespace StabilityClient.Net.Tests.Unit.Factories;

public static class AsyncServerStreamingCallFactory {
    public static AsyncServerStreamingCall<Answer> Create(int numberOfImages) {
        var answers = CreateAnswers(numberOfImages);
        var enumerator = answers.GetEnumerator();
        var asyncStreamReaderSubstitute = CreateAsyncStreamReaderMock(enumerator);

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

    private static IAsyncStreamReader<Answer> CreateAsyncStreamReaderMock(IEnumerator<Answer> enumerator) {
        var asyncStreamReaderSubstitute = Substitute.For<IAsyncStreamReader<Answer>>();
        
        asyncStreamReaderSubstitute
            .MoveNext(Arg.Any<CancellationToken>())
            .Returns(_ => Task.FromResult(enumerator.MoveNext()));

        asyncStreamReaderSubstitute
            .Current
            .Returns(_ => enumerator.Current);

        return asyncStreamReaderSubstitute;
    }

    private static IEnumerable<Answer> CreateAnswers(int numberOfImages) {
        return Enumerable.Range(0, numberOfImages).Select(_ => new Answer {
            Artifacts = {
                new Artifact {
                    Type = ArtifactType.ArtifactImage,
                    Binary = ByteString.Empty
                }
            }
        });
    }
}
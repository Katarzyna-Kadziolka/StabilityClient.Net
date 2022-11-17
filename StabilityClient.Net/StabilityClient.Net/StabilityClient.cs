using Gooseai;
using Grpc.Core;
using Grpc.Net.Client;

namespace StabilityClient.Net;

public class StabilityClient : IStabilityClient {
    private readonly GrpcChannel _channel;
    private DashboardService.DashboardServiceClient? _dashboard;
    private EnginesService.EnginesServiceClient? _engines;
    private GenerationService.GenerationServiceClient? _generation;
    private ProjectService.ProjectServiceClient? _project;

    public StabilityClient(string apiKey, string host = "https://grpc.stability.ai:443") {
        var grpcChannelOptions = new GrpcChannelOptions() {
            Credentials = ChannelCredentials.Create(ChannelCredentials.SecureSsl,
                CallCredentials.FromInterceptor((context, metadata) => {
                    metadata.Add("Authorization", $"Bearer {apiKey}");
                    return Task.CompletedTask;
                })
            )
        };

        _channel = GrpcChannel.ForAddress(host, grpcChannelOptions);
    }

    public StabilityClient() : this(GetApiKeyFromEnv()) {
    }

    private static string GetApiKeyFromEnv() {
        var apiKey = Environment.GetEnvironmentVariable(ApiKeyVariableName);
        if (string.IsNullOrEmpty(apiKey)) {
            throw new ArgumentException($"Environment variable {ApiKeyVariableName} is not set.");
        }

        return apiKey;
    }

    private const string ApiKeyVariableName = "STABILITY_KEY";

    public DashboardService.DashboardServiceClient Dashboard =>
        _dashboard ??= new DashboardService.DashboardServiceClient(_channel);

    public EnginesService.EnginesServiceClient Engines =>
        _engines ??= new EnginesService.EnginesServiceClient(_channel);

    public GenerationService.GenerationServiceClient Generation =>
        _generation ??= new GenerationService.GenerationServiceClient(_channel);

    public ProjectService.ProjectServiceClient Project =>
        _project ??= new ProjectService.ProjectServiceClient(_channel);
}
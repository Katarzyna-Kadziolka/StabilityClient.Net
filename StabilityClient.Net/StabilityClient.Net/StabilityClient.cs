using Gooseai;
using Grpc.Core;
using Grpc.Net.Client;

namespace StabilityClient.Net;

/// <summary>
/// Client for Stability API
/// </summary>
public class StabilityClient : IStabilityClient {
    private const string ApiKeyVariableName = "STABILITY_KEY";

    /// <summary>
    /// Enabled communication with DashboardService
    /// </summary>
    public DashboardService.DashboardServiceClient Dashboard =>
        _dashboard ??= new DashboardService.DashboardServiceClient(_channel);
    /// <summary>
    /// Enabled communication with EnginesService
    /// </summary>
    public EnginesService.EnginesServiceClient Engines =>
        _engines ??= new EnginesService.EnginesServiceClient(_channel);
    /// <summary>
    /// Enabled communication with GenerationService
    /// </summary>
    public GenerationService.GenerationServiceClient Generation =>
        _generation ??= new GenerationService.GenerationServiceClient(_channel);
    /// <summary>
    /// Enabled communication with ProjectService
    /// </summary>
    public ProjectService.ProjectServiceClient Project =>
        _project ??= new ProjectService.ProjectServiceClient(_channel);

    /// <summary>
    /// Creates an instance of the StabilityClient class based on the given apiKey and host (optional)
    /// </summary>
    /// <param name="apiKey">enables authorization</param>
    /// <param name="host">optional, address to API</param>
    public StabilityClient(string apiKey, string host = "https://grpc.stability.ai:443") {
        var grpcChannelOptions = new GrpcChannelOptions() {
            Credentials = ChannelCredentials.Create(ChannelCredentials.SecureSsl,
                CallCredentials.FromInterceptor((_, metadata) => {
                    metadata.Add("Authorization", $"Bearer {apiKey}");
                    return Task.CompletedTask;
                })
            )
        };

        _channel = GrpcChannel.ForAddress(host, grpcChannelOptions);
    }
    /// <summary>
    /// Creates an instance of the StabilityClient class based on the environment variable STABILITY_KEY
    /// </summary>
    public StabilityClient() : this(GetApiKeyFromEnv()) {
    }

    private static string GetApiKeyFromEnv() {
        var apiKey = Environment.GetEnvironmentVariable(ApiKeyVariableName);
        if (string.IsNullOrEmpty(apiKey)) {
            throw new ArgumentException($"Environment variable {ApiKeyVariableName} is not set.");
        }

        return apiKey;
    }

    private readonly GrpcChannel _channel;

    private DashboardService.DashboardServiceClient? _dashboard;
    private EnginesService.EnginesServiceClient? _engines;
    private GenerationService.GenerationServiceClient? _generation;
    private ProjectService.ProjectServiceClient? _project;
}

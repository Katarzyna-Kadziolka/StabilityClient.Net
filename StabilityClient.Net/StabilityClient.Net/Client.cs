using Gooseai;
using Grpc.Net.Client;

namespace StabilityClient.Net;

public class Client  {
    private readonly GrpcChannel _channel;
    private DashboardService.DashboardServiceClient? _dashboard;
    private EnginesService.EnginesServiceClient? _engines;
    private GenerationService.GenerationServiceClient? _generation;
    private ProjectService.ProjectServiceClient? _project;

    public Client() {
        _channel = GrpcChannel.ForAddress("https://grpc.stability.ai:443");
    }

    public DashboardService.DashboardServiceClient Dashboard => _dashboard ??= new DashboardService.DashboardServiceClient(_channel);
    public EnginesService.EnginesServiceClient Engines => _engines ??= new EnginesService.EnginesServiceClient(_channel);
    public GenerationService.GenerationServiceClient Generation =>
        _generation ??= new GenerationService.GenerationServiceClient(_channel);
    public ProjectService.ProjectServiceClient Project => _project ??= new ProjectService.ProjectServiceClient(_channel);
}


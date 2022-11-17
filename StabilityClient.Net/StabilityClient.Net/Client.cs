using Gooseai;
using Grpc.Net.Client;

namespace StabilityClient.Net;

public class Client  {
    private readonly GrpcChannel channel;
    private DashboardService.DashboardServiceClient? _dashboard;
    private EnginesService.EnginesServiceClient? _engines;
    private GenerationService.GenerationServiceClient? _generation;
    private ProjectService.ProjectServiceClient? _project;

    public Client() {
        channel = GrpcChannel.ForAddress("https://localhost:7042");
    }

    public DashboardService.DashboardServiceClient Dashboard => _dashboard ??= new DashboardService.DashboardServiceClient(channel);
    public EnginesService.EnginesServiceClient Engines => _engines ??= new EnginesService.EnginesServiceClient(channel);
    public GenerationService.GenerationServiceClient Generation =>
        _generation ??= new GenerationService.GenerationServiceClient(channel);
    public ProjectService.ProjectServiceClient Project => _project ??= new ProjectService.ProjectServiceClient(channel);
}


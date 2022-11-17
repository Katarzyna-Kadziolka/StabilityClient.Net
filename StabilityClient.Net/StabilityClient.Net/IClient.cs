using Gooseai;

namespace StabilityClient.Net; 

public interface IClient {
    public DashboardService.DashboardServiceClient Dashboard { get; }
    public EnginesService.EnginesServiceClient Engines { get; }
    public GenerationService.GenerationServiceClient Generation { get; }
    public ProjectService.ProjectServiceClient Project { get; }
}
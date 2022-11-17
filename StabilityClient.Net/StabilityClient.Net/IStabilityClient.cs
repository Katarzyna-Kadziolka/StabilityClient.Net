using Gooseai;

namespace StabilityClient.Net; 

public interface IStabilityClient {
    public DashboardService.DashboardServiceClient Dashboard { get; }
    public EnginesService.EnginesServiceClient Engines { get; }
    public GenerationService.GenerationServiceClient Generation { get; }
    public ProjectService.ProjectServiceClient Project { get; }
}
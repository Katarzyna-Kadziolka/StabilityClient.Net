using Gooseai;

namespace StabilityClient.Net; 

/// <summary>
/// Interface for StabilityClient
/// </summary>
public interface IStabilityClient {
    /// <summary>
    /// Provide communication with DashboardService
    /// </summary>
    public DashboardService.DashboardServiceClient Dashboard { get; }
    /// <summary>
    /// Provide communication with EnginesService
    /// </summary>
    public EnginesService.EnginesServiceClient Engines { get; }
    /// <summary>
    /// Provide communication with GenerationService
    /// </summary>
    public GenerationService.GenerationServiceClient Generation { get; }
    /// <summary>
    /// Provide communication with ProjectService
    /// </summary>
    public ProjectService.ProjectServiceClient Project { get; }
}

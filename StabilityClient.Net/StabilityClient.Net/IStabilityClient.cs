using Gooseai;

namespace StabilityClient.Net; 

/// <summary>
/// Interface for StabilityClient
/// </summary>
public interface IStabilityClient {
    /// <summary>
    /// Enabled communication with DashboardService
    /// </summary>
    public DashboardService.DashboardServiceClient Dashboard { get; }
    /// <summary>
    /// Enabled communication with EnginesService
    /// </summary>
    public EnginesService.EnginesServiceClient Engines { get; }
    /// <summary>
    /// Enabled communication with GenerationService
    /// </summary>
    public GenerationService.GenerationServiceClient Generation { get; }
    /// <summary>
    /// Enabled communication with ProjectService
    /// </summary>
    public ProjectService.ProjectServiceClient Project { get; }
}

namespace StabilityClient.Net.Models; 

/// <summary>
/// Result use to return data after file save
/// </summary>
public class GenerateResponseSaveResult {
    /// <summary>
    /// Full path to saved file
    /// </summary>
    public string FullPath { get; set; } = "";
}

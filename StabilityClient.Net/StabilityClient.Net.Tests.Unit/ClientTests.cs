namespace StabilityClient.Net.Tests.Unit;

public class ClientTests {
    [SetUp]
    public void Setup() {
        Environment.SetEnvironmentVariable("STABILITY_KEY", "test api key");
    }

    [Test]
    public void Constructor_ShouldCreateChannel() {
        // Arrange
        // Act
        StabilityClient _ = new StabilityClient();
        // Assert
    }
    
    [Test]
    public void Constructor_ApiKey_ShouldCreateChannel() {
        // Arrange
        // Act
        StabilityClient stabilityClient = new StabilityClient("test api key");
        // Assert
    }
    
    [Test]
    public void Constructor_Host_ShouldCreateChannel() {
        // Arrange
        // Act
        StabilityClient stabilityClient = new StabilityClient("test api key", "https://www.google.pl/");
        // Assert
    }
    
    [Test]
    public void Constructor_EmptyEnvironmentalVariable_ShouldThrowArgumentException() {
        // Arrange
        const string stabilityKey = "STABILITY_KEY";
        var apiKey = Environment.GetEnvironmentVariable(stabilityKey);
        Environment.SetEnvironmentVariable(stabilityKey, "");
        // Act
        Action act = () => new StabilityClient();
        // Assert
        act.Should().Throw<ArgumentException>();
        Environment.SetEnvironmentVariable(stabilityKey, apiKey);
    }
    
    [Test]
    public void Constructor_ShouldInitializedDashboard() {
        // Arrange
        // Act
        StabilityClient stabilityClient = new StabilityClient();
        // Assert
        stabilityClient.Dashboard.Should().NotBeNull();
    }
    
    [Test]
    public void Constructor_ShouldInitializedEngines() {
        // Arrange
        // Act
        StabilityClient stabilityClient = new StabilityClient();
        // Assert
        stabilityClient.Engines.Should().NotBeNull();
    }
    
    [Test]
    public void Constructor_ShouldInitializedGeneration() {
        // Arrange
        // Act
        StabilityClient stabilityClient = new StabilityClient();
        // Assert
        stabilityClient.Generation.Should().NotBeNull();
    }
    
    [Test]
    public void Constructor_ShouldInitializedProject() {
        // Arrange
        // Act
        StabilityClient stabilityClient = new StabilityClient();
        // Assert
        stabilityClient.Project.Should().NotBeNull();
    }
}

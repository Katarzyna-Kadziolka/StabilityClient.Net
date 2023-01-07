using StabilityClient.Net.Extensions;
using StabilityClient.Net.Tests.Unit.Factories;

namespace StabilityClient.Net.Tests.Unit.Tests; 

public class AsyncServerStreamingCallExtensionsTests {
    private const string DirectoryPath = "./Assets";
    
    [SetUp]
    public void SetUp() {
        Directory.CreateDirectory(DirectoryPath);
    }
    
    [TearDown]
    public void TearDown() {
        var directoryInfo = new DirectoryInfo(DirectoryPath);
        foreach (var file in directoryInfo.GetFiles()) {
            file.Delete();
        }
    }
    
    [Test]
    public async Task SaveImagesToAsync_OneSample_ShouldReturnTask() {
        // Arrange
        var response = AsyncServerStreamingCallFactory.Create(1);
        // Act
        var saveResults = await response.SaveImagesToAsync(DirectoryPath);
        // Assert
        var generateResponseSaveResults = saveResults.ToArray();
        generateResponseSaveResults.Should().HaveCount(1);
        
        foreach (var saveResult in generateResponseSaveResults) {
            saveResult.FullPath.Should().Contain(Path.GetFullPath(DirectoryPath));
        }
    }
}
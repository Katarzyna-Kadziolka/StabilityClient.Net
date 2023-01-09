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
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(10)]
    public async Task SaveImagesToAsync_ShouldReturnResult(int numberOfImages) {
        // Arrange
        var response = AsyncServerStreamingCallFactory.Create(numberOfImages);
        // Act
        var saveResults = await response.SaveImagesToAsync(DirectoryPath);
        // Assert
        var generateResponseSaveResults = saveResults.ToArray();
        generateResponseSaveResults.Should().HaveCount(numberOfImages);
        
        foreach (var saveResult in generateResponseSaveResults) {
            saveResult.FullPath.Should().Contain(Path.GetFullPath(DirectoryPath))
                .And.EndWith(".png");
        }
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public async Task SaveImagesToAsync_IncorrectDirectoryPath_ShouldThrowArgumentError(string? path) {
        // Arrange
        var response = AsyncServerStreamingCallFactory.Create(1);
        // Act
        var saveResultsAction = async () => await response.SaveImagesToAsync(path!);
        // Assert
        await saveResultsAction.Should().ThrowAsync<ArgumentException>();
    }

}

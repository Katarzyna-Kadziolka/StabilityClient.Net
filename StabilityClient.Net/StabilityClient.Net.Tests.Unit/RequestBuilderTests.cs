namespace StabilityClient.Net.Tests.Unit;

public class RequestBuilderTests {
    [Test]
    public void Build_WithoutSetTextPrompt_ShouldThrowArgumentException() {
        // Arrange
        var builder = new RequestBuilder();
        // Act
        var act = () => builder.Build();
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void SetTextPrompt_ShouldSetTextPrompt() {
        // Arrange
        var expectedPrompt = "Test";
        var builder = new RequestBuilder();
        // Act
        var request = builder
            .SetTextPrompt(expectedPrompt)
            .Build();
        // Assert
        request.Prompt.Count.Should().Be(1);
        request.Prompt[0].Text.Should().Be(expectedPrompt);
        request.Prompt[0].Parameters.Init.Should().BeTrue();
    }

    [Test]
    public void Build_SetOnyTextPrompt_ShouldSetDefaultValues() {
        // Arrange
        var expectedPrompt = "Test";
        var builder = new RequestBuilder();
        // Act
        var request = builder
            .SetTextPrompt(expectedPrompt)
            .Build();
        // Assert
        request.EngineId.Should().Be(RequestBuilder.DefaultEngineId);
        request.Image.Height.Should().Be(RequestBuilder.DefaultImageHeight);
        request.Image.Width.Should().Be(RequestBuilder.DefaultImageWidth);
        request.Image.Steps.Should().Be(RequestBuilder.DefaultImageSteps);
        request.Image.Samples.Should().Be(RequestBuilder.DefaultNumberOfSamples);
    }

    [Test]
    public void SetImageHeight_ShouldSetImageHeight() {
        // Arrange
        var expectedPrompt = "Test";
        ulong expectedHeight = 1024;
        var builder = new RequestBuilder();
        // Act
        var request = builder
            .SetTextPrompt(expectedPrompt)
            .SetImageHeight(expectedHeight)
            .Build();
        // Assert
        request.Image.Height.Should().Be(expectedHeight);
    }

    [Test]
    public void SetImageWidth_ShouldSetImageWidth() {
        // Arrange
        var expectedPrompt = "Test";
        ulong expectedWidth = 1024;
        var builder = new RequestBuilder();
        // Act
        var request = builder
            .SetTextPrompt(expectedPrompt)
            .SetImageWidth(expectedWidth)
            .Build();
        // Assert
        request.Image.Width.Should().Be(expectedWidth);
    }

    [Test]
    public void SetImageSteps_ShouldSetImageSteps() {
        // Arrange
        var expectedPrompt = "Test";
        ulong expectedSteps = 40;
        var builder = new RequestBuilder();
        // Act
        var request = builder
            .SetTextPrompt(expectedPrompt)
            .SetImageSteps(expectedSteps)
            .Build();
        // Assert
        request.Image.Steps.Should().Be(expectedSteps);
    }

    [Test]
    public void SetEngineId_ShouldSetEngineId() {
        // Arrange
        var expectedPrompt = "Test";
        var expectedEngineId = "TestEngineId";
        var builder = new RequestBuilder();
        // Act
        var request = builder
            .SetTextPrompt(expectedPrompt)
            .SetEngineId(expectedEngineId)
            .Build();
        // Assert
        request.EngineId.Should().Be(expectedEngineId);
    }

    [Test]
    public void SetImageSeed_ShouldSetImageSeed() {
        // Arrange
        var expectedPrompt = "Test";
        uint expectedImageSeed = 1;
        var builder = new RequestBuilder();
        // Act
        var request = builder
            .SetTextPrompt(expectedPrompt)
            .SetImageSeed(expectedImageSeed)
            .Build();
        // Assert
        request.Image.Seed.Count.Should().Be(1);
        request.Image.Seed[0].Should().Be(expectedImageSeed);
    }

    [Test]
    public void SetImageSamples_ShouldSetImageSamples() {
        // Arrange
        var expectedPrompt = "Test";
        ulong expectedSamples = 5;
        var builder = new RequestBuilder();
        // Act
        var request = builder
            .SetTextPrompt(expectedPrompt)
            .SetImageSamples(expectedSamples)
            .Build();
        // Assert
        request.Image.Samples.Should().Be(expectedSamples);
    }
}

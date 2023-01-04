using System.Globalization;
using Gooseai;

namespace StabilityClient.Net;

public class RequestBuilder {
    public const string DefaultEngineId = "stable-diffusion-512-v2-1";
    public const ulong DefaultImageHeight = 512;
    public const ulong DefaultImageWidth = 512;
    public const ulong DefaultImageSteps = 30;
    public const ulong DefaultNumberOfSamples = 1;

    
    private readonly Request _request = new () {
        EngineId = DefaultEngineId,
        Image = new ImageParameters {
            Height = DefaultImageHeight,
            Width = DefaultImageWidth,
            Steps = DefaultImageSteps,
            Samples = DefaultNumberOfSamples
        }
    };
    /// <summary>
    /// Creates a request based on previously specified parameters
    /// </summary>
    /// <returns>Request</returns>
    /// <exception cref="ArgumentException">throw when text prompt is null or empty</exception>
    public Request Build() {
        if (_request.Prompt.Count==0) {
            throw new ArgumentException($"Prompt cannot be empty; Use {nameof(SetTextPrompt)} to add new prompt.");
        }

        return _request;
    }

    /// <summary>
    /// Set the expected height of the image
    /// </summary>
    /// <param name="height">expected value of image height; must be a multiple of 64</param>
    /// <returns>RequestBuilder</returns>
    public RequestBuilder SetImageHeight(ulong height) {
        _request.Image.Height = height;
        return this;
    }
    /// <summary>
    /// Set the expected width of the image
    /// </summary>
    /// <param name="width">expected value of image width; must be a multiple of 64</param>
    /// <returns>RequestBuilder</returns>
    public RequestBuilder SetImageWidth(ulong width) {
        _request.Image.Width = width;
        return this;
    }
    /// <summary>
    ///  Set the number of steps to spend generation (diffusing) image
    /// </summary>
    /// <param name="steps">expected number of stpes</param>
    /// <returns>RequestBuilder</returns>
    public RequestBuilder SetImageSteps(ulong steps) {
        _request.Image.Steps = steps;
        return this;
    }
    /// <summary>
    /// Sets which engine will take care of image generation 
    /// </summary>
    /// <param name="engineId">expected engine id</param>
    /// <returns>RequestBuilder</returns>
    public RequestBuilder SetEngineId(string engineId) {
        _request.EngineId = engineId;
        return this;
    }
    /// <summary>
    /// Sets the text prompt, based on which the image will be generated.
    /// </summary>
    /// <param name="text">expected prompt</param>
    /// <returns>RequestBuilder</returns>
    public RequestBuilder SetTextPrompt(string text) {
        _request.Prompt.Clear();
        _request.Prompt.Add(new Prompt {
            Text = text,
            Parameters = new PromptParameters {
                Init = true
            }
        });
        return this;
    }
    /// <summary>
    /// Sets the seed for the random number generator
    /// </summary>
    /// <param name="seed">number for random number generator</param>
    /// <returns>RequestBuilder</returns>
    public RequestBuilder SetImageSeed(uint seed) {
        _request.Image.Seed.Clear();
        _request.Image.Seed.Add(seed);
        return this;
    }
    /// <summary>
    /// Sets the number of images to generate
    /// </summary>
    /// <param name="samples">number of images</param>
    /// <returns>RequestBuilder</returns>
    public RequestBuilder SetImageSamples(ulong samples) {
        _request.Image.Samples = samples;
        return this;
    }
}

using Google.Protobuf;
using Gooseai;

namespace StabilityClient.Net;

/// <summary>
/// Fluent builder to build request for Stability API
/// </summary>
public class RequestBuilder {
    /// <summary>
    /// Default value for inference engine (model) to use
    /// </summary>
    public const string DefaultEngineId = "stable-diffusion-v1-5";
    /// <summary>
    /// Default value for image height
    /// </summary>
    public const ulong DefaultImageHeight = 512;
    /// <summary>
    /// Default value for image width
    /// </summary>
    public const ulong DefaultImageWidth = 512;
    /// <summary>
    /// Default value of diffusion steps performed on the requested generation
    /// </summary>
    public const ulong DefaultImageSteps = 30;
    /// <summary>
    /// Default value of number of images to generate
    /// </summary>
    public const ulong DefaultNumberOfSamples = 1;


    private readonly Request _request = new() {
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
        if (_request.Prompt.Count == 0) {
            throw new ArgumentException(
                $"Prompt cannot be empty; Use {nameof(SetTextPrompt)} to add new text prompt or {nameof(SetInitImage)} to add init image.");
        }

        if (_request.Prompt.Any(a => a.Artifact != null && a.Artifact.Type == ArtifactType.ArtifactMask)) {
            if (_request.Prompt.All(a => a.Artifact == null || a.Artifact.Type != ArtifactType.ArtifactImage)) {
                throw new ArgumentException(
                    $"If mask image is set, init image must also be provided. Use {nameof(SetInitImage)}.");
            }
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
    /// <param name="steps">expected number of steps</param>
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
    /// <param name="weight">the importance of a given prompt in the image creation process; a negative value excludes the elements described in the prompt from the image</param>
    /// <returns>RequestBuilder</returns>
    public RequestBuilder SetTextPrompt(string text, float weight = 1) {
        _request.Prompt.Add(new Prompt {
            Text = text,
            Parameters = new PromptParameters {
                Init = true,
                Weight = weight
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

    /// <summary>
    /// Set the image used to initialize the generation
    /// </summary>
    /// <param name="pathToImage">path pointing to the image</param>
    /// <param name="weight">the importance of a given image in the image creation process; a negative value excludes elements contained in the init image from the image</param>
    /// <returns>RequestBuilder</returns>
    /// <exception cref="ArgumentException">throw when path to image is null or empty</exception>
    public RequestBuilder SetInitImage(string pathToImage, float weight = 1) {
        if (string.IsNullOrEmpty(pathToImage)) {
            throw new ArgumentException(
                $"Path to image cannot be null or empty, was: {pathToImage}. Change value of {nameof(pathToImage)}.",
                nameof(pathToImage));
        }

        _request.Prompt.Add(new Prompt {
            Parameters = new PromptParameters {
                Init = true,
                Weight = weight
            },
            Artifact = new Artifact {
                Type = ArtifactType.ArtifactImage,
                Binary = ByteString.FromStream(File.OpenRead(pathToImage))
            }
        });
        return this;
    }

    /// <summary>
    /// Set grayscale mask to exclude diffusion from some pixels
    /// </summary>
    /// <param name="pathToImage">path pointing to the image</param>
    /// <returns>RequestBuilder</returns>
    /// <exception cref="ArgumentException">throw when path to image is null or empty</exception>
    public RequestBuilder SetMaskImage(string pathToImage) {
        if (string.IsNullOrEmpty(pathToImage)) {
            throw new ArgumentException(
                $"Path to image cannot be null or empty, was: {pathToImage}. Change value of {nameof(pathToImage)}.",
                nameof(pathToImage));
        }

        _request.Prompt.Add(new Prompt {
            Artifact = new Artifact {
                Type = ArtifactType.ArtifactMask,
                Binary = ByteString.FromStream(File.OpenRead(pathToImage))
            }
        });
        return this;
    }

    /// <summary>
    /// Set start schedule value to skip a proportion of the start of the diffusion steps, allowing the init image to influence the final generated image
    /// </summary>
    /// <param name="startSchedule">lower values will result in more influence from the init image, while higher values will result in more influence from the diffusion steps</param>
    /// <returns>RequestBuilder</returns>
    public RequestBuilder SetStartSchedule(float startSchedule) {
        var stepParameter = GetStepParameter();

        if (stepParameter.Schedule is null) {
            stepParameter.Schedule = new ScheduleParameters();
        }

        stepParameter.Schedule.Start = startSchedule;

        return this;
    }

    /// <summary>
    /// Set end schedule value to skip a proportion of the end of the diffusion steps, allowing the init image to influence the final generated image 
    /// </summary>
    /// <param name="endSchedule">lower values will result in more influence from the init_image, while higher values will result in more influence from the diffusion steps</param>
    /// <returns>RequestBuilder</returns>
    public RequestBuilder SetEndSchedule(float endSchedule) {
        var stepParameter = GetStepParameter();

        if (stepParameter.Schedule is null) {
            stepParameter.Schedule = new ScheduleParameters();
        }

        stepParameter.Schedule.End = endSchedule;

        return this;
    }

    /// <summary>
    /// Set sampling engine to use
    /// </summary>
    /// <param name="sampler">Sampling engine to use</param>
    /// <returns>RequestBuilder</returns>
    public RequestBuilder SetSampler(DiffusionSampler sampler) {
        _request.Image.Transform = new TransformType {
            Diffusion = sampler
        };
        return this;
    }

    /// <summary>
    /// Set cfgScale to dictate how closely the engine attempts to match a generation to the provided prompt
    /// </summary>
    /// <param name="cfgScale">higher value keep image closer to prompt</param>
    /// <returns>RequestBuilder</returns>
    public RequestBuilder SetCfgScale(float cfgScale) {
        var stepParameter = GetStepParameter();

        if (stepParameter.Sampler is null) {
            stepParameter.Sampler = new SamplerParameters ();
        }

        stepParameter.Sampler.CfgScale = cfgScale;

        return this;
    }

    private StepParameter GetStepParameter() {
        if (_request.Image.Parameters.Count == 0) {
            _request.Image.Parameters.Add(new StepParameter());
        }

        var stepParameter = _request.Image.Parameters[0];
        return stepParameter;
    }
}

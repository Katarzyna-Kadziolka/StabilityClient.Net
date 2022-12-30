using Gooseai;

namespace StabilityClient.Net;

public class RequestBuilder {
    public const string DefaultEngineId = "stable-diffusion-512-v2-1";
    public const ulong DefaultImageHeight = 512;
    public const ulong DefaultImageWidth = 512;
    public const ulong DefaultImageSteps = 30;
    
    private readonly Request _request = new () {
        EngineId = DefaultEngineId,
        Image = new ImageParameters {
            Height = DefaultImageHeight,
            Width = DefaultImageWidth,
            Steps = DefaultImageSteps,
        }
    };

    public Request Build() {
        if (_request.Prompt.Count==0) {
            throw new ArgumentException($"Prompt cannot be empty; Use {nameof(SetTextPrompt)} to add new prompt.");
        }

        return _request;
    }

    public RequestBuilder SetHeight(ulong height) {
        _request.Image.Height = height;
        return this;
    }

    public RequestBuilder SetWidth(ulong width) {
        _request.Image.Width = width;
        return this;
    }

    public RequestBuilder SetSteps(ulong steps) {
        _request.Image.Steps = steps;
        return this;
    }

    public RequestBuilder SetEngineId(string engineId) {
        _request.EngineId = engineId;
        return this;
    }

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

    public RequestBuilder SetSeed(uint seed) {
        _request.Image.Seed.Clear();
        _request.Image.Seed.Add(seed);
        return this;
    }
}

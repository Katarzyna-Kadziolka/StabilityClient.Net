[![https://www.nuget.org/packages/StabilityClient.Net/](https://img.shields.io/nuget/v/StabilityClient.Net)](https://www.nuget.org/packages/StabilityClient.Net/)
[![https://www.nuget.org/packages/StabilityClient.Net/](https://img.shields.io/nuget/dt/StabilityClient.Net)](https://www.nuget.org/packages/StabilityClient.Net/)
[![GitHub issues](https://img.shields.io/github/issues/Katarzyna-Kadziolka/StabilityClient.Net)](https://GitHub.com/Katarzyna-Kadziolka/StabilityClient.Net/issues/)
[![GitHub license](https://img.shields.io/github/license/Katarzyna-Kadziolka/StabilityClient.Net.svg)](https://github.com/Katarzyna-Kadziolka/StabilityClient.Net/blob/develop/LICENSE)

# StabilityClient.Net

gRPC client for [StabilitySDK](https://github.com/Stability-AI/stability-sdk) written with .Net 6. 

> [Changelog](CHANGELOG.md)

## Features
- Connect to StabilitySDK using:
    - environment variable STABILITY_KEY (default)
    - host (optional)
    - apiKey (optional)
- ```RequestBuilder```: fluent builder to easily build requests 
- ```SaveImagesToAsync```: extension method to AsyncServerStreamingCall to easily save images

## Usage

### 1. Add Nuget package from [here](https://www.nuget.org/packages/StabilityClient.Net/).
### 2. Get your Stability API Key.

> See [here](https://github.com/Stability-AI/stability-sdk) for instruction about how to get **Stability API Key**.

### 3. Create your client.

Recommended:
```csharp
// This will use environment variable STABILITY_KEY as your API key input and https://grpc.stability.ai:443 as your host input.
var stability = new StabilityClient();
```

Optional:

```csharp
var stability = new StabilityClient("myStabilityApiKey", "myHost");
```

### 4. Create request.

Using ```RequestBuilder```:

```csharp
var request = new RequestBuilder()
    .SetTextPrompt("Chihuahua in sombrero")
    .SetImageHeight(512)                        // optional
    .SetImageWidth(512)                         // optional
    .SetImageSteps(30)                          // optional
    .SetEngineId("stable-diffusion-512-v2-1")   // optional
    .Build();
```
Raw:

```csharp
var request = new Request {
  Prompt = {
    new Prompt {
      Text = "Chihuahua in sombrero",
      Parameters = new PromptParameters {
        Init = true
      },
    },
  },
  EngineId = "stable-diffusion-512-v2-1",
  Image = new ImageParameters {
    Height = 512,
    Width = 512,
    Steps = 30
  }
};
```

### 5. Send request and save image from response to file.

Using ```SaveImagesToAsync``` extension method:

```csharp
// Send request
var response = stabilityClient.Generation.Generate(request);

// Save to file
await response.SaveImagesToAsync("./Images");
```

Raw:

```csharp
// Send request
var response = stabilityClient.Generation.Generate(request).ResponseStream;

// Save to file
var source = new CancellationTokenSource();
while (await response.MoveNext(source.Token)) {
  var answer = response.Current;
  foreach (var artifact in answer.Artifacts) {
    if (artifact.Type == ArtifactType.ArtifactImage) {
      var content = artifact.Binary.ToByteArray();
      await File.WriteAllBytesAsync("C:/Projects/Images/Image.png", content, source.Token);
    }
  }
}
```

### 6. Enjoy your image :)

![29 12 2022 13_40_24-a2089c29-f870-407f-a814-8c5f30796c75](https://user-images.githubusercontent.com/62292047/209964002-65a64d50-72bc-46ee-a3fc-ff4f2294166c.png)

### Development
I am happy to accept suggestions for further development. Please feel free to add Issues :)

### Authors
- [Katarzyna Kądziołka](https://github.com/Katarzyna-Kadziolka)

### License
This project is licensed under the MIT License - see the [LICENSE](https://raw.githubusercontent.com/Katarzyna-Kadziolka/StabilityClient.Net/develop/LICENSE) file for details.

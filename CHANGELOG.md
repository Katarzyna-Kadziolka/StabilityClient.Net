# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.7.0] - 30.10.2023

### Changed
- Updated proto files according to the remote changes

## [1.6.0] - 13.06.2023

### Changed
- Updated proto files according to the remote changes

## [1.5.0] - 17.04.2023

### Changed
- Updated proto files according to the remote changes

## [1.4.0] - 08.03.2023

### Changed
- Updated proto files according to the remote changes

## [1.3.0] - 11.02.2023

### Added
- New parameters to `RequestBuilder`:
  - init image
  - mask image
  - start schedule
  - end schedule
  - sampler
  - CFG scale
  - optional weight parameter to `SetTextPrompt`
- `SaveSingleImageToAsync` - extension method to `AsyncServerStreamingCall` to easily save single image with given name

## [1.2.0] - 25.01.2023

### Changed
- Updated proto files according to the remote changes

## [1.1.0] - 09.01.2023

### Added
- `RequestBuilder` to easily create Generate request
- `SaveImagesToAsync` - extension method to `AsyncServerStreamingCall<Answer>` to easily save images  

## [1.0.0] - 13.12.2022

### Added
- support connecting to StabilitySDK using:
  - environment variable STABILITY_KEY (default)
  - host (optional)
  - apiKey (optional)

# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.0] - 09.01.2023

### Added
- RequestBuilder to easily create Generate request
- SaveImagesToAsync: extension method to AsyncServerStreamingCall<Answer> to easily save images  

## [1.0.0] - 13.12.2022

### Added
- support connecting to StabilitySDK using:
  - environment variable STABILITY_KEY (default)
  - host (optional)
  - apiKey (optional)

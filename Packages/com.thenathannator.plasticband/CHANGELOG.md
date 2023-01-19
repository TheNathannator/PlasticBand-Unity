# Changelog

All notable changes to PlasticBand will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Fixes

XInputProGuitar was incorrectly inheriting from RockBandGuitar instead of ProGuitar.

Various controls did not function correctly due to assumptions I made about how aliases and inheritance with controls work. A summary of what's been changed:

- All 5-fret guitars:
  - `strumUp` and `strumDown` aliases on `dpad/up` and `dpad/down` have been removed, and new `strumUp` and `strumDown` controls that replicate `dpad/up` and `dpad/down` have been added in their place.
- Guitar Hero guitars:
  - The slider control has been reworked so that instead of being one control with 5 child controls, it's simply one control used 5 times in the main layout. `GuitarHeroSliderSegmentControl` has been removed, `GuitarHeroSliderControl` now inherits from `ButtonControl`, and the `SliderFret` enum previously exposed by it is now private, as it no longer has any public use.

## [0.1.0] - 2023/15/01

### Added

Preliminary support for the following devices:

- 5-fret guitars
  - Guitar Hero guitars
    - Xbox 360 (Windows)
    - PS3
  - Rock Band guitars
    - Xbox 360 (Windows)
    - PS3
- 6-fret guitars
  - Xbox 360 (Windows)
  - PS3/Wii U
  - PS4
- Drumkits
  - Rock Band drumkits
    - Xbox 360 (Windows)
    - PS3
  - Guitar Hero drumkits
    - Xbox 360 (Windows)
    - PS3
- DJ Hero turntables
  - Xbox 360 (Windows)
  - PS3
  - Includes euphoria light support
- Rock Band 3 Pro Guitars
  - Xbox 360 (Windows)
- Rock Band 3 Pro Keyboards
  - Xbox 360 (Windows)

Note that Xbox 360 devices are currently only supported on Windows. Mac support for those is an impossibilty currently, Linux support just needs some research and implementation done.

# Changelog

All notable changes to PlasticBand will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

Dates are relative to UTC.

## [0.3.0] - 2023/14/05

### Added

- A number of convenience APIs have been added to each of the base device classes, including being able to retrieve controls such as guitar frets or keyboard keys by index or enum, and to get a mask of the state for each of those controls.
- The Rock Band stage kit is now supported, and can be controlled using the `IStageKitHaptics` haptics interface.
- [sanjay900](https://github.com/sanjay900)'s Santroller devices are now supported, including their various LED haptics. ([GH-3](https://github.com/TheNathannator/PlasticBand-Unity/pull/3) & [GH-4](https://github.com/TheNathannator/PlasticBand-Unity/pull/4), plus a bunch of work done afterwards)
- The slider bars on World Tour guitars are now (hopefully) supported.
  - I'm currently unable to completely verify this works, needs more verification.

### Changed

- The XInput/PS3/Wii (and other non-abstract) device classes have been made internal, as they are effectively only implementation details. There's no benefit to using them over the corresponding abstract type, and hopefully there never will be.

## [0.2.3] - 2023/11/05

### Fixed

- Fixed PS3 turntables still having the hardcoded report ID.
- Fixed 4-lane drumkits and RB solo frets not working correctly by removing a hardcoded offset in their layouts.

### Added

- Re-added face button controls to FourLaneDrumkit since those are no longer guaranteed to be handled by the pads.

## [0.2.2] - 2023/04/05

### Fixed

- Fixed newly-discovered edge case with 4-lane drumkit handling that would cause erroneous pad inputs when hitting a cymbal on some kits.
- Fixed some devices incorrectly using "Back" for their `selectButton` display name instead of "Select".
- Temporarily disabled PS4 device layouts to avoid a crash bug in the native side of the Unity input system on Windows.
- Fixed a layout issue with PS4 drumkits where the red pad was being triggered with the wrong face button, and yellow and green wouldn't be triggered at all. ([GH-5](https://github.com/TheNathannator/PlasticBand-Unity/pull/5))
- Fixed PS3 and Wii Rock Band keyboards not getting registered correctly.

### Changed

- Changed a bunch of device layout names for clarity. For example, instead of `Harmonix Guitar for PlayStation(R)3`, it's now labelled `PlayStation 3 Rock Band Guitar`.
- Report IDs on HID device layouts are now detected automatically instead of being hardcoded into the layout.
  - This ensures everything works correctly on all platforms, and accounts for cases where a report ID may be present where one isn't expected. (Wish the native backend would account for this automatically on all platforms, but this works too.)

## [0.2.1] - 2023/14/04

### Fixed

- I forgot to initialize some things again lol, that is now addressed.

## [0.2.0] - 2023/14/04

### Added

Instrument support:

- Wii Rock Band guitars and drumkits are now supported.
- PS3 and Wii Pro Guitars are now supported, along with the PS3 and Wii MIDI Pro Adapter in Pro Guitar mode.
- PS4 Rock Band guitars and drumkits are now supported.
- The drums mode for the PS3 and Wii MIDI Pro adapters are now supported.
- PS3 and Wii Pro Keyboards are now supported, along with the PS3 and Wii MIDI Pro Adapter in keys mode.
- The World Tour PC guitar is now supported.

Layout controls:

- A new ButtonAxisPair control type has been added, which allows an axis and a button to function as a single axis/button.

### Fixed

- Fixed the layouts for PS3 drumkits and turntables not being initialized.
- Fixed the layouts for Xbox 360 Pro Guitars having some incorrect offsets.
- Fixed the layouts for Xbox 360 keytars potentially having incorrect offsets.
- Fixed the solo fret controls on PS3/Wii Rock Band guitars having the wrong offset.
- Rock Band keyboards now have (hopefully) the correct normalization applied to the analog pedal input.
- Fixed some specific-platform device classes being incorrectly marked as internal instead of public.

## [0.1.2] - 2023/08/04

Minor bugfix release for other issues present in v0.1.

### Fixed

- Fixed PS3/4 GHL guitar strumbar inputs being flipped.
- Fixed PS4 GHL guitar face button mappings (Start, Hero Power, GHTV button, d-pad center).
- Changed the default report ID of PS3 output reports to 0 instead of 1.

## [0.1.1] - 2023/19/01

Hotfix release for issues present in v0.1.0.

### Fixed

XInputProGuitar was incorrectly inheriting from RockBandGuitar instead of ProGuitar.

Various controls did not function correctly due to assumptions I made about how aliases and inheritance with controls work. A summary of what's been changed:

- Replaced all instances of aliasing with dedicated controls.
  - On 5-fret guitars, `strumUp` and `strumDown` aliases on `dpad/up` and `dpad/down` have been removed, and new `strumUp` and `strumDown` controls that replicate `dpad/up` and `dpad/down` have been added in their place.
  - On DJ Hero turntables, the `euphoria` alias on `buttonNorth` has been replaced with a dedicated `euphoria` control that duplicates `buttonNorth`.
- Guitar Hero guitars:
  - The slider control has been reworked so that instead of being one control with 5 child controls, it's simply one control used 5 times in the main layout. `GuitarHeroSliderSegmentControl` has been removed, `GuitarHeroSliderControl` now inherits from `ButtonControl`, and the `SliderFret` enum previously exposed by it is now private, as it no longer has any public use.
- 4-lane drumkits:
  - `FourLanePadsControl` has also been reworked to remove its child controls. `FourLanePadControl` has been removed, `FourLanePadsControl` now inherits from `ButtonControl`, and the `FourLanePad` enum previously exposed by it is now private, as it no longer has any public use.

The PS3 drumkit used the wrong byte offset for its `FourLanePadsControl`.

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

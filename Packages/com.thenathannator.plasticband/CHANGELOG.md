# Changelog

All notable changes to PlasticBand will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

Dates are relative to UTC.

## [Unreleased]

### Fixed

- Various control conflicts/duplicate controls on the device layouts have been fixed. Some of the properties on the device classes may still result in duplicate inputs relative to each other, but in these cases both properties will now refer to the same control instance instead of two separate controls. Interactive rebinding will also now work correctly, whereas before these duplicate controls were causing issues.

### Added

- Velocity support for drumkits has now been added! No new controls have been added for this, instead the existing button controls for the pads and cymbals have been made analog. They have also been configured so that any velocity will register as a press, so no special handling will need to be done if you don't care about velocity, so long as things are done relative to the press point (i.e. via `isPressed` or `IsValueConsideredPressed`).
- Support for the emulated 5-fret flags on Pro Guitars has been expanded to include the solo frets as well, along with additional convenience methods for the emulated frets in general.
- The pedal port on Pro Guitars is now supported as well, at least the digital portion of it. Unsure if it supports analog pedals like the Pro Keyboard does.

### Changed

- Pro Guitar whammy has been removed, as it is not an actual control that those guitars have.
- Pro Guitar string velocity is no longer reported directly, as from what I know it is not very reliable and can be confusing to work with if you haven't had experience with them before. Now, only whether or not a strum has occurred is reported; due to how this is determined, the value only lasts for a single frame and must be reset at the end of the frame.
- The custom control types used internally are now `internal` instead of `public`, as these are meant more as implementation details rather than concrete custom control types.
- `XInputSixFretGuitar` is now `internal` as is intended; it was unintentionally left public when all of the implementation device classes were made internal.

## [0.3.4] - 2023/15/08

### Fixed

- Whammy and tilt on PS4 RB4 guitars has been fixed, and the pickup switch is now implemented.
- Xbox 360 turntables now instantiate correctly again, and are now properly scaled up.

## [0.3.3] - 2023/28/07

### Added

- Preliminary support for the XInput `GuitarBass` subtype has been added. Unsure if it has different input features compared to a regular Rock Band guitar, confirmation needed.

### Changed

- The pickup switch on Xbox 360/PS3/Wii Rock Band guitars is now properly supported. It is now exposed as an integer control instead of an axis, ranging from 0 to 4 for each of the 5 notches on the guitar.
  - The pickup switch on PS4 guitars is not confirmed to be supported, it has to be defined somewhere in the input layout for things to work so I've had to guess where it is for now lol
- Santroller device support has been updated to support the latest version of the HID reports. ([GH-6](https://github.com/TheNathannator/PlasticBand-Unity/pull/6) & [GH-7](https://github.com/TheNathannator/PlasticBand-Unity/pull/7))
- The control name for the PS button on PS3 instruments has been updated to be more generic (`psButton` -> `systemButton`).
- The instrument button on Wii Rock Band instruments now has the correct display name, rather than appearing as the PS button.

### Fixed

- The left platter buttons on PS3 turntables should now register correctly, they were off by 1 bit lol
- Wii RB3 Pro Keyboards should now register correctly, they were registered under the PS3 keyboard's hardware IDs on accident.

## [0.3.2] - 2023/27/05

### Changed

- All HID device layouts are now matched against their proper usages in addition to their vendor/product IDs.
  - This avoids the crash that PS4 and Santroller layouts were causing on Windows.
- PS4 and Santroller device layouts have been re-enabled now that they no longer crash Unity.
- Reworked Santroller HID device layouts to no longer use a layout finder, instead they register multiple matchers for each variation.
  - This will allow the proper layout to persist across editor domain reloads, rather than reverting to the fallback that was previously used for the layout finder.

## [0.3.1] - 2023/23/05

### Fixed

- Some initialization issues when entering Play mode in the editor have been fixed.
- Fixed XInput layout overrides not being evaluated correctly due to a small logic error.
- Temporarily disabled Santroller HID device layouts to avoid the same crash bug as with the PS4 instrument layouts on Windows.

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

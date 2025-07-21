# Changelog

All notable changes to PlasticBand will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

Dates are relative to UTC.

## [0.9.0] - 2025/07/21

### Changed

- XInput device layouts are now registered regardless of platform, enabling these layouts to work with backend packages (such as HIDrogen) that implement the XInput device interface on normally-unsupported platforms.
- Overhauled haptics interfaces and implementations to fix issues, de-duplicate output commands, and update Santroller haptics to their latest revision.
- Overhauled 4-lane drumkit velocity handling to correctly handle pad+cymbal hits of the same color.

## [0.8.5] - 2024/08/07

### Fixed

- Fixed the MadCatz Xbox One drumkit not being recognized, typo'd the product ID.

## [0.8.4] - 2024/07/30

### Fixed

- Fixed the Wii MIDI Pro Adapter not being recognized properly in drums mode, due to an incorrect product ID in the layout registration.

## [0.8.3] - 2024/06/29

### Added

- Added support for the Xbox One Wired and Wireless Legacy Adapters.
  - These adapters might not work fully consistently at first, as GameInput does not currently support sending raw output reports. Instruments may need to be reconnected for them to be picked up until this functionality is implemented on their end.

## [0.8.2] - 2024/06/28

### Fixed

- i screwed up the Santroller fix a little and there was a potential null reference bug, whoops lol

## [0.8.1] - 2024/06/28

### Fixed

- Santroller XInput layouts are now given priority over the normal XInput layouts. This fixes 5-fret Santroller guitars in XInput mode being detected as normal 6-fret guitars.
  - There was also a bug on Santroller's side which caused it to report the incorrect XInput capability flags, which has been fixed on their end.

## [0.8.0] - 2024/06/18

### Added

- Xbox One instruments are now supported through HIDrogen v0.3.0's new GameInput backend. This includes Rock Band 4 Stratocasters/Jaguars, Rock Band 4 drumkits, PDP Riffmasters, and Guitar Hero Live guitars.
  - The Rock Band 4 wireless legacy adapter is not supported yet, but it is planned for the next release. It's more complicated to handle, so I decided to not let it hold back the support for other devices.

### Fixed

- PS3 Rock Band guitars now handle the whammy bar's null state value correctly. It will no longer snap to the middle after a few moments of no movement.
- The yellow and green pads on Xbox 360 Rock Band 1 kits are now read correctly, there was an issue in the velocity conversion code.

## [0.7.1] - 2024/05/09

### Changed

- The display names for start/select on PS4/5 guitars have been corrected to be "Options"/"Share", respectively.

### Fixed

- Fixed an off-by-one error on PS4/5 Riffmasters, causing incorrect input recognition.

## [0.7.0] - 2024/05/01

### Added

- PS4/5 Riffmaster guitars are now supported, with a new dedicated layout added to expose the joystick on their headstock.

### Changed

- Rock Band PS4 guitar layouts now use the independent fret bits sent by the guitar, rather than the fret flags.
- The handling for Rock Band guitar solo frets has been made a lot more reasonable (in my opinion):
  - For guitars that use a flag-based system to report frets (Xbox 360, PS3, Wii), the solo frets are no longer mirrored onto the normal frets; pressing a solo fret will make the normal frets also act as solo frets.
  - For guitars that send the normal and solo frets independently (PS4), it is now ensured that they will be usable independently.

## [0.6.0] - 2024/03/18

### Changed

- Stage kits are now a specialized type of gamepad rather than a completely bespoke device type.
  - This was done to better support Santroller stage kits, which allow mapping all gamepad inputs.
- Santroller HID stage kit support has been updated to use the new input report format.

### Fixed

- PS3 Guitar Hero guitars no longer have inverted accelerometer axes.
- Xbox 360 ION drumkits should no longer be falsely detected as GH drumkits due to a hardware quirk, they are now specially identified and bypass the normal differentiation process.

## [0.5.1] - 2024/02/14

### Fixed

- The PS3 ION drumkit, as well as any other PS3 instruments that don't send the full expected state report, should now work correctly.
  - Explicit sizes were set on the state layout structs, which causes state translation to fail if the instrument doesn't send the right size. However, the extra data within those sizes isn't actually used, so they have been removed.

## [0.5.0] - 2024/01/07

### Added

- A new system has been put into place to effectively allow the layout assigned to a device to be changed at runtime. This system gives some much-needed flexibility for certain future device types, and avoids the need for hacks like manually retrieving the current state for XInput drumkits to distinguish between 4-lane and 5-lane drumkits, which proved to be unreliable.
  - **NOTE:** This system is *incompatible* with `InputSystem.onEvent` and `InputControlExtensions.EnumerateControls`/`EnumerateChangedControls`! It involves forwarding state events from one `InputDevice` to another, so the device ID present in the `InputEventPtr` will not match the device instance the state is ultimately used for. Use `InputState.onChange` or `IInputStateChangeMonitor`s instead.

### Fixed

- The XInput GuitarBass subtype was being matched to the Guitar Hero guitar layout instead of the Rock Band guitar layout.
- Guitar Praise guitars weren't being recognized correctly, since they use the joystick HID usage and not the gamepad usage.
  - The joystick usage is now matched against on all HID device layouts, so this won't affect any other layouts in the future.
- Distinguishing between 4-lane and 5-lane XInput drumkits should be much more reliable now due to the new variant device system.

### Removed

- The `GetFretMaskExcludingSolo` method on `RockBandGuitar`s has been removed, as not all Rock Band guitars report the solo frets independently from the main frets, which could make the results from the API confusing. If this behavior is required, `GetFretMask() & ~GetSoloFretMask()` will attain the same results.

## [0.4.6] - 2024/01/05

### Added

- A new `PLASTICBAND_VERBOSE_LOGGING` compile define has been added to enable certain, potentially repetitive logging in game builds. This define is not required inside the editor, as the logging is always enabled there.
  - Currently the only logging this enables is for device command errors.

### Fixed

- Santroller HID haptics were not using the correct report ID, and wouldn't send as a result. Other fixes in the Santroller firmware made alongside this change now mean that HID haptics are properly functioning finally.

## [0.4.5] - 2024/01/05

### Added

- The Guitar Praise guitar is now supported. This is the first guitar to be neither a `GuitarHeroGuitar` nor a `RockBandGuitar`, it is simply a `FiveFretGuitar`.

### Fixed

- Fixed the `SetMultiplier` method on `ISantrollerHaptics` using the wrong command ID when sending the command to the device.

## [0.4.4] - 22/12/2023

### Fixed

- PS4 Rock Band guitars were not registering whammy/tilt/the pickup switch correctly due to an off-by-one error in the layout.

## [0.4.3] - 2023/12/05

### Fixed

- The 4-lane drumkit layouts now ensure they report a non-zero velocity for all pad/cymbal hits.
  - The 5-lane drumkit layouts don't make the same guarantee currently, as a velocity value of 0 is used to differentiate between pads/cymbals and face buttons.
- Santroller stage kit layouts are now registered correctly.
- Santroller XInput 6-fret guitars are also now registered correctly, they were incorrectly being registered as regular XInput 6-fret guitars.

## [0.4.2] - 2023/11/15

### Fixed

- The velocity values on PS3/Wii 4-lane drumkits are no longer inverted.
- Pro Guitars no longer forget to assign their digital pedal control.
- 6-fret guitars were using the entirely wrong state layout lol, copy-paste error I missed during review.
- Fixed turntables failing to initialize due to `sbyte`s not being considered integers by `InputState.IsIntegerFormat` for some reason.
- Fixed turntable platter buttons and Pro Guitar digital pedal not working correctly; private fields in layout structs aren't checked during layout creation it seems.
- Fixed Pro Guitar strumming not registering in the first input event received for one.
- Fixed Pro Keyboard `GetKeyMask` returning an inverted mask (i.e. bit 24 was key 1 rather than bit 0).
- Fixed Santroller guitars not having the correct default state value set.
- Fixed various input control extensions not working on devices that use the new state translation infrastructure.
- Fixed yellow/green pad/cymbal velocities on XInput 4-lane drumkits being inverted.

### Removed

- The redundant `SoloFretCount`, `TouchFretCount`, and `EmulatedSoloFretCount` constants on `RockBandGuitar`, `GuitarHeroGuitar`, and `ProGuitar` respectively have been removed, in favor of having the one `FiveFretGuitar.FretCount` constant for everything.
  - The `EmulatedFretCount` constant on `ProGuitar` remains for convenience, but is otherwise now directly equivalent to `FiveFretGuitar.FretCount`.

## [0.4.1] - 2023/11/03

### Fixed

- Drumkits no longer forget to assign their face button controls to their class instance on initialization.
- Fixed a null-reference exception when using the `EnumerateControls` extension on an `InputEventPtr` for a device that uses the new state translation infrastructure.

## [0.4.0] - 2023/10/31

### Fixed

- Various control conflicts/duplicate controls on the device layouts have been fixed. Some of the properties on the device classes may still result in duplicate inputs relative to each other, but in these cases both properties will now refer to the same control instance instead of two separate controls. This should fix interactive rebinding not working correctly with these controls.
- The table velocity on turntables will now truly rest at 0 instead of being just slightly above it.

### Added

- Velocity support for drumkits has now been added! No new controls have been added for this, instead the existing button controls for the pads and cymbals have been made analog. They have also been configured so that any velocity will register as a press, so no special handling will need to be done if you don't care about velocity, so long as things are done relative to the press point (i.e. via `isPressed` or `IsValueConsideredPressed`).
- Support for the emulated 5-fret flags on Pro Guitars has been expanded to include the solo frets as well, along with additional convenience methods for the emulated frets in general.
- The pedal port on Pro Guitars is now supported as well, at least the digital portion of it. Unsure if it supports analog pedals like the Pro Keyboard does.
- 5-lane drumkits once again expose dedicated face button controls, this time without being identical to the equivalent drum/cymbal controls. Detection is done to separate the two, and drum hits will mask out any face button presses.

### Changed

- Pro Guitar whammy has been removed, as it is not an actual control that those guitars have.
- Pro Guitar string velocity is no longer reported directly, as from what I know it is not very reliable and can be confusing to work with if you haven't had experience with them before. Now, only whether or not a strum has occurred is reported; due to how this is determined, the value only lasts for a single frame and must be reset at the end of the frame.
- The custom control types used internally are now `internal` instead of `public`, as these are meant more as implementation details rather than concrete custom control types.
- `XInputSixFretGuitar` is now `internal` as is intended; it was unintentionally left public when all of the implementation device classes were made internal.
- Strum up/down on 5/6-fret guitars are now directly aliased to d-pad up/down, and no longer have their own distinct control instances.
- The layout-only velocity controls on Pro Keyboards have been removed. If/when velocity support is implemented, the regular key controls will become analog instead, much like how drums velocity support has been implemented.
- The euphoria button on turntables is now directly aliased to the north face button control, and no longer has its own distinct control instance.
- The crossfader control on turntables has been renamed from `crossFader` to `crossfader`.
- Wii Rock Band instruments now have their start/select controls' display names set to `Plus` and `Minus`.
- The effects dial on turntables is now reported as a value in rotations, ranging from 0 inclusive to 1 exclusive.

## [0.3.4] - 2023/08/15

### Fixed

- Whammy and tilt on PS4 RB4 guitars has been fixed, and the pickup switch is now implemented.
- Xbox 360 turntables now instantiate correctly again, and are now properly scaled up.

## [0.3.3] - 2023/07/28

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

## [0.3.2] - 2023/05/27

### Changed

- All HID device layouts are now matched against their proper usages in addition to their vendor/product IDs.
  - This avoids the crash that PS4 and Santroller layouts were causing on Windows.
- PS4 and Santroller device layouts have been re-enabled now that they no longer crash Unity.
- Reworked Santroller HID device layouts to no longer use a layout finder, instead they register multiple matchers for each variation.
  - This will allow the proper layout to persist across editor domain reloads, rather than reverting to the fallback that was previously used for the layout finder.

## [0.3.1] - 2023/05/23

### Fixed

- Some initialization issues when entering Play mode in the editor have been fixed.
- Fixed XInput layout overrides not being evaluated correctly due to a small logic error.
- Temporarily disabled Santroller HID device layouts to avoid the same crash bug as with the PS4 instrument layouts on Windows.

## [0.3.0] - 2023/05/14

### Added

- A number of convenience APIs have been added to each of the base device classes, including being able to retrieve controls such as guitar frets or keyboard keys by index or enum, and to get a mask of the state for each of those controls.
- The Rock Band stage kit is now supported, and can be controlled using the `IStageKitHaptics` haptics interface.
- [sanjay900](https://github.com/sanjay900)'s Santroller devices are now supported, including their various LED haptics. ([GH-3](https://github.com/TheNathannator/PlasticBand-Unity/pull/3) & [GH-4](https://github.com/TheNathannator/PlasticBand-Unity/pull/4), plus a bunch of work done afterwards)
- The slider bars on World Tour guitars are now (hopefully) supported.
  - I'm currently unable to completely verify this works, needs more verification.

### Changed

- The XInput/PS3/Wii (and other non-abstract) device classes have been made internal, as they are effectively only implementation details. There's no benefit to using them over the corresponding abstract type, and hopefully there never will be.

## [0.2.3] - 2023/05/11

### Fixed

- Fixed PS3 turntables still having the hardcoded report ID.
- Fixed 4-lane drumkits and RB solo frets not working correctly by removing a hardcoded offset in their layouts.

### Added

- Re-added face button controls to FourLaneDrumkit since those are no longer guaranteed to be handled by the pads.

## [0.2.2] - 2023/05/04

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

## [0.2.1] - 2023/04/14

### Fixed

- I forgot to initialize some things again lol, that is now addressed.

## [0.2.0] - 2023/04/14

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

## [0.1.2] - 2023/04/08

Minor bugfix release for other issues present in v0.1.

### Fixed

- Fixed PS3/4 GHL guitar strumbar inputs being flipped.
- Fixed PS4 GHL guitar face button mappings (Start, Hero Power, GHTV button, d-pad center).
- Changed the default report ID of PS3 output reports to 0 instead of 1.

## [0.1.1] - 2023/01/19

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

## [0.1.0] - 2023/01/15

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

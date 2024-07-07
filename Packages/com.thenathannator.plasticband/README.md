# PlasticBand-Unity

An add-on for the [Unity InputSystem](https://github.com/Unity-Technologies/InputSystem) package that provides mappings and classes for Rock Band and Guitar Hero peripherals.

## Table of Contents

- [Usage](#usage)
  - [Notes](#notes)
  - [Configuration](#configuration)
- [Supported Devices](#supported-devices)
  - [5-Fret Guitars](#5-fret-guitars)
    - [Guitar Hero](#guitar-hero)
    - [Rock Band](#rock-band)
    - [Riffmaster](#riffmaster)
    - [Other](#other)
  - [6-Fret Guitars](#6-fret-guitars)
  - [4-Lane Drumkits (Rock Band)](#4-lane-drumkits-rock-band)
  - [5-Lane Drumkits (Guitar Hero)](#5-lane-drumkits-guitar-hero)
  - [DJ Hero Turntables](#dj-hero-turntables)
  - [Rock Band Pro Guitars](#rock-band-pro-guitars)
  - [Rock Band Pro Keyboards](#rock-band-pro-keyboards)
  - [Rock Band MIDI Pro Adapters](#rock-band-midi-pro-adapters)
  - [Rock Band Stage Kits](#rock-band-stage-kits)
  - [Rock Band Legacy Adapters](#rock-band-legacy-adapters)
- [License](#license)

## Usage

After installation, this package integrates and operates on its own. No manual initialization is required.

Device layouts from this package are used just like the built-in ones. You can use them in input actions, poll them manually through the provided static `current` and `all` properties on each layout, receive low-level `InputEventPtr`s through `InputState.onChange`, etc.

> [!WARNING]
> Usage of `InputSystem.onEvent` for low-level state handling is *not* recommended, as many layouts in this package have their own manual state handling requirements (see [`IInputStateCallbackReceiver`](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/api/UnityEngine.InputSystem.LowLevel.IInputStateCallbackReceiver.html)) which will be bypassed if you use it. Use `InputState.onChange` or `IInputStateChangeMonitor`s instead. I've done my best to keep compatibility where I can, but certain spots are just inherently incompatible, and there's not much I can do.
>
> In addition, some special handling will be needed for `InputSystem.onDeviceChange` with regards to the variant device system that this package uses for certain devices. Any devices being `InputDeviceChange.Added` where `device.enabled` is false must be ignored, as these are the container devices used to enable variance. An example of this is provided in the [DeviceConnectionHandler sample](Samples~/DeviceConnectionHandler/DeviceConnectionHandler.cs).

This package has no dependencies, however I highly recommend installing the [HIDrogen](https://github.com/TheNathannator/HIDrogen) package in addition to this one, as it helps provide additional controller support that Unity does not provide on its own.

### Notes

- Unity 2022.2 and onward do not natively support XInput instruments. This can be addressed by using v0.4.0 or higher of [HIDrogen](https://github.com/TheNathannator/HIDrogen).
- Pre-compiling the layouts from this package is not currently possible without breaking things. A large number of them have to determine or construct things at runtime, and pre-compilation circumvents the mechanisms that allow these layouts to determine things in the first place.
  - At some point I could consider looking into workarounds for this (e.g. initializing everything in an `OnAdded` override), but for the time being it's not a priority. (PRs welcome if you can get it worked out!)
- Xbox 360/One controllers will only work properly on Windows currently. Mac and Linux require custom drivers or support through raw USB handling.
- Xbox One support on Windows requires the [HIDrogen](https://github.com/TheNathannator/HIDrogen) package to be installed alongside this one. No errors will occur without it, but they will not be detected by the input system otherwise.

### Configuration

Some configuration is available through compile defines:

- `PLASTICBAND_VERBOSE_LOGGING`: Enables some potentially repetitive logging to help debug issues with devices.
- `PLASTICBAND_DEBUG_CONTROLS`: Enables verbose logging for custom control types to help debug issues with devices.
  - Independent from verbose logging, since this is strictly meant for active development and debugging.

## Supported Devices

### 5-Fret Guitars

#### Guitar Hero

- [x] Xbox 360
- [ ] PS2
  - Likely won't be possible to support directly, PS2 adapters are all over the place and usually show up as PS3 controllers.
- [x] PS3
- [ ] Wii
- [x] Santroller

#### Rock Band

- [x] Xbox 360
- [x] Xbox One
- [x] PS3
- [x] PS4
- [x] Wii
- [x] Santroller

TODO: Auto-calibration sensors; not currently possible due to Unity not supporting HID feature reports.

#### Riffmaster

- [x] Xbox One
- [x] PS4/5

#### Other

- [x] Guitar Praise

### 6-Fret Guitars

- [x] Xbox 360
- [x] Xbox One
- [x] PS3/Wii U
- [x] PS4
- [ ] iOS
- [x] Santroller

### 4-Lane Drumkits (Rock Band)

- [x] Xbox 360
- [x] Xbox One
- [x] PS2/PS3
- [x] PS4
- [x] Wii
- [x] Santroller

### 5-Lane Drumkits (Guitar Hero)

- [x] Xbox 360
- [x] PS2/PS3
- [ ] Wii
- [x] Santroller

### DJ Hero Turntables

- [x] Xbox 360
- [x] PS3
- [ ] Wii
- [x] Santroller

### Rock Band Pro Guitars

- [x] Xbox 360
  - Tilt on this model is unsupported unfortunately, due to not being reported through XInput.
- [x] PS3
- [x] Wii

TODO: Auto-calibration sensors; not currently possible due to Unity not supporting HID feature reports.

### Rock Band Pro Keyboards

- [x] Xbox 360
- [x] PS3
- [x] Wii
- [ ] Santroller

### Rock Band MIDI Pro Adapters

- [x] Xbox 360
- [x] PS3
- [x] Wii

### Rock Band Stage Kits

- [x] Xbox 360
- [x] Santroller

### Rock Band Legacy Adapters

- [x] Wired (Xbox One)
- [x] Wireless (Xbox One)

## License

This project is licensed under the MIT license. See [LICENSE.md](LICENSE.md) for details.

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
- Xbox 360/One support outside Windows depends on drivers or raw USB handling.
  The instrument profiles described below also support Linux evdev input.
- Xbox One support on Windows requires the [HIDrogen](https://github.com/TheNathannator/HIDrogen) package to be installed alongside this one. No errors will occur without it, but they will not be detected by the input system otherwise.

#### Linux evdev Input

With the corresponding evdev-enabled HIDrogen checkout installed, supported
instruments can use Linux event devices as an input fallback.
Hidraw remains preferred when usable; no application-side selection is needed.
Mappings target native Santroller HID instruments and the instrument identities exposed by `xpad` and `hid-sony`
(`sony` in sysfs), not ordinary PlayStation or Xbox gamepads.

| Driver / family | Evdev input |
| --- | --- |
| xpad GH/RB five-fret guitars and bass | Existing guitar translation, including solo frets |
| xpad GH Live six-fret guitar | Known Xbox dongle identity (`1430:079B`); generic guitar subtype alone is ambiguous |
| xpad GH/RB drums | Existing drum translation; known wired identities or distinct wireless GH/RB names |
| xpad DJ Hero turntable | Platter buttons/axes, crossfader, effects dial and navigation |
| xpad pro guitar / keyboard | Available packed gameplay data reconstructed from xpad axes |
| hid-generic Santroller native HID (legacy and current firmware) | GH/RB five-fret guitars, GH Live, GH/RB drums with velocities, and DJ Hero turntable |
| hid-generic Santroller native HID (current firmware) | Pro guitar and keyboard using their exposed packed XInput-compatible controls |
| hid-sony PS3 GH/RB guitars, including registered Wii RB guitars | Frets, navigation and exposed guitar axes |
| hid-sony PS3/Wii U and PS4 GH Live | Six frets, strum, whammy, tilt and navigation; kernel handles keepalive |
| hid-sony PS3/Wii RB drums and PS3 GH drums | Digital pad/cymbal/kick hits and navigation; **no hit velocity** |
| hid-sony PS3 DJ Hero turntable | Platter buttons/axes, crossfader, effects dial and menu buttons; D-pad is filtered by the driver |
| hid-sony PS4 RB guitars, PS4/PS5 Riffmaster and CRKD guitars | Shared fret buttons with solo routing, whammy, tilt and navigation; Riffmaster-style joystick and P1 where exposed |

Both older sequential HID-button mappings and newer hid-sony instrument mappings
are accepted. Xpad's Y-axis inversion is reversed before translation.
Native Santroller axes are not inverted. Legacy HID (`1209:2882`) uses the
instrument type in the USB version. Current firmware (`1209:2882` or
`1209:2883`) must expose a vendor-specific USB identity descriptor after each
gamepad interface's endpoints: `09 FF 53 41 4E 54 02 TT CC` (hexadecimal),
where `SANT` is the signature, `02` is the protocol version, and `TT` / `CC`
are the instrument type / capability flags. HIDrogen reads the cached sysfs
USB descriptors without accessing hidraw. Interface/profile names are unchanged.
Older current-generation firmware lacking
this metadata remains unmatched rather than guessing from a shared descriptor.
This per-interface identification supports composite devices with different
instrument profiles. Native Stage Kit and legacy pro guitar/keyboard modes
remain unmatched; output/haptics are not available through evdev. Current
native pro keyboard shares the XInput layout's touch-strip/velocity limitations.

These are input-only layouts. Evdev does not provide the raw HID output/feature
commands, nor recover fields discarded or filtered by the kernel driver.
In particular:

- Sony drum hits are reported at full digital strength, not measured velocity.
- PS4/PS5 guitars cannot report independent simultaneous normal/solo fret sets or
  the pickup selector through these mappings. The shared buttons are routed using
  the solo flag. Riffmaster joystick click is omitted because it overlaps that flag;
  touchpad-only P1 input is not exposed.
- PS3/Wii pro guitars and keyboards remain unmatched: the driver does not preserve
  enough of their packed gameplay report. Their existing raw HID support is unchanged.
- Wireless xpad identification requires the instrument names supplied by the
  enhanced wireless-device driver. Older drivers that report generic controller
  identities are left unmatched. Product IDs alone cannot recover the subtype,
  since real product IDs can replace subtype-derived IDs.
- Wired drum subtype alone does not distinguish GH from RB; a known identity is
  required. Stage Kit output is not available through this input-only backend.
- XInput pro keyboard touch strip is absent from the source report; individual
  key-velocity controls remain unexposed, as in the existing XInput layout.

Available controls depend on the descriptors and mappings of the installed kernel.
An instrument's registration does not imply raw-HID feature parity.

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

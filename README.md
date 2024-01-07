# PlasticBand-Unity

An add-on for the [Unity InputSystem](https://github.com/Unity-Technologies/InputSystem) package that provides mappings and classes for Rock Band and Guitar Hero peripherals.

This project is a companion to the [PlasticBand](https://github.com/TheNathannator/PlasticBand) repository. Documentation on how to interface with the peripherals in a general, non-Unity context can be found there.

This package is a work-in-progress. Contributions are welcome!

## Usage

After installation, this package integrates and operates on its own. No manual initialization is required.

Device layouts from this package are used just like the built-in ones. You can use them in input actions, poll them manually through the provided static `current` and `all` properties on each layout, receive low-level `InputEventPtr`s through `InputState.onChange`, etc.

> [!WARNING]
> Usage of `InputSystem.onEvent` for low-level state handling is *not* recommended, as many layouts in this package have their own manual state handling requirements (see `IInputStateCallbackReceiver`) which will be bypassed if you use it. Use `InputState.onChange` or `IInputStateChangeMonitor`s instead.
>
> I've done my best to keep compatibility where I can, but certain spots are just inherently incompatible, and there's not much I can do.

### Notes

- **Unity 2022.2 currently has issues with Xbox 360 controllers on Windows, and only the standard gamepad controllers will be picked up.** The only workarounds currently are to downgrade Unity or write your own XInput backend for the input system.
- HID devices on Windows and Mac should work without any issues. However, Linux requires special implementation, as the native backend there just uses SDL for inputs rather than providing raw HID data. Because of this, I created the [HIDrogen](https://github.com/TheNathannator/HIDrogen) package to go alongside this project.
- Xbox 360 devices can only be confirmed working on Windows currently. Mac does not support them, and the driver that was made to support them is no longer in development. Linux will likely need special layouts, but it is theoretically doable.
- Xbox One instruments are not natively supported on any platform, and are not currently supported through this package. Like with Xbox 360 devices, these also cannot be supported on Mac currently, unless there is some way to interact with them without the need for a special kernel driver.
- Pre-compiling the layouts from this package is not currently possible without breaking things. A large number of them have to determine or construct things at runtime, and pre-compilation circumvents the mechanisms that allow these layouts to determine things in the first place.
  - At some point I could consider looking into workarounds for this (e.g. initializing everything in an `OnAdded` override), but for the time being it's not a priority. (PRs welcome if you can get it worked out!)

### Configuration

Some configuration is available through compile defines:

- `PLASTICBAND_VERBOSE_LOGGING`: Enables certain, more potentially repetitive logging to help debug issues with devices.
  - Currently, only enables error logging for sending device commands.
- `PLASTICBAND_DEBUG_CONTROLS`: Enables verbose logging for custom control types to help debug issues with devices.
  - Independent from verbose logging, since this is strictly meant for active development and debugging.

## Installing

### From the Releases Page

See the [Unity documentation](https://docs.unity3d.com/Manual/upm-ui-local.html) for full details.

1. Download the .tgz file from the [latest release](https://github.com/TheNathannator/PlasticBand-Unity/releases/latest).
2. Open the Unity Package Manager and hit the + button, then pick `Add package from tarball`.
3. Select the downloaded .tgz file in the file prompt.

To update, repeat with the new .tgz.

### From Git

See the [Unity documentation](https://docs.unity3d.com/Manual/upm-git.html) for full details.

#### Via URL

1. Open the Unity Package Manager and hit the + button, then select `Add package from git URL`.
2. Paste in `https://github.com/TheNathannator/PlasticBand-Unity.git?path=/Packages/com.thenathannator.plasticband#v0.2.3` and hit Add.

To update, increment the version number at the end of the URL to the new version number and repeat these steps with the new URL. Alternatively, you can edit the URL listed in your `manifest.json` file as described in the [Via Manifest](#via-manifest) section.

#### Via Manifest

In your Packages > `manifest.json` file, add the following line to your `dependencies`:

```diff
{
  "dependencies": {
+   "com.thenathannator.plasticband": "https://github.com/TheNathannator/PlasticBand-Unity.git?path=/Packages/com.thenathannator.plasticband#v0.2.3"
  }
}
```

To update, increment the version number at the end of the URL to the new version number. The package manager will automatically pull the new changes upon regaining focus.

#### Via Cloning

1. Clone this repository to somewhere on your system.
2. Go to the Unity Package Manager and hit the + button, then pick `Add package from disk`.
3. Navigate to the `Packages` > `com.thenathannator.plasticband` folder inside the clone and select the `package.json` file.

To update, pull the latest commits. Unity will detect the changes automatically.

## TODO List

Devices to support:

- Guitar Hero 5-Fret Guitars
  - [x] Xbox 360
  - [ ] PS2
    - Likely won't be possible to support directly, PS2 adapters are all over the place and usually show up as PS3 controllers.
  - [x] PS3
  - [ ] Wii
- Rock Band 5-Fret Guitars
  - All:
    - [ ] Auto-calibration sensors
  - [x] Xbox 360
  - [ ] Xbox One
  - [x] PS3
  - [x] PS4
  - [x] Wii
- 6-Fret Guitars
  - [x] Xbox 360
  - [ ] Xbox One
  - [x] PS3/Wii U
  - [x] PS4
  - [ ] iOS
- Rock Band Kits
  - [x] Xbox 360
  - [ ] Xbox One
  - [x] PS2/PS3
  - [x] PS4
  - [x] Wii
- Guitar Hero Kits
  - [x] Xbox 360
  - [x] PS2/PS3
  - [ ] Wii
- Turntables
  - [x] Xbox 360
  - [x] PS3
  - [ ] Wii
- Rock Band Keyboards
  - All:
    - [ ] Pair velocities to keypresses
  - [x] Xbox 360
  - [x] PS3
  - [x] Wii
- Rock Band Pro Guitars
  - All:
    - [ ] Auto-calibration sensors
  - [x] Xbox 360
    - Some things are missing but either that's outside of our control, or it doesn't matter very much and just needs a bit more research.
  - [x] PS3
  - [x] Wii
- Rock Band MIDI Pro Adapter
  - [x] Xbox 360
  - [x] PS3
  - [x] Wii

## License

This project is licensed under the MIT license. See [LICENSE.md](LICENSE.md) for details.

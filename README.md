# PlasticBand-Unity

An add-on for the [Unity InputSystem](https://github.com/Unity-Technologies/InputSystem) package that provides mappings and classes for Rock Band and Guitar Hero peripherals.

This project is a companion to the [PlasticBand](https://github.com/TheNathannator/PlasticBand) repository. Documentation on how to interface with the peripherals in a general, non-Unity context can be found there.

This package is a work-in-progress. Contributions are welcome!

## Notes

- **Unity 2022.2 currently has issues with Xbox 360 controllers on Windows, and only the standard gamepad controllers will be picked up.** The only workarounds currently are to downgrade Unity or write your own XInput backend for the input system.
- HID devices on Windows and Mac should work without any issues. However, Linux requires special implementation, as the native backend there just uses SDL for inputs rather than providing raw HID data. Because of this, I created the [HIDrogen](https://github.com/TheNathannator/HIDrogen) package to go alongside this project.
- Xbox 360 devices can only be confirmed working on Windows currently. Mac does not support them, and the driver that was made to support them is no longer in development. Linux will likely need special layouts, but it is theoretically doable.
- Xbox One instruments are not natively supported on any platform, and are not currently supported through this package. Like with Xbox 360 devices, these also cannot be supported on Mac currently, unless there is some way to interact with them without the need for a special kernel driver.

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
2. Paste in `https://github.com/TheNathannator/PlasticBand-Unity.git?path=/Packages/com.thenathannator.plasticband#v0.1.2` and hit Add.

To update, just repeat these steps with the same URL, and the package manager will automatically update from the latest Git commit.

#### Via Cloning

1. Clone this repository to somewhere on your system.
2. Go to the Unity Package Manager and hit the + button, then pick `Add package from disk`.
3. Navigate to the `Packages` > `com.thenathannator.plasticband` folder inside the clone and select the `package.json` file.

To update, pull the latest commits. Unity will detect the changes automatically.

#### Via Manifest

In your Packages > `manifest.json` file, add the following line to your `dependencies`:

```diff
{
  "dependencies": {
+   "com.thenathannator.plasticband": "https://github.com/TheNathannator/PlasticBand-Unity.git?path=/Packages/com.thenathannator.plasticband#v0.1.2"
  }
}
```

To update, go into Packages > `package-lock.json` and remove the `hash` field from the package listing, along with the comma on the preceding field:

```diff
{
  "dependencies": {
    "com.thenathannator.plasticband": {
      "version": "https://github.com/TheNathannator/PlasticBand-Unity.git?path=/Packages/com.thenathannator.plasticband#v0.1.2",
      "depth": 0,
      "source": "git",
      "dependencies": {
        ...
-     }, // It is *important* that you remove the comma here! The package manager will error out otherwise
-     "hash": ...
+     }
    }
  }
}
```

## TODO List

Devices to support:

- Guitar Hero 5-Fret Guitars
  - [x] Xbox 360
  - [ ] PS2
    - Likely won't be possible to support directly, PS2 adapters are all over the place and usually show up as PS3 controllers.
  - [x] PS3
  - [ ] Wii
- Rock Band 5-Fret Guitars
  - [x] Xbox 360
    - [x] Controls
    - [ ] Auto-calibration sensors
  - [ ] Xbox One
  - [x] PS3
    - [x] Controls
    - [ ] Auto-calibration sensors
  - [x] PS4
    - [x] Controls
    - [ ] Auto-calibration sensors
  - [x] Wii
    - [x] Controls
    - [ ] Auto-calibration sensors
- 6-Fret Guitars
  - [x] Xbox 360
  - [ ] Xbox One
  - [x] PS3/Wii U
  - [x] PS4
  - [ ] iOS
- Rock Band Kits
  - [x] Xbox 360
    - [x] Controls and velocity
    - [ ] Pair velocities to hits
  - [ ] Xbox One
  - [x] PS2/PS3
    - [x] Controls and velocity
    - [ ] Pair velocities to hits
  - [ ] PS4
  - [x] Wii
    - [x] Controls and velocity
    - [ ] Pair velocities to hits
- Guitar Hero Kits
  - [x] Xbox 360
    - [x] Controls and velocity
    - [ ] Pair velocities to hits
  - [x] PS2/PS3
    - [x] Controls and velocity
    - [ ] Pair velocities to hits
  - [ ] Wii
- Turntables
  - [x] Xbox 360
  - [x] PS3
  - [ ] Wii
- Rock Band Keyboards
  - [x] Xbox 360
    - [x] Controls and velocity
    - [ ] Pair velocities to keypresses
  - [ ] PS3
  - [ ] Wii
- Rock Band Pro Guitars
  - [x] Xbox 360
    - Some things are missing but either that's outside of our control, or it doesn't matter very much and just needs a bit more research.
  - [x] PS3
    - [x] Controls
    - [ ] Auto-calibration sensors
  - [x] Wii
    - [x] Controls
    - [ ] Auto-calibration sensors

Unity will automatically restore from the latest Git commit upon regaining focus.

## License

This project is licensed under the MIT license. See [LICENSE.md](LICENSE.md) for details.

# PlasticBand-Unity

An add-on for the [Unity InputSystem](https://github.com/Unity-Technologies/InputSystem) package that provides mappings and classes for Rock Band and Guitar Hero peripherals.

This project is a companion to the [PlasticBand](https://github.com/TheNathannator/PlasticBand) repository. Documentation on how to interface with the peripherals in a general, non-Unity context can be found there.

Some notes:

- For now, only Windows support can be confirmed. Other platforms may require special attention for devices.
- HID devices such as the PS3 guitars and drums will most likely work on macOS, but Linux will probably need special implementation, as the native input backend uses SDL instead of HID on Linux.
- Xbox 360/One guitars on macOS will never be usable unless a new driver is made for them, or if there's some way to interact with them on a lower level without any special drivers.

## WIP

This package is a work-in-progress. Contributions are welcome!

## TODO

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
  - [ ] PS4
  - [ ] Wii
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
  - [ ] Wii
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
  - [ ] PS3
  - [ ] Wii

## Installing

### From the Releases Page

1. Download the .tgz file from the [latest release](https://github.com/TheNathannator/PlasticBand-Unity/releases/latest).
2. Open the Unity Package Manager and hit the + button, then pick `Add package from tarball`.
3. Select the downloaded .tgz file in the file prompt.

To update, repeat with the new .tgz.

### From Git via URL

1. Open the Unity Package Manager and hit the + button, then select `Add package from git URL`.
2. Paste in `https://github.com/TheNathannator/PlasticBand-Unity.git?path=/Packages/com.thenathannator.plasticband` and hit Add.

To update, just repeat these steps with the same URL, and the package manager will automatically update from the latest Git commit.

### From Git via Cloning

1. Clone this repository to somewhere on your system.
2. Go to the Unity Package Manager and hit the + button, then pick `Add package from disk`.
3. Navigate to the `Packages` > `com.thenathannator.plasticband` folder inside the clone and select the `package.json` file.

To update, pull the latest commits. Unity will detect the changes automatically.

### From Git via Manifest

In your Packages > `manifest.json` file, add the following line to your `dependencies`:

```diff
{
  "dependencies": {
+   "com.thenathannator.plasticband": "https://github.com/TheNathannator/PlasticBand-Unity.git?path=/Packages/com.thenathannator.plasticband"
  }
}
```

To update, go into Packages > `package-lock.json` and remove the `hash` field from the package listing, along with the comma on the preceding field:

```diff
{
  "dependencies": {
    "com.thenathannator.plasticband": {
      "version": "https://github.com/TheNathannator/PlasticBand-Unity.git?path=/Packages/com.thenathannator.plasticband",
      "depth": 0,
      "source": "git",
      "dependencies": {
        "com.unity.inputsystem": "1.4.4"
-     }, // It is *important* that you remove the comma here! The package manager will error out otherwise
-     "hash": "506eab2dff57d2d3436fec840dcc85a12d4f6062"
+     }
    }
  }
}
```

Unity will automatically restore from the latest Git commit upon regaining focus.

## License

This project is licensed under the MIT license. See [LICENSE.md](LICENSE.md) for details.

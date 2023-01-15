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

## License

This project is licensed under the MIT license. See [LICENSE.md](LICENSE.md) for details.

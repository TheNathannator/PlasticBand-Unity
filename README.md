# PlasticBand-Unity

A package for the [Unity InputSystem](https://github.com/Unity-Technologies/InputSystem) package that provides mappings and classes for Rock Band and Guitar Hero peripherals.

This project is a companion to the [PlasticBand](https://github.com/TheNathannator/PlasticBand) repository. Documentation on how to interface with the peripherals in a general, non-Unity context can be found there.

## WIP

This package is a work-in-progress. Contributions are welcome!

For the initial stage of this project, only Windows support can be confirmed. HID devices such as the PS3 guitars/drums will likely work on all platforms once implemented, but non-HID devices such as Xbox 360 guitars will require separate implementations for each platforms, and Mac support for Xbox 360/One guitars most likely isn't possible at all unless a new driver gets made for those.

## TODO

Things that need to be done.

Devices to support:

- [ ] 5-Fret Guitars
  - [x] Xbox 360
  - [ ] Xbox One
  - [ ] PS2 (Likely won't be possible to support directly, PS2 adapters are all over the place and usually show up as PS3 controllers)
  - [ ] PS3
  - [ ] PS4
  - [ ] Wii
- [ ] 6-Fret Guitars
  - [x] Xbox 360
  - [ ] Xbox One
  - [ ] PS3
  - [ ] PS4
  - [ ] Wii U
  - [ ] iOS
- [ ] Rock Band Kits
  - [ ] Xbox 360
  - [ ] Xbox One
  - [ ] PS2/PS3
  - [ ] PS4
  - [ ] Wii
- [ ] Guitar Hero Kits
  - [ ] Xbox 360
  - [ ] PS2/PS3
  - [ ] Wii
- [ ] Turntables
  - [x] Xbox 360
  - [ ] PS3
  - [ ] Wii
- [ ] Rock Band Keyboards
  - [ ] Xbox 360
  - [ ] PS3
  - [ ] Wii
- [ ] Rock Band Pro Guitars
  - [ ] Xbox 360
  - [ ] PS3
  - [ ] Wii

## License

This project is licensed under the MIT license. See [LICENSE.md](LICENSE.md) for details.

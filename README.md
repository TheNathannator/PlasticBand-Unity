# PlasticBand-Unity

[![openupm](https://img.shields.io/npm/v/com.thenathannator.plasticband?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.thenathannator.plasticband/)

An add-on for the [Unity InputSystem](https://github.com/Unity-Technologies/InputSystem) package that provides mappings and classes for Rock Band and Guitar Hero peripherals.

This project is a companion to the [PlasticBand](https://github.com/TheNathannator/PlasticBand) repository. Documentation on how to interface with the peripherals in a general, non-Unity context can be found there.

This package has no dependencies, however I highly recommend installing the [HIDrogen](https://github.com/TheNathannator/HIDrogen) package in addition to this one, as it helps provide additional controller support that Unity does not provide on its own.

For more package details, such as usage notes and supported devices, refer to the inner [package README](Packages/com.thenathannator.plasticband/README.md).

## Installing

### From OpenUPM

Refer to the OpenUPM documentation:

- [Unity Editor instructions](https://openupm.com/docs/getting-started.html)
- [OpenUPM-CLI instructions](https://openupm.com/docs/getting-started-cli.html)

The OpenUPM package page is [located here](https://openupm.com/packages/com.thenathannator.plasticband).

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
2. Paste in `https://github.com/TheNathannator/PlasticBand-Unity.git?path=/Packages/com.thenathannator.plasticband#v0.8.5` and hit Add.

To update, increment the version number at the end of the URL to the new version number and repeat these steps with the new URL. Alternatively, you can edit the URL listed in your `manifest.json` file as described in the [Via Manifest](#via-manifest) section.

#### Via Manifest

In your Packages > `manifest.json` file, add the following line to your `dependencies`:

```diff
{
  "dependencies": {
+   "com.thenathannator.plasticband": "https://github.com/TheNathannator/PlasticBand-Unity.git?path=/Packages/com.thenathannator.plasticband#v0.8.5"
  }
}
```

To update, increment the version number at the end of the URL to the new version number. The package manager will automatically pull the new changes upon regaining focus.

#### Via Cloning

1. Clone this repository to somewhere on your system.
2. Go to the Unity Package Manager and hit the + button, then pick `Add package from disk`.
3. Navigate to the `Packages` > `com.thenathannator.plasticband` folder inside the clone and select the `package.json` file.

To update, pull the latest commits. Unity will detect the changes automatically.

## License

This project is licensed under the MIT license. See [LICENSE.md](LICENSE.md) for details.

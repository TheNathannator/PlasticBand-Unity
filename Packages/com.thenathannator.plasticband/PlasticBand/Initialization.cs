using PlasticBand.Controls;
using PlasticBand.Devices;
using PlasticBand.LowLevel;
using UnityEditor;
using UnityEngine;

namespace PlasticBand
{
    /// <summary>
    /// Handles initialization of the package.
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    internal static class Initialization
    {
#if UNITY_EDITOR
        static Initialization()
        {
            Initialize();
        }
#endif

        /// <summary>
        /// Initializes everything.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void Initialize()
        {
            // General controls
            ButtonAxisPairControl.Initialize();
            MaskButtonControl.Initialize();

            // 5-fret guitars
            FiveFretGuitar.Initialize();

            // Guitar Hero guitars
            GuitarHeroSliderControl.Initialize();
            GuitarHeroGuitar.Initialize();
            XInputGuitarHeroGuitar.Initialize();
            PS3GuitarHeroGuitar.Initialize();
            SantrollerHIDGuitarHeroGuitar.Initialize();
            SantrollerXInputGuitarHeroGuitar.Initialize();

            // Rock Band guitars
            RockBandGuitar.Initialize();
            XInputRockBandGuitar.Initialize();
            PS3RockBandGuitar.Initialize();
            PS4RockBandGuitar.Initialize();
            WiiRockBandGuitar.Initialize();
            SantrollerHIDRockBandGuitar.Initialize();
            SantrollerXInputRockBandGuitar.Initialize();

            // 6-fret guitars
            SixFretGuitar.Initialize();
            XInputGuitarGHL.Initialize();
            PS3WiiUSixFretGuitar.Initialize();
            PS4SixFretGuitar.Initialize();
            SantrollerHIDSixFretGuitar.Initialize();
            SantrollerXInputGuitarGHL.Initialize();

            // Pro Guitars
            ProGuitar.Initialize();
            XInputProGuitar.Initialize();
            PS3ProGuitar.Initialize();
            WiiProGuitar.Initialize();

            // 4-lane drumkits
            FourLanePadsControl.Initialize();
            FourLaneDrumkit.Initialize();
            XInputFourLaneDrumkit.Initialize();
            PS3FourLaneDrumkit.Initialize();
            PS4FourLaneDrumkit.Initialize();
            WiiFourLaneDrumkit.Initialize();
            SantrollerHIDFourLaneDrumkit.Initialize();
            SantrollerXInputFourLaneDrumkit.Initialize();

            // 5-lane drumkits
            FiveLaneDrumkit.Initialize();
            XInputFiveLaneDrumkit.Initialize();
            PS3FiveLaneDrumkit.Initialize();
            SantrollerHIDFiveLaneDrumkit.Initialize();
            SantrollerXInputFiveLaneDrumkit.Initialize();

            // Turntables
            Turntable.Initialize();
            XInputTurntable.Initialize();
            PS3Turntable.Initialize();
            SantrollerHIDTurntable.Initialize();
            SantrollerXInputTurntable.Initialize();

            // Keytars
            Keytar.Initialize();
            XInputKeytar.Initialize();
            PS3Keytar.Initialize();
            WiiKeytar.Initialize();

            SantrollerLayoutFinder.Initialize();

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            // XInput layout fix-ups for devices that require state information to determine the true type
            XInputLayoutFixup.Initialize();
#endif
        }
    }
}
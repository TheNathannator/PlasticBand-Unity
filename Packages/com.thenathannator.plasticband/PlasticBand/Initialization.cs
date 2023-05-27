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

#if !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        internal static void Initialize()
        {
            // Layout finders
            HidLayoutFinder.Initialize();
            XInputLayoutFinder.Initialize();
            // SantrollerLayoutFinder.Initialize(); // Temporarily disabled to avoid a crash bug
            // this time it's not even related to the devices this is supposed to support lol

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
            // PS4RockBandGuitar.Initialize(); // Temporarily disabled to avoid a crash bug
            WiiRockBandGuitar.Initialize();
            SantrollerHIDRockBandGuitar.Initialize();
            SantrollerXInputRockBandGuitar.Initialize();

            // 6-fret guitars
            SixFretGuitar.Initialize();
            XInputSixFretGuitar.Initialize();
            PS3WiiUSixFretGuitar.Initialize();
            // PS4SixFretGuitar.Initialize(); // Temporarily disabled to avoid a crash bug
            SantrollerHIDSixFretGuitar.Initialize();
            SantrollerXInputSixFretGuitar.Initialize();

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
            // PS4FourLaneDrumkit.Initialize(); // Temporarily disabled to avoid a crash bug
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

            // Pro Keyboards
            ProKeyboard.Initialize();
            XInputProKeyboard.Initialize();
            PS3ProKeyboard.Initialize();
            WiiProKeyboard.Initialize();

            // Rock Band stage kit
            StageKit.Initialize();
            XInputStageKit.Initialize();
        }
    }
}
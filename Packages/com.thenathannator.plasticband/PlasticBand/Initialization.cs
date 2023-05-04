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

            // Rock Band guitars
            RockBandGuitar.Initialize();
            XInputRockBandGuitar.Initialize();
            PS3RockBandGuitar.Initialize();
            // PS4RockBandGuitar.Initialize(); // Temporarily disabled to avoid a crash bug
            WiiRockBandGuitar.Initialize();

            // 6-fret guitars
            SixFretGuitar.Initialize();
            XInputSixFretGuitar.Initialize();
            PS3WiiUSixFretGuitar.Initialize();
            // PS4SixFretGuitar.Initialize(); // Temporarily disabled to avoid a crash bug

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

            // 5-lane drumkits
            FiveLaneDrumkit.Initialize();
            XInputFiveLaneDrumkit.Initialize();
            PS3FiveLaneDrumkit.Initialize();

            // Turntables
            Turntable.Initialize();
            XInputTurntable.Initialize();
            PS3Turntable.Initialize();

            // Pro Keyboards
            ProKeyboard.Initialize();
            XInputProKeyboard.Initialize();
            PS3ProKeyboard.Initialize();
            WiiProKeyboard.Initialize();

            // Layout finders
            HidReportIdLayoutFinder.Initialize();
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            XInputLayoutFixup.Initialize();
#endif
        }
    }
}
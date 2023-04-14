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
            GuitarHeroSliderControl.Initialize();
            FiveFretGuitar.Initialize();
            GuitarHeroGuitar.Initialize();
            RockBandGuitar.Initialize();
            XInputGuitar.Initialize();
            XInputGuitarAlternate.Initialize();
            PS3GuitarHeroGuitar.Initialize();
            PS3RockBandGuitar.Initialize();
            WiiRockBandGuitar.Initialize();

            // 6-fret guitars
            SixFretGuitar.Initialize();
            XInputGuitarGHL.Initialize();
            PS3WiiUSixFretGuitar.Initialize();
            PS4SixFretGuitar.Initialize();

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

            // 5-lane drumkits
            FiveLaneDrumkit.Initialize();
            XInputFiveLaneDrumkit.Initialize();
            PS3FiveLaneDrumkit.Initialize();

            // Turntables
            Turntable.Initialize();
            XInputTurntable.Initialize();
            PS3Turntable.Initialize();

            // Keytars
            Keytar.Initialize();
            XInputKeytar.Initialize();

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            // XInput layout fix-ups for devices that require state information to determine the true type
            XInputLayoutFixup.Initialize();
#endif
        }
    }
}
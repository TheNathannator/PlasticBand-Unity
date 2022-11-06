using PlasticBand.Controls;
using PlasticBand.Devices;
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void Initialize()
        {
            // Controls
            MaskButtonControl.Initialize();

            // Guitars
            FiveFretGuitar.Initialize();
            GuitarHeroGuitar.Initialize();
            XInputGuitarAlternate.Initialize();
        }
    }
}
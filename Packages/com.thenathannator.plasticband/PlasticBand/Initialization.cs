using PlasticBand.Controls;
using PlasticBand.Devices;
using PlasticBand.LowLevel;
using UnityEditor;
using UnityEngine.InputSystem;

#if !UNITY_EDITOR
using UnityEngine;
#endif

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
#if UNITY_EDITOR
            AssemblyReloadEvents.beforeAssemblyReload += OnDomainReload;
#endif

            // Layout finders
            HidLayoutFinder.Initialize();
            XInputLayoutFinder.Initialize();

            // General controls
            IntegerAxisControl.Initialize();
            IntegerButtonControl.Initialize();
            MaskButtonControl.Initialize();

            // 5-fret guitars
            FiveFretGuitar.Initialize();
            GuitarPraiseGuitar.Initialize();

            // Guitar Hero guitars
            GuitarHeroSliderControl.Initialize();
            GuitarHeroGuitar.Initialize();
            XInputGuitarHeroGuitar.Initialize();
            PS3GuitarHeroGuitar.Initialize();
            SantrollerHIDGuitarHeroGuitar.Initialize();
            SantrollerXInputGuitarHeroGuitar.Initialize();

            // Rock Band guitars
            RockBandPickupSwitchControl.Initialize();
            RockBandGuitar.Initialize();
            XInputRockBandGuitar.Initialize();
            XboxOneRockBandGuitar.Initialize();
            PS3RockBandGuitar.Initialize();
            PS4RockBandGuitar.Initialize();
            WiiRockBandGuitar.Initialize();
            SantrollerHIDRockBandGuitar.Initialize();
            SantrollerXInputRockBandGuitar.Initialize();

            // Riffmaster guitars
            RiffmasterGuitar.Initialize();
            XboxOneRiffmasterGuitar.Initialize();
            PS4RiffmasterGuitar.Initialize();
            PS5RiffmasterGuitar.Initialize();

            // 6-fret guitars
            SixFretGuitar.Initialize();
            XInputSixFretGuitar.Initialize();
            XboxOneSixFretGuitar.Initialize();
            PS3WiiUSixFretGuitar.Initialize();
            PS4SixFretGuitar.Initialize();
            SantrollerHIDSixFretGuitar.Initialize();
            SantrollerXInputSixFretGuitar.Initialize();

            // Pro Guitars
            ProGuitar.Initialize();
            XInputProGuitar.Initialize();
            PS3ProGuitar.Initialize();
            WiiProGuitar.Initialize();

            // 4-lane drumkits
            FourLaneDrumkit.Initialize();
            XInputFourLaneDrumkit.Initialize();
            XboxOneFourLaneDrumkit.Initialize();
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

            // Pro Keyboards
            ProKeyboard.Initialize();
            XInputProKeyboard.Initialize();
            PS3ProKeyboard.Initialize();
            WiiProKeyboard.Initialize();

            // Rock Band stage kit
            StageKit.Initialize();
            XInputStageKit.Initialize();
            SantrollerHidStageKit.Initialize();
            SantrollerXInputStageKit.Initialize();

            // Variant devices
            XInputVariantDrumkit.Initialize();
            XboxOneWirelessLegacyAdapter.Initialize();
            XboxOneWiredLegacyAdapter.Initialize();
        }

#if UNITY_EDITOR
        internal static void OnDomainReload()
        {
            // We need to be sure any VariantDevices remove their inner devices from the input system
            // before a domain reload occurs, otherwise they persist beyond when they should
            foreach (var device in InputSystem.devices)
            {
                if (device is IDomainReloadReceiver reloadable)
                    reloadable.OnDomainReload();
            }
        }
#endif
    }
}
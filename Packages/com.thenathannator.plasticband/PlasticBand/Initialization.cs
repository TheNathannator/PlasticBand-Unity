using PlasticBand.Controls;
using PlasticBand.Devices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;

namespace PlasticBand
{
    /// <summary>
    /// Handles initialization of the package.
    /// </summary>
    internal static class Initialization
    {
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSplashScreen)]
#endif
        internal static void Initialize()
        {
#if UNITY_EDITOR
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += OnDomainReload;
#endif

            // Layout finders
            HidLayoutFinder.Initialize();
            XInputLayoutFinder.Initialize();
            SantrollerLayoutFinder.Initialize();

            // General controls
            IntegerAxisControl.Initialize();
            IntegerButtonControl.Initialize();
            MaskButtonControl.Initialize();

            // Gamepads
            XInputControllerUniversal.Initialize();
            SwitchProControllerPatches.Initialize();
            AndroidGamepad.Initialize();
            SantrollerHIDGamepad.Initialize();
            SantrollerV2HIDGamepad.Initialize();
#if HIDROGEN_0_6_0_OR_LATER
            SantrollerEvdevGamepad.Initialize();
            SantrollerV2EvdevGamepad.Initialize();
#endif

            // 5-fret guitars
            FiveFretGuitar.Initialize();
            GuitarPraiseGuitar.Initialize();

            // Guitar Hero guitars
            GuitarHeroGuitar.Initialize();
            XInputGuitarHeroGuitar.Initialize();
            PS3GuitarHeroGuitar.Initialize();
            PCGuitarHeroGuitar.Initialize();
            SantrollerHIDGuitarHeroGuitar.Initialize();
            SantrollerV2HIDGuitarHeroGuitar.Initialize();
            SantrollerXInputGuitarHeroGuitar.Initialize();
#if HIDROGEN_0_6_0_OR_LATER
            XpadEvdevGuitarHeroGuitar.Initialize();
            SantrollerEvdevGuitarHeroGuitar.Initialize();
            SantrollerV2EvdevGuitarHeroGuitar.Initialize();
            PS3EvdevGuitarHeroGuitar.Initialize();
            PCEvdevGuitarHeroGuitar.Initialize();
            WiiEvdevGuitarHeroGuitar.Initialize();
#endif

            // Rock Band guitars
            RockBandGuitar.Initialize();
            XInputRockBandGuitar.Initialize();
            XboxOneRockBandGuitar.Initialize();
            PS3RockBandGuitar.Initialize();
            PS4RockBandGuitar.Initialize();
            WiiRockBandGuitar.Initialize();
            SantrollerHIDRockBandGuitar.Initialize();
            SantrollerV2HIDRockBandGuitar.Initialize();
            SantrollerXInputRockBandGuitar.Initialize();
#if HIDROGEN_0_6_0_OR_LATER
            XpadEvdevRockBandGuitar.Initialize();
            XoneEvdevRockBandGuitar.Initialize();
            SantrollerEvdevRockBandGuitar.Initialize();
            SantrollerV2EvdevRockBandGuitar.Initialize();
            PS3EvdevRockBandGuitar.Initialize();
            PS4EvdevRockBandGuitar.Initialize();
            PS5EvdevRockBandGuitar.Initialize();
            WiiEvdevRockBandGuitar.Initialize();
#endif

            // Riffmaster guitars
            RiffmasterGuitar.Initialize();
            XboxOneRiffmasterGuitar.Initialize();
            PS4RiffmasterGuitar.Initialize();
            PS5RiffmasterGuitar.Initialize();

            // CRKD guitars
            XboxCrkdGuitar.Initialize();
            PS4CrkdGuitar.Initialize();
            PS5CrkdGuitar.Initialize();
            SwitchCrkdGuitar.Initialize();

            // 6-fret guitars
            SixFretGuitar.Initialize();
            XInputSixFretGuitar.Initialize();
            XboxOneSixFretGuitar.Initialize();
            PS3WiiUSixFretGuitar.Initialize();
            PS4SixFretGuitar.Initialize();
            SantrollerHIDSixFretGuitar.Initialize();
            SantrollerV2HIDSixFretGuitar.Initialize();
            SantrollerXInputSixFretGuitar.Initialize();
#if HIDROGEN_0_6_0_OR_LATER
            XpadEvdevSixFretGuitar.Initialize();
            SantrollerEvdevSixFretGuitar.Initialize();
            SantrollerV2EvdevSixFretGuitar.Initialize();
            PS3EvdevSixFretGuitar.Initialize();
            PS4EvdevSixFretGuitar.Initialize();
#endif

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
            PS5FourLaneDrumkit.Initialize();
            WiiFourLaneDrumkit.Initialize();
            SantrollerHIDFourLaneDrumkit.Initialize();
            SantrollerV2HIDFourLaneDrumkit.Initialize();
            SantrollerXInputFourLaneDrumkit.Initialize();
#if HIDROGEN_0_6_0_OR_LATER
            XpadEvdevFourLaneDrumkit.Initialize();
            XoneEvdevFourLaneDrumkit.Initialize();
            SantrollerEvdevFourLaneDrumkit.Initialize();
            SantrollerV2EvdevFourLaneDrumkit.Initialize();
            PS3EvdevFourLaneDrumkit.Initialize();
            WiiEvdevFourLaneDrumkit.Initialize();
#endif

            // CRKD drumkits
            XboxCrkdDrumkit.Initialize();
            PS4CrkdDrumkit.Initialize();
            PS5CrkdDrumkit.Initialize();
            SwitchCrkdDrumkit.Initialize();

            // 5-lane drumkits
            FiveLaneDrumkit.Initialize();
            XInputFiveLaneDrumkit.Initialize();
            PS3FiveLaneDrumkit.Initialize();
            SantrollerHIDFiveLaneDrumkit.Initialize();
            SantrollerV2HIDFiveLaneDrumkit.Initialize();
            SantrollerXInputFiveLaneDrumkit.Initialize();
#if HIDROGEN_0_6_0_OR_LATER
            XpadEvdevFiveLaneDrumkit.Initialize();
            SantrollerEvdevFiveLaneDrumkit.Initialize();
            SantrollerV2EvdevFiveLaneDrumkit.Initialize();
            PS3EvdevFiveLaneDrumkit.Initialize();
            WiiEvdevFiveLaneDrumkit.Initialize();
#endif

            // Turntables
            Turntable.Initialize();
            XInputTurntable.Initialize();
            PS3Turntable.Initialize();
            SantrollerHIDTurntable.Initialize();
            SantrollerV2HIDTurntable.Initialize();
            SantrollerXInputTurntable.Initialize();
#if HIDROGEN_0_6_0_OR_LATER
            XpadEvdevTurntable.Initialize();
            SantrollerEvdevTurntable.Initialize();
            SantrollerV2EvdevTurntable.Initialize();
            PS3EvdevTurntable.Initialize();
            WiiEvdevTurntable.Initialize();
#endif

            // Pro Keyboards
            ProKeyboard.Initialize();
            XInputProKeyboard.Initialize();
            PS3ProKeyboard.Initialize();
            WiiProKeyboard.Initialize();

            // Rock Band stage kit
            StageKit.Initialize();
            XInputStageKit.Initialize();
            SantrollerHIDStageKit.Initialize();
            SantrollerV2HIDStageKit.Initialize();
            SantrollerXInputStageKit.Initialize();
#if HIDROGEN_0_6_0_OR_LATER
            SantrollerEvdevStageKit.Initialize();
            SantrollerV2EvdevStageKit.Initialize();
            XpadEvdevStageKit.Initialize();
#endif

            // Variant devices
            XboxOneWirelessLegacyAdapter.Initialize();
            XboxOneWiredLegacyAdapter.Initialize();
            SantrollerHIDAdapter.Initialize();
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

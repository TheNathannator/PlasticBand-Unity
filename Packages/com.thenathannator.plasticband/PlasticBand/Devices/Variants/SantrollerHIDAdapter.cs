using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    using static HID;

    [InputControlLayout(stateType = typeof(VariantDeviceDummyState), hideInUI = true)]
    internal class SantrollerHIDAdapter : VariantSingleDevice
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerHIDAdapter>(matches: new InputDeviceMatcher()
                .WithInterface(HidDefinitions.InterfaceName)
                .WithCapability("vendorId", SantrollerLayoutFinder.VendorID)
                .WithCapability("productId", SantrollerLayoutFinder.ProductID)
            );
            InputSystem.RegisterLayout<SantrollerHIDAdapter>(matches: new InputDeviceMatcher()
                .WithInterface(HidDefinitions.InterfaceName)
                .WithCapability("vendorId", SantrollerLayoutFinder.VendorID)
                .WithCapability("productId", SantrollerLayoutFinder.ProductIDV2)
            );
        }

        private const double kRequestRetryInterval = 0.5; // seconds
        private const int kMaxRequestAttempts = 8;

        private bool m_IsV2;
        private bool m_FeaturesRequested;
        private SantrollerCapabilities m_Capabilities;

        private int m_RequestAttempts;
        private double m_NextRequestTime;

        protected override void OnAdded()
        {
            base.OnAdded();

            var _desc = description;
            var hidDescriptor = HidLayoutFinder.GetDeviceDescriptor(ref _desc, ExecuteCommand);
            m_IsV2 = IsV2Descriptor(hidDescriptor);

            if (m_IsV2)
            {
                Logging.Verbose($"[SantrollerHIDAdapter] Santroller V2 device detected. Waiting for update tick to request capabilities...");
            }
            else
            {
                if (!int.TryParse(description.version, out int version))
                {
                    version = 0;
                }

                var subType = (SantrollerDeviceType)(version >> 8);
                string layout = GetLegacyLayout(subType);
                if (!string.IsNullOrEmpty(layout))
                {
                    Logging.Verbose($"[SantrollerHIDAdapter] Creating legacy Santroller device {layout} for subtype {subType} (revision 0x{version:X4}).");
                    CreateRealDevice(layout);
                }
                else
                {
                    Logging.Warning($"[SantrollerHIDAdapter] No legacy layout available for subtype {subType} (revision 0x{version:X4}).");
                }
            }
        }

        protected override void OnNextUpdate()
        {
            base.OnNextUpdate();
            TryRequestFeatures();
        }

        protected override void OnDomainReload()
        {
            base.OnDomainReload();
            m_FeaturesRequested = false;
            m_RequestAttempts = 0;
            m_NextRequestTime = 0;
        }

        private bool IsV2Descriptor(HIDDeviceDescriptor descriptor)
        {
            int elementCount = descriptor.elements != null ? descriptor.elements.Length : 0;
            int collectionCount = descriptor.collections != null ? descriptor.collections.Length : 0;

            // Santroller V2 report descriptors include a vendor-defined usage of FF00:2882 and/or a report ID of 0x10

            if (descriptor.elements != null)
            {
                foreach (var element in descriptor.elements)
                {
                    if ((element.usagePage == UsagePage.VendorDefined && element.usage == SantrollerLayoutFinder.ProductID) ||
                        element.reportId == 0x10)
                    {
                        Logging.Verbose($"[SantrollerHIDAdapter] Found Santroller V2 via descriptor elements");
                        return true;
                    }
                }
            }

            if (descriptor.collections != null)
            {
                foreach (var collection in descriptor.collections)
                {
                    if (collection.usagePage == UsagePage.VendorDefined && collection.usage == SantrollerLayoutFinder.ProductID)
                    {
                        Logging.Verbose($"[SantrollerHIDAdapter] Found Santroller V2 via descriptor collections");
                        return true;
                    }
                }
            }

            // Only check revision if the descriptor is unavailable
            if (elementCount == 0 && collectionCount == 0)
            {
                if (!int.TryParse(description.version, out int version) || (version >> 8) == 0)
                {
                    Logging.Verbose($"[SantrollerHIDAdapter] Found Santroller V2 via version");
                    return true;
                }
            }

            Logging.Verbose($"[SantrollerHIDAdapter] Could not find Santroller V2 in descriptor, assuming legacy firmware.");
            return false;
        }

        private void TryRequestFeatures()
        {
            if (!m_IsV2 || m_RealDevice != null)
            {
                return;
            }

            double currentTime = InputState.currentTime;
            if (!m_FeaturesRequested || (currentTime >= m_NextRequestTime && m_RequestAttempts < kMaxRequestAttempts))
            {
                m_FeaturesRequested = true;
                m_RequestAttempts++;
                m_NextRequestTime = currentTime + kRequestRetryInterval;
                Logging.Verbose($"[SantrollerHIDAdapter] Requesting capabilities (attempt {m_RequestAttempts}/{kMaxRequestAttempts})...");
                RequestFeatures();
            }
        }

        private void RequestFeatures()
        {
            var command = SantrollerRequestFeaturesCommand.Create();
            long result = ExecuteCommand(ref command);
            if (result < 0)
            {
                Logging.Warning($"[SantrollerHIDAdapter] Capabilities request failed with error code: {result}");
            }
        }

        protected override unsafe void OnStateEvent(InputEventPtr eventPtr)
        {
            if (eventPtr.type != StateEvent.Type)
            {
                return;
            }

            var stateEvent = StateEvent.From(eventPtr);

            // V2 Features response report (ID 0x10)
            if (m_IsV2 && StateReader.ReadReportIdState(stateEvent, out SantrollerFeaturesResponse* features))
            {
                m_Capabilities = features->capabilities;

                string layout = GetV2Layout(features->subType);
                if (!string.IsNullOrEmpty(layout))
                {
                    Logging.Verbose($"[SantrollerHIDAdapter] Capabilities received. Creating Santroller V2 device {layout} (subtype {features->subType}, capabilities {features->capabilities})");
                    CreateRealDevice(layout);
                }
                else
                {
                    Logging.Warning($"[SantrollerHIDAdapter] Capabilities received, but no V2 layout available for subtype {features->subType} (capabilities {features->capabilities})");
                }
                return;
            }

            if (m_RealDevice == null)
            {
                if (m_IsV2)
                {
                    TryRequestFeatures();
                }
                return;
            }

            m_RealDevice.OnStateEvent(stateEvent->stateFormat, stateEvent->state, (int)stateEvent->stateSizeInBytes);
        }

        private void CreateRealDevice(string layout)
        {
            if (m_RealDevice != null)
            {
                Logging.Verbose($"[SantrollerHIDAdapter] Previous real device was not cleaned up!");
                m_RealDevice.Dispose();
            }

            m_RealDevice = VariantRealDevice.Create(this, layout);
        }

        // TODO: Use device descriptions/matchers instead of hard-coding layouts?

        private static string GetLegacyLayout(SantrollerDeviceType subType)
        {
            switch (subType)
            {
                case SantrollerDeviceType.GuitarHeroGuitar: return nameof(SantrollerHIDGuitarHeroGuitar);
                case SantrollerDeviceType.RockBandGuitar:   return nameof(SantrollerHIDRockBandGuitar);
                case SantrollerDeviceType.GuitarHeroDrums:  return nameof(SantrollerHIDFiveLaneDrumkit);
                case SantrollerDeviceType.RockBandDrums:    return nameof(SantrollerHIDFourLaneDrumkit);
                case SantrollerDeviceType.LiveGuitar:       return nameof(SantrollerHIDSixFretGuitar);
                case SantrollerDeviceType.DjHeroTurntable:  return nameof(SantrollerHIDTurntable);
                case SantrollerDeviceType.StageKit:         return nameof(SantrollerHIDStageKit);
                default:                                    return null;
            }
        }

        private static string GetV2Layout(SantrollerDeviceType subType)
        {
            switch (subType)
            {
                case SantrollerDeviceType.GuitarHeroGuitar: return nameof(SantrollerV2HIDGuitarHeroGuitar);
                case SantrollerDeviceType.RockBandGuitar:   return nameof(SantrollerV2HIDRockBandGuitar);
                case SantrollerDeviceType.GuitarHeroDrums:  return nameof(SantrollerV2HIDFiveLaneDrumkit);
                case SantrollerDeviceType.RockBandDrums:    return nameof(SantrollerV2HIDFourLaneDrumkit);
                case SantrollerDeviceType.LiveGuitar:       return nameof(SantrollerV2HIDSixFretGuitar);
                case SantrollerDeviceType.DjHeroTurntable:  return nameof(SantrollerV2HIDTurntable);
                case SantrollerDeviceType.StageKit:         return nameof(SantrollerV2HIDStageKit);
                default:                                    return null;
            }
        }
    }
}

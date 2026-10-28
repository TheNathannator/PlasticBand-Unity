using PlasticBand;
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
                .WithCapability("productId", SantrollerLayoutFinder.ProductID));
            InputSystem.RegisterLayout<SantrollerHIDAdapter>(matches: new InputDeviceMatcher()
                .WithInterface(HidDefinitions.InterfaceName)
                .WithCapability("vendorId", SantrollerLayoutFinder.VendorID)
                .WithCapability("productId", SantrollerLayoutFinder.ProductIDV2));
        }

        private const double kRequestRetryInterval = 0.5; // seconds
        private const int kMaxRequestAttempts = 8;

        private bool m_IsV2;
        private SantrollerCapabilities m_Capabilities;
        private bool m_FeaturesRequested;
        private int m_RequestAttempts;
        private double m_NextRequestTime;

        protected override void OnAdded()
        {
            base.OnAdded();

            // Detect Old vs New firmware:
            // In V2 firmware, the device exposes vendor-defined usage (page 0xFF00, usage 0x2882)
            // or Report ID 0x10 in its HID report descriptor, matching SDL's detection.
            var desc = description;
            var hidDescriptor = HidLayoutFinder.GetDeviceDescriptor(ref desc, ExecuteCommand);
            bool isV2 = HasV2Usage(hidDescriptor);

            if (isV2)
            {
                m_IsV2 = true;
                Logging.Message($"[SantrollerHIDAdapter] Device {this} detected as V2 firmware via HID descriptor. Waiting for update tick to request capabilities...");
            }
            else
            {
                m_IsV2 = false;
                int version = 0;
                int.TryParse(description.version, out version);
                var subType = (SantrollerDeviceType)(version >> 8);
                Logging.Message($"[SantrollerHIDAdapter] Device {this} detected as legacy firmware via HID descriptor (version={version:X4}, subtype={subType}).");
                CreateRealDevice(GetLegacyLayout(subType));
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
            m_RealDevice = null;
            m_FeaturesRequested = false;
            m_RequestAttempts = 0;
            m_NextRequestTime = 0;
        }

        private bool HasV2Usage(HIDDeviceDescriptor descriptor)
        {
            int elementCount = descriptor.elements != null ? descriptor.elements.Length : 0;
            int collectionCount = descriptor.collections != null ? descriptor.collections.Length : 0;
            Logging.Message($"[SantrollerHIDAdapter] Inspecting HID descriptor for {this}: {elementCount} elements, {collectionCount} collections.");

            if (descriptor.elements != null)
            {
                foreach (var element in descriptor.elements)
                {
                    if ((int)element.usagePage == 0xFF00 && element.usage == SantrollerLayoutFinder.ProductID)
                    {
                        Logging.Message($"[SantrollerHIDAdapter] Found vendor usage 0xFF00:0x{SantrollerLayoutFinder.ProductID:X4} in element (reportId=0x{element.reportId:X2}).");
                        return true;
                    }
                    if (element.reportId == 0x10)
                    {
                        Logging.Message($"[SantrollerHIDAdapter] Found Report ID 0x10 in element (usagePage={element.usagePage}, usage={element.usage}).");
                        return true;
                    }
                }
            }

            if (descriptor.collections != null)
            {
                foreach (var collection in descriptor.collections)
                {
                    if ((int)collection.usagePage == 0xFF00 && collection.usage == SantrollerLayoutFinder.ProductID)
                    {
                        Logging.Message($"[SantrollerHIDAdapter] Found vendor usage 0xFF00:0x{SantrollerLayoutFinder.ProductID:X4} in collection.");
                        return true;
                    }
                }
            }

            if (elementCount == 0 && collectionCount == 0)
            {
                int version = 0;
                bool isV2 = !int.TryParse(description.version, out version) || (version >> 8) == 0;
                Logging.Message($"[SantrollerHIDAdapter] Descriptor elements/collections unavailable, falling back to version check: version={version:X4}, isV2={isV2}");
                return isV2;
            }

            return false;
        }

        private void TryRequestFeatures()
        {
            if (!m_IsV2 || m_RealDevice != null)
                return;

            double currentTime = InputState.currentTime;
            if (!m_FeaturesRequested || (currentTime >= m_NextRequestTime && m_RequestAttempts < kMaxRequestAttempts))
            {
                m_FeaturesRequested = true;
                m_RequestAttempts++;
                m_NextRequestTime = currentTime + kRequestRetryInterval;
                Logging.Message($"[SantrollerHIDAdapter] Requesting capabilities from {this} (attempt {m_RequestAttempts}/{kMaxRequestAttempts})...");
                RequestFeatures();
            }
        }

        private void RequestFeatures()
        {
            var command = SantrollerRequestFeaturesCommand.Create();
            long result = ExecuteCommand(ref command);
            if (result < 0)
                Logging.Warning($"[SantrollerHIDAdapter] Sent capabilities request (Report ID 0x10) to {this}, but ExecuteCommand failed with error code: {result}");
            else
                Logging.Message($"[SantrollerHIDAdapter] Sent capabilities request (Report ID 0x10) to {this} (result = {result}).");
        }

        protected override unsafe void OnStateEvent(InputEventPtr eventPtr)
        {
            if (eventPtr.type != StateEvent.Type)
                return;

            var stateEvent = StateEvent.From(eventPtr);
            // Handle V2 Features response report (ID 0x10)
            if (m_IsV2 && StateReader.ReadReportIdState(stateEvent, out SantrollerFeaturesResponse* features))
            {
                Logging.Message($"[SantrollerHIDAdapter] Received capabilities response from {this}: SubType = {features->subType} ({(int)features->subType}), Capabilities = {features->capabilities}");
                m_Capabilities = features->capabilities;
                string layout = GetV2Layout(features->subType);
                if (string.IsNullOrEmpty(layout))
                    Logging.Warning($"[SantrollerHIDAdapter] No V2 layout mapping found for Santroller subtype: {features->subType} ({(int)features->subType})");
                else
                    Logging.Message($"[SantrollerHIDAdapter] Creating real device with layout: '{layout}'");

                CreateRealDevice(layout);
                return;
            }

            if (m_RealDevice == null)
            {
                if (m_IsV2)
                    TryRequestFeatures();
                return;
            }

            m_RealDevice.OnStateEvent(stateEvent->stateFormat, stateEvent->state, (int)stateEvent->stateSizeInBytes);
        }

        private void CreateRealDevice(string layout)
        {
            m_RealDevice?.Dispose();
            if (string.IsNullOrEmpty(layout))
            {
                m_RealDevice = null;
                return;
            }

            m_RealDevice = new VariantRealDevice(this, layout);
            Logging.Message($"[SantrollerHIDAdapter] Instantiated real device {m_RealDevice.device} with layout '{layout}'.");
        }

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
                case SantrollerDeviceType.StageKit:         return nameof(SantrollerHidStageKit);
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
                case SantrollerDeviceType.StageKit:         return nameof(SantrollerV2HidStageKit);
                default:                                    return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

/*
I've chosen to do this instead of compile defines because it is a greater guarantee that the device layout will be correct.

With the rising popularity of Arduino/Pico modding, there's less of a guarantee that whether or not a report ID is provided
will match what is expected. Checking for it at runtime is the best bet to ensure the proper layout is given.

While this creates some slight complexity with each of the HID devices in the codebase, it's at least something I hopefully
won't ever have to worry about again. The only compile defines I'm using for HID devices is for the default state used on
the main layout, which is only really necessary for the case where an editor domain reload makes the device receive the main
layout instead of the one determined here.
*/

namespace PlasticBand.LowLevel
{
    using static HID;

    /// <summary>
    /// Checks registered HID devices for the presence of a report ID, and picks a layout accordingly.
    /// </summary>
    internal static class HidLayoutFinder
    {
        // Get built-in HID descriptor retrieval so we don't have to copy it
        private unsafe delegate HIDDeviceDescriptor HID_ReadHIDDeviceDescriptor(ref InputDeviceDescription deviceDescription,
            InputDeviceExecuteCommandDelegate executeCommandDelegate);
        private static readonly HID_ReadHIDDeviceDescriptor s_GetDeviceDescriptor = (HID_ReadHIDDeviceDescriptor)typeof(HID)
            .GetMethod("ReadHIDDeviceDescriptor", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null,
                new Type[] { typeof(InputDeviceDescription).MakeByRefType(), typeof(InputDeviceExecuteCommandDelegate) }, null)
            .CreateDelegate(typeof(HID_ReadHIDDeviceDescriptor));

        private static readonly Dictionary<string, (string reportId, string noReportId)> s_AvailableLayouts = new Dictionary<string, (string, string)>();

        internal static void Initialize()
        {
            // Ensure no layouts have persisted across a domain reload
            s_AvailableLayouts.Clear();

            // Register layout finder
            InputSystem.onFindLayoutForDevice += FindDeviceLayout;
        }

        internal static string FindDeviceLayout(ref InputDeviceDescription description, string matchedLayout,
            InputDeviceExecuteCommandDelegate commandDelegate)
        {
            if (description.interfaceName != HidDefinitions.InterfaceName || string.IsNullOrEmpty(matchedLayout)
                || !s_AvailableLayouts.TryGetValue(matchedLayout, out var layouts))
                return null;

            var descriptor = s_GetDeviceDescriptor(ref description, commandDelegate);
            if (descriptor.elements == null || descriptor.elements.Length < 1)
                return null;

            // Any elements with a bit offset less than 8 indicates that there is no report ID
            bool hasReportId = !descriptor.elements.Where((element) => element.reportType == HIDReportType.Input)
                .Any((element) => element.reportOffsetInBits < 8);

            return hasReportId ? layouts.reportId : layouts.noReportId;
        }

        internal static void RegisterLayout<TDevice>(int vendorId, int productId)
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>(matches: GetMatcher(vendorId, productId));
        }

        internal static void RegisterLayout<TReportId, TNoReportId>(int vendorId, int productId, bool reportIdDefault = false)
            where TReportId : InputDevice
            where TNoReportId : InputDevice
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || ((UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX) && HIDROGEN_FORCE_REPORT_IDS)
            var TDefault = typeof(TReportId);
#else
            var TDefault = reportIdDefault ? typeof(TReportId) : typeof(TNoReportId);
#endif

            // Register default layout
            InputSystem.RegisterLayout(TDefault, matches:
                GetMatcher(vendorId, productId, (int)UsagePage.GenericDesktop, (int)GenericDesktop.Joystick));
            InputSystem.RegisterLayout(TDefault, matches:
                GetMatcher(vendorId, productId, (int)UsagePage.GenericDesktop, (int)GenericDesktop.Gamepad));

            // Register report ID/no report ID variants
            if (!s_AvailableLayouts.ContainsKey(TDefault.Name))
            {
                InputSystem.RegisterLayout<TReportId>();
                InputSystem.RegisterLayout<TNoReportId>();
                s_AvailableLayouts.Add(TDefault.Name, (typeof(TReportId).Name, typeof(TNoReportId).Name));
            }
        }

        internal static InputDeviceMatcher GetMatcher(int vendorId, int productId,
            int usagePage = (int)UsagePage.GenericDesktop, int usage = (int)GenericDesktop.Gamepad)
        {
            return new InputDeviceMatcher()
                .WithInterface(HidDefinitions.InterfaceName)
                .WithCapability("vendorId", vendorId)
                .WithCapability("productId", productId)
                .WithCapability("usagePage", usagePage)
                .WithCapability("usage", usage);
        }
    }
}
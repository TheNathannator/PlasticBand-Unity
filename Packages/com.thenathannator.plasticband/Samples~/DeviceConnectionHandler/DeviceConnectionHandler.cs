using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

/// <summary>
/// Example for how to properly handle the container devices for VariantDevices.
/// </summary>
public static class DeviceConnectionHandler
{
    public static event Action<InputDevice> DeviceAdded;
    public static event Action<InputDevice> DeviceRemoved;

    private static readonly HashSet<InputDevice> _disabledDevices = new HashSet<InputDevice>();

    public static void Initialize()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private static void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                // Ignore if the device was disabled before being added
                // Necessary for ignoring VariantDevices correctly, without this they
                // will be exposed to the rest of the application for a brief moment.
                if (!device.enabled)
                {
                    _disabledDevices.Add(device);
                    return;
                }

                DeviceAdded?.Invoke(device);
                break;

            case InputDeviceChange.Removed:
                // Ignore if the device was disabled before being removed,
                // as DeviceRemoved will have already been fired for it.
                if (_disabledDevices.Remove(device))
                    return;

                DeviceRemoved?.Invoke(device);
                break;

            // case InputDeviceChange.Reconnected: // Fired alongside Added, not needed
            case InputDeviceChange.Enabled:
                // Ignore if the device was already enabled.
                if (!_disabledDevices.Remove(device))
                    return;

                DeviceAdded?.Invoke(device);
                break;

            // case InputDeviceChange.Disconnected: // Fired alongside Removed, not needed
            case InputDeviceChange.Disabled:
                // Ignore if the device was already disabled.
                if (_disabledDevices.Contains(device))
                    return;

                _disabledDevices.Add(device);
                DeviceRemoved?.Invoke(device);
                break;
        }
    }
}
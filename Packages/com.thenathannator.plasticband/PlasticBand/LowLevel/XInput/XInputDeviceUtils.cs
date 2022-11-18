using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.LowLevel
{
    /// <summary>
    /// Utility functionality for XInput devices.
    /// </summary>
    internal static class XInputDeviceUtils
    {
        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput device of the specified <see cref="XInputController.DeviceSubType"/>.
        /// </summary>
        public static void Register<TDevice>(XInputController.DeviceSubType subType)
            where TDevice : InputDevice
            => Register<TDevice>((int)subType);

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput device of the specified <see cref="XInputNonStandardSubType"/>.
        /// </summary>
        public static void Register<TDevice>(XInputNonStandardSubType subType)
            where TDevice : InputDevice
            => Register<TDevice>((int)subType);

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput device of the specified subtype.
        /// </summary>
        public static void Register<TDevice>(int subType)
            where TDevice : InputDevice
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            InputSystem.RegisterLayout<TDevice>(matches: new InputDeviceMatcher()
                .WithInterface(XInputOther.InterfaceName)
                .WithCapability("subType", subType)
            );
#endif
        }
    }
}
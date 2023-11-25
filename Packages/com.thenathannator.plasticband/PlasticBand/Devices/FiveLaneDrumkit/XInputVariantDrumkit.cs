using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(VariantDeviceDummyState), hideInUI = true)]
    internal class XInputVariantDrumkit : VariantDevice<XInputGamepad>
    {
        internal static void Initialize()
        {
            XInputLayoutFinder.RegisterLayout<XInputVariantDrumkit>(XInputController.DeviceSubType.DrumKit);
        }

        protected override string DetermineLayout(ref XInputGamepad state)
        {
            if (!string.IsNullOrEmpty(m_CurrentLayout))
                return null;

            // 4-lane kits and 5-lane kits share the same subtype, so they need to be differentiated in another way
            // 5-lane kits always hold the left-stick click input, 4-lane kits use that for the second kick but
            // realistically that isn't likely to be held when powering on
            if ((state.buttons & (ushort)XInputButton.LeftThumb) != 0)
                return nameof(XInputFiveLaneDrumkit);
            else
                return nameof(XInputFourLaneDrumkit);
        }
    }
}
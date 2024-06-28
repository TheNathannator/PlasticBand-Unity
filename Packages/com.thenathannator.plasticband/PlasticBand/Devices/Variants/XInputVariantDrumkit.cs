using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(VariantDeviceDummyState), hideInUI = true)]
    internal class XInputVariantDrumkit : VariantSingleDevice
    {
        internal static void Initialize()
        {
            XInputLayoutFinder.RegisterLayout<XInputVariantDrumkit>(XInputController.DeviceSubType.DrumKit);
        }

        protected override unsafe void OnStateEvent(InputEventPtr eventPtr)
        {
            // Create real device on first input event
            // Delayed since there's no reliable way to detect 5-lane kits via capabilities
            if (m_RealDevice == null)
            {
                if (!StateReader.ReadState(eventPtr, out XInputGamepad* state))
                    return;

                // 4-lane kits and 5-lane kits share the same subtype, so they need to be differentiated in another way
                // 5-lane kits always hold the left-stick click input, 4-lane kits use that for the second kick but
                // realistically that isn't likely to be held when powering on
                string layout = (state->buttons & (ushort)XInputButton.LeftThumb) != 0
                    ? nameof(XInputFiveLaneDrumkit)
                    : nameof(XInputFourLaneDrumkit);

                m_RealDevice = new VariantRealDevice(layout);
            }

            m_RealDevice.OnStateEvent(eventPtr);
        }
    }
}
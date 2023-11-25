using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    /// <summary>
    /// An input device which can vary in layout based on state information.
    /// </summary>
    internal abstract class VariantDevice<TState> : InputDevice, IInputStateCallbackReceiver
        where TState : unmanaged, IInputStateTypeInfo
    {
        public static readonly FourCC StateFormat = default(TState).format;

        protected string m_CurrentLayout;
        protected InputDevice m_RealDevice;
        protected IInputStateCallbackReceiver m_RealDeviceCallbacks;

        unsafe void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
        {
            if (eventPtr.type != StateEvent.Type)
                return;

            var stateEvent = StateEvent.From(eventPtr);
            // Ensure the format matches and the buffer is big enough for each state type
            if (stateEvent->stateFormat != StateFormat || stateEvent->stateSizeInBytes < sizeof(TState))
                return;

            // Always check for a new layout so it can change on-the-fly
            ref TState state = ref *(TState*)stateEvent->state;
            string layout = DetermineLayout(ref state);
            if (!string.IsNullOrEmpty(layout))
            {
                m_CurrentLayout = layout;
                if (m_RealDevice != null)
                    InputSystem.RemoveDevice(m_RealDevice);
                m_RealDevice = InputSystem.AddDevice(layout);
                if (m_RealDevice is IInputStateCallbackReceiver callbacks)
                    m_RealDeviceCallbacks = callbacks;
            }

            if (m_RealDevice != null)
            {
                if (m_RealDeviceCallbacks != null)
                    m_RealDeviceCallbacks.OnStateEvent(eventPtr);
                else
                    InputState.Change(m_RealDevice, eventPtr);
            }
        }

        void IInputStateCallbackReceiver.OnNextUpdate() => m_RealDeviceCallbacks?.OnNextUpdate();
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
        {
            if (m_RealDeviceCallbacks != null)
                return m_RealDeviceCallbacks.GetStateOffsetForEvent(control, eventPtr, ref offset);
            else
                return false;
        }

        protected abstract string DetermineLayout(ref TState state);

        protected override void OnAdded()
        {
            base.OnAdded();

            // Disable device but not its events
            // Must be done here and not in FinishSetup, we need to be added to the system first
            InputSystem.DisableDevice(this, keepSendingEvents: true);

            if (m_RealDevice != null)
                InputSystem.AddDevice(m_RealDevice);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            if (m_RealDevice != null)
                InputSystem.RemoveDevice(m_RealDevice);
        }
    }
}
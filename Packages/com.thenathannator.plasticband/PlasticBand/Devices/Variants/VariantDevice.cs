using System;
using System.Collections.Generic;
using PlasticBand.LowLevel;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    // While we can technically just do something like `stateFormat = "BYTE"` for variant device layouts,
    // it ends up tripping an assert since there are no control items present in the layout.
    // So, this will have to do.
    internal struct VariantDeviceDummyState : IInputStateTypeInfo
    {
        public static readonly FourCC Format = new FourCC('V', 'A', 'R', 'I');
        public FourCC format => Format;

        [InputControl(layout = "Integer")]
        public byte dummy;
    }

    internal class VariantRealDevice : IDisposable
    {
        private InputDevice m_Device;
        private IInputStateCallbackReceiver m_DeviceCallbacks;

        public VariantRealDevice(string layout)
        {
            m_Device = InputSystem.AddDevice(layout);
            m_DeviceCallbacks = m_Device as IInputStateCallbackReceiver;
        }

        public VariantRealDevice(InputDeviceDescription description)
        {
            m_Device = InputSystem.AddDevice(description);
            m_DeviceCallbacks = m_Device as IInputStateCallbackReceiver;
        }

        ~VariantRealDevice()
        {
            Logging.Error($"[PlasticBand] VariantRealDevice '{m_Device}' was not disposed! Device cannot be removed outside of the main thread");
        }

        public void Dispose()
        {
            InputSystem.RemoveDevice(m_Device);
            m_Device = null;
            m_DeviceCallbacks = null;

            GC.SuppressFinalize(this);
        }

        public unsafe void OnStateEvent<TState>(ref TState state)
            where TState : unmanaged, IInputStateTypeInfo
        {
            // Create state buffer
            int eventSize = sizeof(TState) + (sizeof(StateEvent) - 1); // StateEvent already includes 1 byte at the end
            byte* _stateEvent = stackalloc byte[eventSize];
            StateEvent* stateEvent = (StateEvent*)_stateEvent;
            *stateEvent = new StateEvent()
            {
                baseEvent = new InputEvent(StateEvent.Type, eventSize, m_Device.deviceId),
                stateFormat = StateCache<TState>.StateFormat
            };
            *(TState*)stateEvent->state = state;

            // Send state event
            OnStateEvent((InputEvent*)stateEvent);
        }

        public unsafe void OnStateEvent(FourCC format, void* stateBuffer, int stateLength)
        {
            // Safety limit, to avoid allocating too much on the stack
            // (InputSystem.StateEventBuffer.kMaxSize)
            const int kMaxStateSize = 512;

            if (stateBuffer == null || stateLength < 1 || stateLength > kMaxStateSize)
                return;

            // Create state buffer
            int eventSize = stateLength + (sizeof(StateEvent) - 1); // StateEvent already includes 1 byte at the end
            byte* _stateEvent = stackalloc byte[eventSize];
            StateEvent* stateEvent = (StateEvent*)_stateEvent;
            *stateEvent = new StateEvent
            {
                baseEvent = new InputEvent(StateEvent.Type, eventSize, m_Device.deviceId),
                stateFormat = format
            };

            // Copy state data
            UnsafeUtility.MemCpy(stateEvent->state, stateBuffer, stateLength);

            // Send state event
            OnStateEvent((InputEvent*)stateEvent);
        }

        public void OnStateEvent(InputEventPtr eventPtr)
        {
            if (m_DeviceCallbacks != null)
                m_DeviceCallbacks.OnStateEvent(eventPtr);
            else
                InputState.Change(m_Device, eventPtr);
        }

        // The input system will update the device on its own
        // public void OnNextUpdate()
        //     => m_DeviceCallbacks?.OnNextUpdate();

        public bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => m_DeviceCallbacks?.GetStateOffsetForEvent(control, eventPtr, ref offset) ?? false;
    }

    /// <summary>
    /// An input device which can vary in layout based on state information.
    /// </summary>
    internal abstract class VariantDevice : InputDevice, IInputStateCallbackReceiver, IDomainReloadReceiver
    {
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => OnStateEvent(eventPtr);
        void IInputStateCallbackReceiver.OnNextUpdate()
            => OnNextUpdate();
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => GetStateOffsetForEvent(control, eventPtr, ref offset);

        protected abstract void OnStateEvent(InputEventPtr eventPtr);
        protected virtual void OnNextUpdate() {}
        protected virtual bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => false;

        protected override void OnAdded()
        {
            base.OnAdded();

            // Disable device but not its events
            // Must be done here and not in FinishSetup, we need to be added to the system first
            InputSystem.DisableDevice(this, keepSendingEvents: true);
        }

        void IDomainReloadReceiver.OnDomainReload()
            => OnDomainReload();

        protected virtual void OnDomainReload() {}
    }

    /// <summary>
    /// A variant device which contains a single device.
    /// </summary>
    internal abstract class VariantSingleDevice : VariantDevice
    {
        protected VariantRealDevice m_RealDevice;

        protected override bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => m_RealDevice?.GetStateOffsetForEvent(control, eventPtr, ref offset) ?? false;

        protected override void OnRemoved()
        {
            base.OnRemoved();
            m_RealDevice?.Dispose();
        }

        protected override void OnDomainReload()
        {
            m_RealDevice?.Dispose();
        }
    }

    /// <summary>
    /// A variant device which contains multiple devices.
    /// </summary>
    internal abstract class VariantMultiDevice : VariantDevice
    {
        protected List<VariantRealDevice> m_RealDevices = new List<VariantRealDevice>();

        protected override void OnRemoved()
        {
            base.OnRemoved();

            foreach (var realDevice in m_RealDevices)
                realDevice?.Dispose();
            m_RealDevices.Clear();
        }

        protected override void OnDomainReload()
        {
            foreach (var realDevice in m_RealDevices)
                realDevice?.Dispose();
            m_RealDevices.Clear();
        }
    }
}
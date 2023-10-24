using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    /// <summary>
    /// Translates a device's state from its original format (<typeparamref name="TFromState"/>)
    /// to another (<typeparamref name="TToState"/>).
    /// </summary>
    internal abstract class StateTranslator<TFromState, TToState>
        where TFromState : unmanaged, IInputStateTypeInfo
        where TToState : unmanaged, IInputStateTypeInfo
    {
        private static FourCC s_FromStateFormat = default(TFromState).format;
        private static FourCC s_ToStateFormat = default(TToState).format;

        public unsafe void VerifyDevice(InputDevice device)
        {
            if (device.stateBlock.format != s_ToStateFormat)
                throw new NotSupportedException($"State format must be {s_ToStateFormat} ({typeof(TToState).Name})!");

            if (device.stateBlock.sizeInBits / 8 < sizeof(TToState))
                throw new NotSupportedException($"State block is too small to accomodate reports translated into {typeof(TToState).Name}!");

            if (sizeof(TFromState) < sizeof(TToState))
                throw new NotSupportedException($"Raw state size is too small for its input events to accomodate reports translated into {typeof(TToState).Name}!");
        }

        protected abstract unsafe TToState TranslateState(ref TFromState fromState);

        public unsafe bool UpdateState(InputDevice device, InputEventPtr eventPtr)
        {
            // Only take state events
            if (eventPtr.type != StateEvent.Type)
                return false;

            var statePtr = StateEvent.From(eventPtr);
            if (statePtr->stateFormat == s_ToStateFormat)
                // This event has already been handled, skip everything else
                return true;

            // Read state data
            if (statePtr->stateSizeInBytes < sizeof(TToState) || !statePtr->TryReadState(out TFromState fromState))
                return false;

            UpdateStateUnchecked(device, statePtr, ref fromState);
            return true;
        }

        public unsafe bool UpdateState(InputDevice device, StateEvent* statePtr, ref TFromState fromState)
        {
            if (statePtr == null)
                return false;

            if (statePtr->stateFormat == s_ToStateFormat)
                // This event has already been handled, skip everything else
                return true;

            UpdateStateUnchecked(device, statePtr, ref fromState);
            return true;
        }

        private unsafe void UpdateStateUnchecked(InputDevice device, StateEvent* statePtr, ref TFromState fromState)
        {
            // Translate state data
            var translated = TranslateState(ref fromState);
            UnsafeUtility.CopyStructureToPtr(ref translated, statePtr->state);

            // Update underlying state buffers
            // We don't need to worry about the final state event buffer being greater than
            // the state block size, as any extra state data is simply ignored
            var eventPtr = statePtr->ToEventPtr();
            InputState.Change(device, eventPtr);
            eventPtr.handled = true;
        }

        // This method is used pretty much only for compatibility with 'InputSystem.onEvent', as otherwise
        // our state transformations aren't applied.
        // Using 'InputState.onChange' for global device state update notifications is recommended instead,
        // as it doesn't circumvent the normal handling of input events, including IInputStateCallbackReceiver.OnStateEvent.
        public bool GetStateOffsetForEvent(InputDevice device, InputControl control, InputEventPtr eventPtr, ref uint offset)
        {
            if (control.device != device || !UpdateState(device, eventPtr))
                return false;

            // No offset is required, all of our controls will be reading
            // relative to the start of the device's state block
            offset = 0;
            return true;
        }

        public unsafe bool GetStateOffsetForEvent(InputDevice device, InputControl control, StateEvent* statePtr,
            ref TFromState fromState, ref uint offset)
        {
            if (control.device != device || !UpdateState(device, statePtr, ref fromState))
                return false;

            // No offset is required, all of our controls will be reading
            // relative to the start of the device's state block
            offset = 0;
            return true;
        }
    }
}
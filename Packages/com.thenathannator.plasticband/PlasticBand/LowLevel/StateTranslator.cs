using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.LowLevel
{
    internal delegate TToState TranslateStateHandler<TFromState, TToState>(ref TFromState fromState)
        where TFromState : unmanaged, IInputStateTypeInfo
        where TToState : unmanaged, IInputStateTypeInfo;

    /// <summary>
    /// Translates a device's state from its original format (<typeparamref name="TFromState"/>)
    /// to another (<typeparamref name="TToState"/>).
    /// </summary>
    internal static unsafe class StateTranslator<TFromState, TToState>
        where TFromState : unmanaged, IInputStateTypeInfo
        where TToState : unmanaged, IInputStateTypeInfo
    {
        public static readonly FourCC FromStateFormat = default(TFromState).format;
        public static readonly FourCC ToStateFormat = default(TToState).format;

        public static void VerifyDevice(InputDevice device)
        {
            if (device.stateBlock.format != ToStateFormat)
                throw new NotSupportedException($"State format must be {ToStateFormat} ({typeof(TToState).Name})!");

            if (device.stateBlock.sizeInBits / 8 < sizeof(TToState))
                throw new NotSupportedException($"State block is too small to accomodate reports translated into {typeof(TToState).Name}!");

            if (sizeof(TFromState) < sizeof(TToState))
                throw new NotSupportedException($"Raw state size is too small for its input events to accomodate reports translated into {typeof(TToState).Name}!");
        }

        public static bool UpdateState(InputDevice device, InputEventPtr eventPtr,
            TranslateStateHandler<TFromState, TToState> translator)
        {
            if (eventPtr.type != StateEvent.Type)
                return false;

            var stateEvent = StateEvent.From(eventPtr);
            // Skip if this event has already been handled
            if (stateEvent->stateFormat == ToStateFormat)
                return true;

            // Ensure the format matches and the buffer is big enough for each state type
            if (stateEvent->stateFormat != FromStateFormat ||
                stateEvent->stateSizeInBytes < sizeof(TFromState) || stateEvent->stateSizeInBytes < sizeof(TToState))
                return false;

            // Read and translate state data
            // Yes, this works! It's exactly what Unsafe.AsRef does under the hood
            ref TFromState fromState = ref *(TFromState*)stateEvent->state;
            var translated = translator(ref fromState);
            UnsafeUtility.CopyStructureToPtr(ref translated, stateEvent->state);
            stateEvent->stateFormat = ToStateFormat;

            // Update underlying state buffers
            // We don't need to worry about the final state event buffer being greater than
            // the state block size, as any extra state data is simply ignored
            InputState.Change(device, eventPtr);
            eventPtr.handled = true;

            return true;
        }

        // This method is used pretty much only for compatibility with 'InputSystem.onEvent', as otherwise
        // our state transformations aren't applied.
        // Using 'InputState.onChange' for global device state update notifications is recommended instead,
        // as it doesn't circumvent the normal handling of input events, including IInputStateCallbackReceiver.OnStateEvent.
        public static bool GetStateOffsetForEvent(InputDevice device, InputControl control, InputEventPtr eventPtr,
            ref uint offset, TranslateStateHandler<TFromState, TToState> translator)
        {
            if ((control != null && control.device != device) || !UpdateState(device, eventPtr, translator))
                return false;

            // No offset is required, all of our controls will be reading
            // relative to the start of the device's state block
            offset = 0;
            return true;
        }
    }
}
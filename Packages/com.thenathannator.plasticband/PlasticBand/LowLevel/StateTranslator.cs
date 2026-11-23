using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.LowLevel
{
    internal unsafe delegate TToState TranslateStateHandler<TToState>(StateEvent* stateEvent)
        where TToState : unmanaged, IInputStateTypeInfo;

    internal delegate TToState TranslateStateHandler<TFromState, TToState>(ref TFromState fromState)
        where TFromState : unmanaged, IInputStateTypeInfo
        where TToState : unmanaged, IInputStateTypeInfo;

    /// <summary>
    /// Translates a device's state events to another format.
    /// </summary>
    internal static unsafe class StateTranslator
    {
        public static void VerifyDevice<TToState>(InputDevice device)
            where TToState : unmanaged, IInputStateTypeInfo
        {
            var destFormat = StateCache<TToState>.StateFormat;

            if (device.stateBlock.format != destFormat)
            {
                throw new NotSupportedException($"Device state format must be {destFormat} ({typeof(TToState).Name}).");
            }

            if (device.stateBlock.sizeInBits / 8 < sizeof(TToState))
            {
                throw new NotSupportedException($"State block is too small to accomodate reports translated into {destFormat} ({typeof(TToState).Name}).");
            }
        }

        public static void VerifyDevice<TFromState, TToState>(InputDevice device)
            where TFromState : unmanaged, IInputStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            VerifyDevice<TToState>(device);

            var destFormat = StateCache<TToState>.StateFormat;

            if (sizeof(TFromState) < sizeof(TToState))
            {
                throw new NotSupportedException($"Input state size is too small to accomodate reports translated into {destFormat} ({typeof(TToState).Name}).");
            }
        }

        public static void OnStateEvent<TToState>(
            InputDevice device,
            InputEventPtr eventPtr,
            TranslateStateHandler<TToState> translator
        )
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (!eventPtr.handled && TranslateState(device, eventPtr, translator))
            {
                // Update underlying state buffers
                // We don't need to worry about the final state event buffer being greater than
                // the state block size, as any extra state data is simply ignored
                //
                // This is also not done in TranslateState, otherwise it screws with the event-based
                // input control extensions since the device's current state buffer will have been
                // overwritten by the data in the event before it can actually be checked against
                InputState.Change(device, eventPtr);
                eventPtr.handled = true;
            }
        }

        public static void OnStateEvent<TFromState, TToState>(
            InputDevice device,
            InputEventPtr eventPtr,
            TranslateStateHandler<TFromState, TToState> translator
        )
            where TFromState : unmanaged, IInputStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (!eventPtr.handled && TranslateState(device, eventPtr, translator))
            {
                InputState.Change(device, eventPtr);
                eventPtr.handled = true;
            }
        }

        private static bool TranslateState<TToState>(
            InputDevice device,
            InputEventPtr eventPtr,
            TranslateStateHandler<TToState> translator
        )
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (eventPtr.type != StateEvent.Type)
            {
                Logging.EditorError($"Non-state event {eventPtr.type} received on translating device {device}!");
                return false;
            }

            var destFormat = StateCache<TToState>.StateFormat;

            var stateEvent = StateEvent.From(eventPtr);
            if (stateEvent->stateFormat == destFormat)
            {
                // Already translated
                return true;
            }

            if (stateEvent->stateSizeInBytes < sizeof(TToState))
            {
                Logging.EditorError($"Event for state format {stateEvent->stateFormat} cannot hold translated format {destFormat}! (Device {device})");
                return false;
            }

            var translated = translator(stateEvent);
            UnsafeUtility.CopyStructureToPtr(ref translated, stateEvent->state);
            stateEvent->stateFormat = destFormat;
            return true;
        }

        private static bool TranslateState<TFromState, TToState>(
            InputDevice device,
            InputEventPtr eventPtr,
            TranslateStateHandler<TFromState, TToState> translator
        )
            where TFromState : unmanaged, IInputStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (eventPtr.type != StateEvent.Type)
            {
                Logging.EditorError($"Non-state event {eventPtr.type} received on translating device {device}!");
                return false;
            }

            var sourceFormat = StateCache<TFromState>.StateFormat;
            var destFormat = StateCache<TToState>.StateFormat;

            var stateEvent = StateEvent.From(eventPtr);
            if (stateEvent->stateFormat == destFormat)
            {
                // Already translated
                return true;
            }

            if (stateEvent->stateFormat != sourceFormat ||
                stateEvent->stateSizeInBytes < sizeof(TFromState) ||
                stateEvent->stateSizeInBytes < sizeof(TToState))
            {
                Logging.EditorError($"Event for state format {stateEvent->stateFormat} cannot hold translated format {destFormat}! (Device {device})");
                return false;
            }

            ref TFromState fromState = ref *(TFromState*)stateEvent->state;
            var translated = translator(ref fromState);
            UnsafeUtility.CopyStructureToPtr(ref translated, stateEvent->state);
            stateEvent->stateFormat = destFormat;
            return true;
        }

        // This method is used pretty much only for compatibility with 'InputSystem.onEvent', as otherwise
        // our state transformations aren't applied.
        // Using 'InputState.onChange' for global device state update notifications is recommended instead,
        // as it doesn't circumvent the normal handling of input events, including IInputStateCallbackReceiver.OnStateEvent.
        public static bool GetStateOffsetForEvent<TToState>(
            InputDevice device,
            InputControl control,
            InputEventPtr eventPtr,
            ref uint offset,
            TranslateStateHandler<TToState> translator
        )
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if ((control == null || control.device == device) &&
                TranslateState(device, eventPtr, translator))
            {
                // No offset is required, all of our controls will be reading
                // relative to the start of the device's state block
                offset = 0;
                return true;
            }

            return false;
        }

        public static bool GetStateOffsetForEvent<TFromState, TToState>(
            InputDevice device,
            InputControl control,
            InputEventPtr eventPtr,
            ref uint offset,
            TranslateStateHandler<TFromState, TToState> translator
        )
            where TFromState : unmanaged, IInputStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if ((control == null || control.device == device) &&
                TranslateState(device, eventPtr, translator))
            {
                offset = 0;
                return true;
            }

            return false;
        }
    }
}

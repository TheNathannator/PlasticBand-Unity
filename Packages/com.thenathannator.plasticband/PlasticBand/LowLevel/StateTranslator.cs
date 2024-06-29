using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
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
        public static readonly FourCC FromStateFormat = StateCache<TFromState>.StateFormat;
        public static readonly FourCC ToStateFormat = StateCache<TToState>.StateFormat;

        public static void VerifyDevice(InputDevice device)
        {
            if (device.stateBlock.format != ToStateFormat)
                throw new NotSupportedException($"Device state format must be {ToStateFormat} ({typeof(TToState).Name})!");

            if (device.stateBlock.sizeInBits / 8 < sizeof(TToState))
                throw new NotSupportedException($"State block is too small to accomodate reports translated into {ToStateFormat} ({typeof(TToState).Name})!");

            if (sizeof(TFromState) < sizeof(TToState))
                throw new NotSupportedException($"Input state size is too small to accomodate reports translated into {ToStateFormat} ({typeof(TToState).Name})!");
        }

        public static void OnStateEvent(InputDevice device, InputEventPtr eventPtr,
            TranslateStateHandler<TFromState, TToState> translator)
        {
            // Skip if event has already been handled
            if (eventPtr.handled || !TranslateState(device, eventPtr, translator))
                return;

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

        private static bool TranslateState(InputDevice device, InputEventPtr eventPtr,
            TranslateStateHandler<TFromState, TToState> translator)
        {
            if (eventPtr.type != StateEvent.Type)
            {
#if UNITY_EDITOR || PLASTICBAND_VERBOSE_LOGGING
                Logging.Error($"Non-state event {eventPtr.type} received on translating device {device}!");
#endif
                return false;
            }

            var stateEvent = StateEvent.From(eventPtr);
            // Skip if this event has already been translated
            if (stateEvent->stateFormat == ToStateFormat)
                return true;

            // Ensure that the state format matches
            if (stateEvent->stateFormat != FromStateFormat)
            {
#if UNITY_EDITOR || PLASTICBAND_VERBOSE_LOGGING
                Logging.Error($"Wrong state format {stateEvent->stateFormat} for translating device {device}! Expected {FromStateFormat}");
#endif
                return false;
            }

            // ...and that the buffer is big enough for each state type
            if (stateEvent->stateSizeInBytes < sizeof(TFromState) || stateEvent->stateSizeInBytes < sizeof(TToState))
            {
#if UNITY_EDITOR || PLASTICBAND_VERBOSE_LOGGING
                Logging.Error($"State size {stateEvent->stateSizeInBytes} for translating device {device} is too small for input size {sizeof(TFromState)} and output size {sizeof(TToState)}!");
#endif
                return false;
            }

            // Read and translate state data
            ref TFromState fromState = ref *(TFromState*)stateEvent->state;
            var translated = translator(ref fromState);

            // Write translated state to input event
            UnsafeUtility.CopyStructureToPtr(ref translated, stateEvent->state);
            stateEvent->stateFormat = ToStateFormat;
            return true;
        }

        // This method is used pretty much only for compatibility with 'InputSystem.onEvent', as otherwise
        // our state transformations aren't applied.
        // Using 'InputState.onChange' for global device state update notifications is recommended instead,
        // as it doesn't circumvent the normal handling of input events, including IInputStateCallbackReceiver.OnStateEvent.
        public static bool GetStateOffsetForEvent(InputDevice device, InputControl control, InputEventPtr eventPtr,
            ref uint offset, TranslateStateHandler<TFromState, TToState> translator)
        {
            if ((control != null && control.device != device) || !TranslateState(device, eventPtr, translator))
                return false;

            // No offset is required, all of our controls will be reading
            // relative to the start of the device's state block
            offset = 0;
            return true;
        }
    }
}
using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.LowLevel
{
    internal interface IStateTranslator : IInputStateCallbackReceiver // for convenience
    {
        InputDevice ToDevice();
    }

    internal interface IStateTranslator<TToState> : IStateTranslator
        where TToState : unmanaged, IInputStateTypeInfo
    {
        unsafe TToState TranslateStateEvent(StateEvent* stateEvent);
    }

    internal interface IStateTranslator<TFromState, TToState> : IStateTranslator
        where TFromState : unmanaged, IInputStateTypeInfo
        where TToState : unmanaged, IInputStateTypeInfo
    {
        TToState TranslateState(ref TFromState fromState);
    }

    /// <summary>
    /// Translates a device's state events to another format.
    /// </summary>
    internal static unsafe class StateTranslator
    {
        public static void VerifyDevice<TToState>(IStateTranslator<TToState> translator)
            where TToState : unmanaged, IInputStateTypeInfo
        {
            var device = translator.ToDevice();
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

        public static void VerifyDevice<TFromState, TToState>(IStateTranslator<TFromState, TToState> translator)
            where TFromState : unmanaged, IInputStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            var device = translator.ToDevice();
            var destFormat = StateCache<TToState>.StateFormat;

            if (device.stateBlock.format != destFormat)
            {
                throw new NotSupportedException($"Device state format must be {destFormat} ({typeof(TToState).Name}).");
            }

            if (device.stateBlock.sizeInBits / 8 < sizeof(TToState))
            {
                throw new NotSupportedException($"State block is too small to accomodate reports translated into {destFormat} ({typeof(TToState).Name}).");
            }

            if (sizeof(TFromState) < sizeof(TToState))
            {
                throw new NotSupportedException($"Input state size is too small to accomodate reports translated into {destFormat} ({typeof(TToState).Name}).");
            }
        }

        public static void OnStateEvent<TToState>(
            IStateTranslator<TToState> translator,
            InputEventPtr eventPtr
        )
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (!eventPtr.handled && TranslateState(translator, eventPtr))
            {
                // Update underlying state buffers
                // We don't need to worry about the final state event buffer being greater than
                // the state block size, as any extra state data is simply ignored
                //
                // This is also not done in TranslateState, otherwise it screws with the event-based
                // input control extensions since the device's current state buffer will have been
                // overwritten by the data in the event before it can actually be checked against
                InputState.Change(translator.ToDevice(), eventPtr);
                eventPtr.handled = true;
            }
        }

        public static void OnStateEvent<TFromState, TToState>(
            IStateTranslator<TFromState, TToState> translator,
            InputEventPtr eventPtr
        )
            where TFromState : unmanaged, IInputStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (!eventPtr.handled && TranslateState(translator, eventPtr))
            {
                InputState.Change(translator.ToDevice(), eventPtr);
                eventPtr.handled = true;
            }
        }

        private static bool TranslateState<TToState>(
            IStateTranslator<TToState> translator,
            InputEventPtr eventPtr
        )
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (eventPtr.type != StateEvent.Type)
            {
                Logging.EditorError($"Non-state event {eventPtr.type} received on translating device {translator.ToDevice()}!");
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
                Logging.EditorError($"State for {translator.ToDevice()} cannot hold translated format {destFormat}!");
                return false;
            }

            var translated = translator.TranslateStateEvent(stateEvent);
            UnsafeUtility.CopyStructureToPtr(ref translated, stateEvent->state);
            stateEvent->stateFormat = destFormat;
            return true;
        }

        private static bool TranslateState<TFromState, TToState>(
            IStateTranslator<TFromState, TToState> translator,
            InputEventPtr eventPtr
        )
            where TFromState : unmanaged, IInputStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (eventPtr.type != StateEvent.Type)
            {
                Logging.EditorError($"Non-state event {eventPtr.type} received on translating device {translator.ToDevice()}!");
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
                Logging.EditorError($"State for {translator.ToDevice()} cannot hold translated format {destFormat}!");
                return false;
            }

            ref TFromState fromState = ref *(TFromState*)stateEvent->state;
            var translated = translator.TranslateState(ref fromState);
            UnsafeUtility.CopyStructureToPtr(ref translated, stateEvent->state);
            stateEvent->stateFormat = destFormat;
            return true;
        }

        // This method is used pretty much only for compatibility with 'InputSystem.onEvent', as otherwise
        // our state transformations aren't applied.
        // Using 'InputState.onChange' for global device state update notifications is recommended instead,
        // as it doesn't circumvent the normal handling of input events, including IInputStateCallbackReceiver.OnStateEvent.
        public static bool GetStateOffsetForEvent<TToState>(
            IStateTranslator<TToState> translator,
            InputControl _control,
            InputEventPtr eventPtr,
            ref uint offset
        )
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (TranslateState(translator, eventPtr))
            {
                // No offset is required, all of our controls will be reading
                // relative to the start of the device's state block
                offset = 0;
                return true;
            }

            return false;
        }

        public static bool GetStateOffsetForEvent<TFromState, TToState>(
            IStateTranslator<TFromState, TToState> translator,
            InputControl _control,
            InputEventPtr eventPtr,
            ref uint offset
        )
            where TFromState : unmanaged, IInputStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (TranslateState(translator, eventPtr))
            {
                offset = 0;
                return true;
            }

            return false;
        }
    }
}

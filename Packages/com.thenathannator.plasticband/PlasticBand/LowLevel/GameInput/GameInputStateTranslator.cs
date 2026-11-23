using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.LowLevel
{
    internal static class GameInputStateTranslator
    {
        public static void VerifyDevice<TFromState, TToState>(InputDevice device)
            where TFromState : unmanaged, IReportIdStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (StateCache<TFromState>.StateFormat != GameInputDefinitions.InputFormat)
            {
                throw new NotSupportedException($"Input state format must be {GameInputDefinitions.InputFormat} ({typeof(TFromState).Name}).");
            }

            StateTranslator.VerifyDevice<TFromState, TToState>(device);
        }

        private static unsafe bool IsValidEvent<TFromState, TToState>(
            InputDevice device,
            InputEventPtr eventPtr
        )
            where TFromState : unmanaged, IReportIdStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (eventPtr.type != StateEvent.Type)
            {
                Logging.EditorError($"Non-state event {eventPtr.type} received on translating device {device}!");
                return false;
            }

            var stateEvent = StateEvent.From(eventPtr);
            if (stateEvent->stateFormat == StateCache<TFromState>.StateFormat)
            {
                return stateEvent->stateSizeInBytes >= sizeof(TFromState) &&
                    *(byte*)stateEvent->state == ReportIdStateCache<TFromState>.StateReportID;
            }
            else if (stateEvent->stateFormat == StateCache<TToState>.StateFormat)
            {
                // No extra checks to be done, this event has already been translated
                return true;
            }

            return false;
        }

        public static void OnStateEvent<TFromState, TToState>(
            InputDevice device,
            InputEventPtr eventPtr,
            TranslateStateHandler<TFromState, TToState> translator
        )
            where TFromState : unmanaged, IReportIdStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (IsValidEvent<TFromState, TToState>(device, eventPtr))
            {
                StateTranslator.OnStateEvent(device, eventPtr, translator);
            }
        }

        public static bool GetStateOffsetForEvent<TFromState, TToState>(
            InputDevice device,
            InputControl control,
            InputEventPtr eventPtr,
            ref uint offset,
            TranslateStateHandler<TFromState, TToState> translator
        )
            where TFromState : unmanaged, IReportIdStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (IsValidEvent<TFromState, TToState>(device, eventPtr))
            {
                return StateTranslator.GetStateOffsetForEvent(device, control, eventPtr, ref offset, translator);
            }

            return false;
        }
    }
}

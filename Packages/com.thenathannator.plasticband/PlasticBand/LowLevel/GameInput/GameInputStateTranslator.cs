using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.LowLevel
{
    internal static class GameInputStateTranslator
    {
        public static void VerifyDevice<TFromState, TToState>(IStateTranslator<TFromState, TToState> translator)
            where TFromState : unmanaged, IReportIdStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (StateCache<TFromState>.StateFormat != GameInputDefinitions.InputFormat)
            {
                throw new NotSupportedException($"Input state format must be {GameInputDefinitions.InputFormat} ({typeof(TFromState).Name}).");
            }

            StateTranslator.VerifyDevice(translator);
        }

        private static unsafe bool IsValidEvent<TFromState, TToState>(
            IStateTranslator<TFromState, TToState> translator,
            InputEventPtr eventPtr
        )
            where TFromState : unmanaged, IReportIdStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (eventPtr.type != StateEvent.Type)
            {
                Logging.EditorError($"Non-state event {eventPtr.type} received on translating device {translator.ToDevice()}!");
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
            IStateTranslator<TFromState, TToState> translator,
            InputEventPtr eventPtr
        )
            where TFromState : unmanaged, IReportIdStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (IsValidEvent(translator, eventPtr))
            {
                StateTranslator.OnStateEvent(translator, eventPtr);
            }
        }

        public static bool GetStateOffsetForEvent<TFromState, TToState>(
            IStateTranslator<TFromState, TToState> translator,
            InputControl control,
            InputEventPtr eventPtr,
            ref uint offset
        )
            where TFromState : unmanaged, IReportIdStateTypeInfo
            where TToState : unmanaged, IInputStateTypeInfo
        {
            if (IsValidEvent(translator, eventPtr))
            {
                return StateTranslator.GetStateOffsetForEvent(translator, control, eventPtr, ref offset);
            }

            return false;
        }
    }
}

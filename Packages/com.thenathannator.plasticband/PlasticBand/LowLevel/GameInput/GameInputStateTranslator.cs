using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.LowLevel
{
    internal static class GameInputStateTranslator<TFromState, TToState>
        where TFromState : unmanaged, IReportIdStateTypeInfo
        where TToState : unmanaged, IInputStateTypeInfo
    {
        public static void VerifyDevice(InputDevice device)
        {
            if (StateCache<TFromState>.StateFormat != GameInputDefinitions.InputFormat)
                throw new NotSupportedException($"Input state format must be {GameInputDefinitions.InputFormat} ({typeof(TFromState).Name})!");

            StateTranslator<TFromState, TToState>.VerifyDevice(device);
        }

        private static unsafe bool IsValidEvent(InputEventPtr eventPtr)
        {
            if (eventPtr.type != StateEvent.Type)
                return false;

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

        public static void OnStateEvent(InputDevice device, InputEventPtr eventPtr,
            TranslateStateHandler<TFromState, TToState> translator)
        {
            if (!IsValidEvent(eventPtr))
                return;

            StateTranslator<TFromState, TToState>.OnStateEvent(device, eventPtr, translator);
        }

        public static bool GetStateOffsetForEvent(InputDevice device, InputControl control, InputEventPtr eventPtr,
            ref uint offset, TranslateStateHandler<TFromState, TToState> translator)
        {
            if (!IsValidEvent(eventPtr))
                return false;

            return StateTranslator<TFromState, TToState>.GetStateOffsetForEvent(
                device, control, eventPtr, ref offset, translator);
        }
    }
}
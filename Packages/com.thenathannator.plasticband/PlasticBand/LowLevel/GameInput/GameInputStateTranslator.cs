using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.LowLevel
{
    internal interface IGameInputStateTypeInfo : IInputStateTypeInfo
    {
        byte reportId { get; }
    }

    internal static class GameInputStateTranslator<TFromState, TToState>
        where TFromState : unmanaged, IGameInputStateTypeInfo
        where TToState : unmanaged, IInputStateTypeInfo
    {
        public static readonly FourCC FromStateFormat = default(TFromState).format;
        public static readonly FourCC ToStateFormat = default(TToState).format;

        public static readonly byte FromStateReportID = default(TFromState).reportId;

        public static void VerifyDevice(InputDevice device)
        {
            if (FromStateFormat != GameInputDefinitions.InputFormat)
                throw new NotSupportedException($"State format must be {GameInputDefinitions.InputFormat} ({typeof(TFromState).Name})!");

            StateTranslator<TFromState, TToState>.VerifyDevice(device);
        }

        private static unsafe bool IsValidEvent(InputEventPtr eventPtr)
        {
            if (eventPtr.type != StateEvent.Type)
                return false;

            var stateEvent = StateEvent.From(eventPtr);
            if (stateEvent->stateFormat != FromStateFormat || stateEvent->stateSizeInBytes < sizeof(TFromState))
                return false;

            return *(byte*)stateEvent->state == FromStateReportID;
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
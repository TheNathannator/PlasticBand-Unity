using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.LowLevel
{
    internal interface IReportIdStateTypeInfo : IInputStateTypeInfo
    {
        byte reportId { get; }
    }

    internal static class StateReader
    {
        public static unsafe bool ReadState<TState>(InputEventPtr eventPtr, out TState* state)
            where TState : unmanaged, IInputStateTypeInfo
        {
            state = null;
            if (eventPtr.type != StateEvent.Type)
                return false;

            var stateEvent = StateEvent.From(eventPtr);
            return ReadState(stateEvent, out state);
        }

        public static unsafe bool ReadState<TState>(StateEvent* stateEvent, out TState* state)
            where TState : unmanaged, IInputStateTypeInfo
        {
            if (stateEvent->stateFormat != StateCache<TState>.StateFormat || stateEvent->stateSizeInBytes < sizeof(TState))
            {
                state = null;
                return false;
            }

            state = (TState*)stateEvent->state;
            return true;
        }

        public static unsafe bool ReadReportIdState<TState>(StateEvent* stateEvent, out TState* state)
            where TState : unmanaged, IReportIdStateTypeInfo
        {
            return ReadState(stateEvent, out state) && *(byte*)state == ReportIdStateCache<TState>.StateReportID;
        }
    }

    internal static class StateCache<TState>
        where TState : unmanaged, IInputStateTypeInfo
    {
        public static readonly FourCC StateFormat = default(TState).format;
    }

    internal static class ReportIdStateCache<TState>
        where TState : unmanaged, IReportIdStateTypeInfo
    {
        public static readonly byte StateReportID = default(TState).reportId;
    }
}
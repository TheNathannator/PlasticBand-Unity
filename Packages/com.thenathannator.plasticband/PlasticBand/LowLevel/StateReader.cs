using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.LowLevel
{
    internal interface IReportIdStateTypeInfo : IInputStateTypeInfo
    {
        byte reportId { get; }
    }

    internal static class StateCache<TState>
        where TState : unmanaged, IInputStateTypeInfo
    {
        public static readonly FourCC StateFormat;
        public static readonly byte? ReportID;

        static StateCache()
        {
            var state = default(TState);
            StateFormat = state.format;

            if (state is IReportIdStateTypeInfo info)
            {
                ReportID = info.reportId;
            }
            else
            {
                ReportID = null;
            }
        }
    }

    internal static class StateReader
    {
        public static unsafe bool ReadState<TState>(InputEventPtr eventPtr, out TState* state)
            where TState : unmanaged, IInputStateTypeInfo
        {
            state = null;
            if (eventPtr.type != StateEvent.Type)
            {
                return false;
            }

            var stateEvent = StateEvent.From(eventPtr);
            return ReadState(stateEvent, out state);
        }

        public static unsafe bool ReadState<TState>(StateEvent* stateEvent, out TState* state)
            where TState : unmanaged, IInputStateTypeInfo
        {
            // Validate format
            if (stateEvent->stateFormat != StateCache<TState>.StateFormat ||
                stateEvent->stateSizeInBytes < sizeof(TState))
            {
                state = null;
                return false;
            }

            // Validate report ID, if the state has one
            if (StateCache<TState>.ReportID != null &&
                *(byte*)stateEvent->state != StateCache<TState>.ReportID)
            {
                state = null;
                return false;
            }

            state = (TState*)stateEvent->state;
            return true;
        }
    }
}

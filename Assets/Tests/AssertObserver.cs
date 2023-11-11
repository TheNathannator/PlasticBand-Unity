using System;
using System.Runtime.ExceptionServices;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests
{
    // This exists to forward exceptions from InputSystem.onUpdate handlers to the code that
    // registered the handler, since InputSystem.onUpdate catches and logs those exceptions
    // to prevent the other handlers from not being run.
    //
    // In our case, we want to forward assert exceptions so that they don't get swallowed
    // up by the input system and correctly propogate to NUnit, otherwise tests likely
    // won't behave as expected.
    public class AssertObserver : IObserver<InputEventPtr>, IDisposable
    {
        private readonly IObservable<InputEventPtr> m_Source;
        private readonly Action<InputEventPtr> m_Action;
        private ExceptionDispatchInfo m_ExceptionInfo = null;

        public AssertObserver(IObservable<InputEventPtr> source, Action<InputEventPtr> action)
        {
            m_Source = source;
            m_Action = action;

            m_Source.Subscribe(this);
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            m_ExceptionInfo = ExceptionDispatchInfo.Capture(error);
        }

        public void OnNext(InputEventPtr value)
        {
            if (m_ExceptionInfo != null)
                return;

            try
            {
                m_Action(value);
            }
            catch (Exception ex)
            {
                m_ExceptionInfo = ExceptionDispatchInfo.Capture(ex);
            }
        }

        public void Dispose()
        {
            m_ExceptionInfo?.Throw();
        }
    }

    public static class ObservableExtensions
    {
        public static IDisposable Assert(this IObservable<InputEventPtr> source, Action<InputEventPtr> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            return new AssertObserver(source, action);
        }
    }
}

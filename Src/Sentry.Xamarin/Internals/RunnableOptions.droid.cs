using System;
using System.Collections.Generic;
using System.Text;
using static IO.Sentry.Sentry;

namespace Sentry.Internals
{
    internal class RunnableOptions<T> : Java.Lang.Object, IOptionsConfiguration where T : Java.Lang.Object
    {
        private readonly Action<T> OnInvoked;

        public RunnableOptions(Action<T> onInvoked)
        {
            OnInvoked = onInvoked;
        }

        public void Configure(Java.Lang.Object p0)
        {
            Console.WriteLine(p0.Class.Name);
            try
            {
                T parameter = (T)p0;
                OnInvoked?.Invoke(parameter);
            }
            catch (Exception ex)
            {
                _ = ex;
                // Exception handling, if needed
            }
        }

        public Java.Lang.Object Invoke(Java.Lang.Object objParameter)
        {
            try
            {
                T parameter = (T)objParameter;
                OnInvoked?.Invoke(parameter);
            }
            catch (Exception ex)
            {
                _ = ex;
                // Exception handling, if needed
            }
            return null;
        }
    }
}

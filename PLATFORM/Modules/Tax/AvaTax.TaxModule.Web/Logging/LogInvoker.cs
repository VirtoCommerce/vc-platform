using System;
using System.Diagnostics.Tracing;

namespace AvaTax.TaxModule.Web.Logging
{
    public class LogInvoker : LogInvoker<BaseLogContext>
    {
    }

    public class LogInvoker<T> where T : BaseLogContext
    {
        public LogInvoker()
        {
            this.Context = Activator.CreateInstance<T>();
        }

        public delegate void LogMethod(T context);
        public delegate void WrappedMethod(T context);
        public T Context { get; set; }

        public static LogInvoker<T> Execute(WrappedMethod codeBody)
        {
            var logInvoker = new LogInvoker<T>();

            try
            {
                logInvoker.Context.Start();
                codeBody.Invoke(logInvoker.Context);
            }
            catch (Exception ex)
            {
                logInvoker.Context.Error = LogExtendedException.Create(ex);
            }
            finally
            {
                logInvoker.Context.Stop();
            }

            return logInvoker;
        }

        public LogInvoker<T> OnSuccess(LogMethod codeBody)
        {
            if (!this.Context.HasError)
            {
                codeBody.Invoke(this.Context);
            }

            return this;
        }

        public LogInvoker<T> OnSuccess(AvalaraLogger eventSource, int eventCode)
        {
            if (!this.Context.HasError)
            {
                eventSource.Write(eventCode, this.Context);
            }
            return this;
        }

        public LogInvoker<T> OnError(LogMethod codeBody)
        {
            if (this.Context.HasError)
            {
                codeBody.Invoke(this.Context);
            }

            return this;
        }

        public LogInvoker<T> OnError(AvalaraLogger eventSource, int eventCode)
        {
            if (this.Context.HasError)
            {
                eventSource.Write(eventCode, this.Context);
            }

            return this;
        }

        public LogInvoker<T> OnFinished(LogMethod codeBody)
        {
            codeBody.Invoke(this.Context);
            return this;
        }

        public LogInvoker<T> OnFinished(AvalaraLogger eventSource, int eventCode)
        {
            eventSource.Write(eventCode, this.Context);
            return this;
        }
    }
}
using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Text;
using System.Threading;

namespace VirtoCommerce.Foundation.Frameworks.Workflow.Services
{
    internal class ActivityInvoker
    {
        private static readonly MethodInfo _preserveStackTraceMethod = GetExceptionPreserveMethod();

        public static IDictionary<String, Object> Invoke(Activity activity, IDictionary<String, Object> inputParameters = null, object[] extensions = null)
        {
            if (inputParameters == null)
            {
                inputParameters = new Dictionary<String, Object>();
            }

            WorkflowApplication application = new WorkflowApplication(activity, inputParameters);
            
            if (extensions != null)
            {
                foreach (var ext in extensions)
                {
                    application.Extensions.Add(ext);
                }
            }

            Exception thrownException = null;
            IDictionary<String, Object> outputParameters = new Dictionary<String, Object>();
            ManualResetEvent waitHandle = new ManualResetEvent(false);

            application.OnUnhandledException = (WorkflowApplicationUnhandledExceptionEventArgs arg) =>
            {
                // Preserve the stack trace in this exception
                // This is a hack into the Exception.InternalPreserveStackTrace method that allows us to re-throw and preserve the call stack
                _preserveStackTraceMethod.Invoke(arg.UnhandledException, null);

                thrownException = arg.UnhandledException;

                return UnhandledExceptionAction.Terminate;
            };
            application.Completed = (WorkflowApplicationCompletedEventArgs obj) =>
            {
                waitHandle.Set();

                outputParameters = obj.Outputs;
            };
            application.Aborted = (WorkflowApplicationAbortedEventArgs obj) => waitHandle.Set();
            application.Idle = (WorkflowApplicationIdleEventArgs obj) => waitHandle.Set();
            application.PersistableIdle = (WorkflowApplicationIdleEventArgs arg) =>
            {
                waitHandle.Set();

                return PersistableIdleAction.Persist;
            };
            application.Unloaded = (WorkflowApplicationEventArgs obj) => waitHandle.Set();

            application.Run();

            waitHandle.WaitOne();

            if (thrownException != null)
            {
                throw thrownException;
            }

            return outputParameters;
        }

        private static MethodInfo GetExceptionPreserveMethod()
        {
            return typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}

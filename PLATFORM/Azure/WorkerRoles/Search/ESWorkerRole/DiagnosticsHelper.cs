using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Azure.WorkerRoles.ElasticSearch
{
    using System.Diagnostics;
    using System.IO;

    using Microsoft.WindowsAzure.ServiceRuntime;

    public static class DiagnosticsHelper
    {

        private static TraceSource __traceSource = CreateTraceSource();

        internal static void TraceVerbose(string message)
        {
            __traceSource.TraceEvent(TraceEventType.Verbose, 0, message);
        }

        internal static void TraceVerbose(
            string message,
            params object[] args)
        {
            __traceSource.TraceEvent(
                TraceEventType.Verbose,
                0,
                message,
                args);
        }

        internal static void TraceInformation(string message)
        {
            __traceSource.TraceEvent(TraceEventType.Information, 0, message);
        }

        internal static void TraceInformation(
            string message,
            params object[] args)
        {
            __traceSource.TraceEvent(
                TraceEventType.Information,
                0,
                message,
                args);
        }

        internal static void TraceWarning(string message)
        {
            __traceSource.TraceEvent(TraceEventType.Warning, 0, message);
        }

        internal static void TraceWarning(
            string message,
            params object[] args)
        {
            __traceSource.TraceEvent(
                TraceEventType.Warning,
                0,
                message,
                args);
        }

        internal static void TraceError(string message)
        {
            __traceSource.TraceEvent(TraceEventType.Error, 0, message);
        }

        internal static void TraceError(
            string message,
            params object[] args)
        {
            __traceSource.TraceEvent(
                TraceEventType.Error,
                0,
                message,
                args);
        }

        internal static void TraceCritical(string message)
        {
            __traceSource.TraceEvent(TraceEventType.Critical, 0, message);
        }

        internal static void TraceCritical(
            string message,
            params object[] args)
        {
            __traceSource.TraceEvent(
                TraceEventType.Critical,
                0,
                message,
                args);
        }

        private static TraceSource CreateTraceSource()
        {
            // Clone Trace but with an expanded name to include the current
            // role instance id.
            string traceSourceName;
            var roleInstanceId = RoleEnvironment.CurrentRoleInstance.Id;
            try
            {
                traceSourceName = Path.GetFileName(
                    Environment.GetCommandLineArgs()[0]) +
                    ": " + roleInstanceId;
            }
            catch (NotSupportedException)
            {
                traceSourceName = "UNSUPPORTED: " + roleInstanceId;
            }

            var traceSource = new TraceSource(traceSourceName, SourceLevels.All);

            // Clear the default TraceSource listeners
            traceSource.Listeners.Clear();

            // Add the registered Trace listeners
            traceSource.Listeners.AddRange(Trace.Listeners);

            return traceSource;
        }

    }
}

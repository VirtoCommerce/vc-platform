using System;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNet.SignalR;

namespace VirtoCommerce.Web.Areas.VirtoAdmin.Helpers
{
    public class SignalRTraceListener : TraceListener
    {
        private StringBuilder _sb = new StringBuilder();

        public string Log
        {
            get
            {
                return _sb.ToString();
            }
        }

        public override void Write(string message)
        {
            _sb.Append(message);
            SetupWorker.TraceMessage(message);
        }

        public override void WriteLine(string message)
        {
            _sb.AppendLine(message);
            SetupWorker.TraceMessageLine(message);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using VirtoCommerce.Web.Areas.VirtoAdmin.Models;

namespace VirtoCommerce.Web.Areas.VirtoAdmin.Helpers
{
    public class SetupWorker : Hub
    {
        public static void SendMessage(string msg, params object[] format)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SetupWorker>();
            context.Clients.All.traceMessage(string.Format(msg,format));
        }

        public static void SendMessageLine(string msg, params object[] format)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SetupWorker>();
            context.Clients.All.traceMessageLine(string.Format(msg, format));
        }

        public static void DisplayErrorMessage(string msg, string selector = ".validation-summary-errors")
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SetupWorker>();
            context.Clients.All.otherMessage(msg, selector);
        }

        public static void DisplayInfoMessage(string msg, string selector = ".message-Information")
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SetupWorker>();
            context.Clients.All.otherMessage(msg, selector);
        }
    }
}
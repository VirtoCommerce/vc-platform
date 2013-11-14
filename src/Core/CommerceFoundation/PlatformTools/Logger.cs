using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Web;
using System.Text;

namespace VirtoCommerce.Foundation.PlatformTools
{
	public static class Logger
	{
		static readonly ILogger InternalLogger;

		static Logger()
		{
			InternalLogger = new VirtoCommerceTraceSource();
		}

		public static Dictionary<string,string> GetTags()
		{
			var @value = new Dictionary<string, string>();
			try
			{
				@value.Add("HttpContextIdentity.Name", HttpContext.Current.User.Identity.Name);
			}
			catch
			{
				@value.Add("HttpContextIdentity.Name", "");
			}
			
			try
			{
				var idenity = System.Security.Principal.WindowsIdentity.GetCurrent();
				@value.Add("WindowsIdentity.Name", idenity != null ? idenity.Name : "");
			}
			catch
			{
				@value.Add("WindowsIdentity.Name", "");
			}

			try
			{
				@value.Add("ManagedThreadId", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture));
			}
			catch
			{
				@value.Add("ManagedThreadId", "");
			}
			
			return @value;
		}

		public static string FromatTags(Dictionary<string,string> tags)
		{
			var sb = new StringBuilder();
			foreach (var tag in tags)
			{
				sb.Append(tag.Key).Append("=").Append(tag.Value).Append("; ");
			}
			return sb.ToString();
		}

		public static void TraceEvent(TraceEventType eventType, string message)
		{
			var tags = FromatTags(GetTags());
			InternalLogger.TraceEvent(eventType, message + Environment.NewLine + tags);
		}

		public static void Error(string message)
		{
			var tags = FromatTags(GetTags());
			InternalLogger.Error(message + Environment.NewLine + tags);
		}

		public static void Info(string message)
		{
			try
			{
				var tags = FromatTags(GetTags());
				InternalLogger.Info(message + Environment.NewLine + tags);
			}
			catch (Exception ex)
			{
				ex.Data["ignored"] = "Logger.Info";
			}
		}

		public static void Warning(string message)
		{
			var tags = FromatTags(GetTags());
			InternalLogger.Warning(message + Environment.NewLine + tags);
		}
	}
}

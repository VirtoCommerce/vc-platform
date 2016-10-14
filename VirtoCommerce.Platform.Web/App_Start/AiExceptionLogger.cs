using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace VirtoCommerce.Platform.Web.App_Start
{
	/// <summary>
	/// To capture unhandled exceptions from Web API 2.x controllers the recommended option is to add an implementation of IExceptionLogger.
	/// </summary>
	public class AiExceptionLogger : ExceptionLogger
	{
		public override void Log(ExceptionLoggerContext context)
		{
			if (context != null && context.Exception != null)
			{
				// Note: A single instance of telemetry client is sufficient to track multiple telemetry items.
				var ai = new TelemetryClient();
				ai.TrackException(context.Exception);
			}
			base.Log(context);
		}
	}
}
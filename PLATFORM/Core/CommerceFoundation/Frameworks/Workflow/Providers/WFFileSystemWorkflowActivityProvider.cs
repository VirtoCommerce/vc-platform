using System.Activities;
using System.Activities.XamlIntegration;
using System.IO;
using System.Xaml;
using System.Configuration.Provider;
using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Caching;

namespace VirtoCommerce.Foundation.Frameworks.Workflow.Providers
{
	public class WFFileSystemWorkflowActivityProvider : WFActivityProvider
	{
		private static readonly Cache _cache;

		object lockObject = new object();

		static WFFileSystemWorkflowActivityProvider()
		{
			HttpContext context = HttpContext.Current;

			if (context != null)
			{
				_cache = context.Cache;
			}
			else
			{
				_cache = HttpRuntime.Cache;
			}
		}

		protected string WorkflowPath;

		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
		{
			if (!string.IsNullOrEmpty(config["WorkflowPath"]))
			{
				WorkflowPath = config["WorkflowPath"];
				config.Remove("WorkflowPath");
			}
			base.Initialize(name, config);
		}

		public override Activity GetWorkflowActivity(string activityName)
		{
			string cacheKey = String.Format("wf-{0}", activityName);

			/*
			object val = Get(cacheKey);

			if (val != null)
				return val as Activity;
			 * */

			var activityPath = GetActivityFilePath(activityName);
			var retVal = ActivityXamlServices.Load(ActivityXamlServices.CreateReader(new XamlXmlReader(activityPath,
																					 new XamlXmlReaderSettings { LocalAssembly = System.Reflection.Assembly.GetExecutingAssembly() })));

			//Insert(cacheKey, retVal, new CacheDependency(activityPath), new TimeSpan(0, 10, 0));

			return retVal;
		}

		protected virtual string GetActivityFilePath(string activityName)
		{
			string prefixPath = WorkflowPath;
			if (prefixPath.StartsWith("~/"))
			{
				if (HttpContext.Current != null)
				{
					prefixPath = HttpContext.Current.Server.MapPath(prefixPath);
				}
			}

			return Path.ChangeExtension(Path.Combine(prefixPath, activityName), "xaml");
		}

		public static void Insert(string key, object value, CacheDependency dependency, TimeSpan timeframe)
		{
			Insert(key, value, dependency, timeframe, CacheItemPriority.Normal);
		}

		public static void Insert(string key, object value, CacheDependency dependency, TimeSpan timeframe, CacheItemPriority priority)
		{
			if (value != null)
			{
				_cache.Insert(key, value, dependency, DateTime.Now.Add(timeframe), Cache.NoSlidingExpiration, priority, null);
			}
		}

		public static object Get(string key)
		{
			return _cache[key];
		}
	}
}

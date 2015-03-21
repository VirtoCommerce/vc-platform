namespace VirtoCommerce.Foundation.Data.Azure.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Microsoft.WindowsAzure;

    public static class AzureCommonHelper
	{
		// HACK: those tokens will probably be provided as constants in the StorageClient library
		public const int MaxEntityTransactionCount = 100;

		/// <summary>
		/// Performs lazy splitting of the provided collection into collections of <paramref name="sliceLength"/>
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="sliceLength">Maximum length of the slice.</param>
		/// <returns>lazy enumerator of the collection of arrays</returns>
		public static IEnumerable<TItem[]> Slice<TItem>(IEnumerable<TItem> source, int sliceLength)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (sliceLength <= 0)
			{
				throw new ArgumentOutOfRangeException("sliceLength", "value must be greater than 0");
			}

			var list = new List<TItem>(sliceLength);
			foreach (var item in source)
			{
				list.Add(item);
				if (sliceLength == list.Count)
				{
					yield return list.ToArray();
					list.Clear();
				}
			}

			if (list.Count > 0)
				yield return list.ToArray();
		}

        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
		public static string GetErrorCode(DataServiceRequestException ex)
		{
			var r = new Regex(@"<code>(\w+)</code>", RegexOptions.IgnoreCase);
			var match = r.Match(ex.InnerException.Message);
			return match.Groups[1].Value;
		}

		/// <summary>
		/// Determines whether we are running in azure environment. If AzureRuntime library is not available returns false or whatever is in the configuration setting "AzureDeployment".
		/// </summary>
		/// <returns>
		///   <c>true</c> if [is azure environment]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsAzureEnvironment()
		{
            /*
            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("RoleRoot")))
            {
                return true;
            }
             * */
			
            // code causes issues with EF6
			if (CheckForAzureEnvironment())
			{
				return true;
			}


            try
            {
                if (CloudConfigurationManager.GetSetting("AzureDeployment") != null)
                {
                    return true;
                }
            }
            catch
            {
                
                return false;
            }

		    return false;
		}

        /// <summary>
        /// Checks for azure environment using reflection.
        /// </summary>
        /// <returns>true of running in azure</returns>
		private static bool CheckForAzureEnvironment()
		{
			var serviceRuntimeAssembly = GetServiceRuntimeAssembly();
			var isAzureEnvironment = false;
			if (serviceRuntimeAssembly != null)
			{
				var type = serviceRuntimeAssembly.GetType("Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment", false);
				var roleEnvironmentExceptionType =
					serviceRuntimeAssembly.GetType("Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironmentException", false);
				if (type != null)
				{

					var property = type.GetProperty("IsAvailable");
					try
					{
						isAzureEnvironment = (property != null) && ((bool)property.GetValue(null, new object[0]));
						Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Loaded \"{0}\"",
													  new object[] { serviceRuntimeAssembly.FullName }));
					}
					catch (TargetInvocationException exception)
					{
						if (!(exception.InnerException is TypeInitializationException))
						{
							throw;
						}
						isAzureEnvironment = false;
					}
				}
			}

			return isAzureEnvironment;
		}

		private static Assembly GetServiceRuntimeAssembly()
		{
			Assembly assembly = null;
			var assemblyName =
				"Microsoft.WindowsAzure.ServiceRuntime, Version=2.4.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL";

			try
			{
				assembly = Assembly.Load(assemblyName);
			}
			catch (Exception exception)
			{
				if ((!(exception is FileNotFoundException) && !(exception is FileLoadException)) &&
					!(exception is BadImageFormatException))
				{
					throw;
				}
			}
			return assembly;
		}
	}
}

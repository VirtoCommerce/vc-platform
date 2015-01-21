using System;
using System.Configuration;
using System.Threading;

namespace VirtoCommerce.Web.Core.Configuration.DynamicContent
{
	public class DynamicContentConfiguration : ConfigurationSection
	{
		private static Lazy<DynamicContentConfiguration> _instance = new Lazy<DynamicContentConfiguration>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

		public static DynamicContentConfiguration Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private static DynamicContentConfiguration CreateInstance()
		{
            var config = (DynamicContentConfiguration)ConfigurationManager.GetSection("VirtoCommerce/DynamicContent");

            if (config == null)
            {
                config = new DynamicContentConfiguration { Connection = new DynamicContentConnection() };
            }

            return config;
		}

		[ConfigurationProperty("Connection", IsRequired = true)]
		public DynamicContentConnection Connection
		{
			get
			{
				return (DynamicContentConnection)this["Connection"];
			}
            private set { this["Connection"] = value; }
		}

        /// <summary>
        /// Config settings which define where caching is enabled and timeouts related to it.
        /// </summary>
        /// <value>The cache.</value>
        [ConfigurationProperty("Cache", IsRequired = true)]
        public CacheConfiguration Cache
        {
            get
            {
                return (CacheConfiguration)this["Cache"];
            }
        }
	} //DynamicContentConfiguration

	public class DynamicContentConnection : ConfigurationElement
	{
		public DynamicContentConnection() { }


		[ConfigurationProperty("dataServiceUri", IsRequired = false)]
		public string DataServiceUri
		{
			get
			{
				return (string)this["dataServiceUri"];
			}
			set
			{
				this["dataServiceUri"] = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only.
		/// </summary>
		/// <returns>
		/// true if the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only; otherwise, false.
		/// </returns>
		public override bool IsReadOnly()
		{
			return false;
		}
	} //DynamicContentConnection

	/// <summary>
	/// Config settings which define where caching is enabled and timeouts related to it.
	/// </summary>
	public class CacheConfiguration : ConfigurationElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CacheConfiguration"/> class.
		/// </summary>
		public CacheConfiguration() { }

		/// <summary>
		/// Attribute determines whether in-memory caching is enabled or not.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is enabled; otherwise, <c>false</c>.
		/// </value>
		[ConfigurationProperty("enabled", IsRequired = true, DefaultValue = true)]
		public bool IsEnabled
		{
			get
			{
				return (bool)this["enabled"];
			}
			set
			{
				this["enabled"] = value;
			}
		}

		[ConfigurationProperty("dynamicContentTimeout", IsRequired = true)]
		public TimeSpan DynamicContentTimeout
		{
			get
			{
				return (TimeSpan)this["dynamicContentTimeout"];
			}
			set
			{
				this["dynamicContentTimeout"] = value.ToString();
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only.
		/// </summary>
		/// <returns>
		/// true if the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only; otherwise, false.
		/// </returns>
		public override bool IsReadOnly()
		{
			return false;
		}
	} //CacheConfiguration

}//namespace

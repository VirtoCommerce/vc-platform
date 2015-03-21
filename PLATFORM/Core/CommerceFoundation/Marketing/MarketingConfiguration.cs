using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;

namespace VirtoCommerce.Foundation.Marketing
{
	public class MarketingConfiguration : ConfigurationSection
	{
		private static Lazy<MarketingConfiguration> _instance = new Lazy<MarketingConfiguration>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

		public static MarketingConfiguration Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private static MarketingConfiguration CreateInstance()
		{
            var config = (MarketingConfiguration)ConfigurationManager.GetSection("VirtoCommerce/Marketing");

            if (config == null)
            {
                config = new MarketingConfiguration { Connection = new MarketingConnection() };
            }

            return config;
		}

		[ConfigurationProperty("Connection", IsRequired = true)]
		public MarketingConnection Connection
		{
			get
			{
				return (MarketingConnection)this["Connection"];
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
	}

	public class MarketingConnection : ConfigurationElement
	{
		public MarketingConnection() { }

		[ConfigurationProperty("wsEndPointName", IsRequired = false)]
		public string WSEndPointName
		{
			get
			{
				return (string)this["wsEndPointName"];
			}
			set
			{
				this["wsEndPointName"] = value;
			}
		}


        [ConfigurationProperty("sqlConnectionStringName", IsRequired = false, DefaultValue = "VirtoCommerce")]
		public string SqlConnectionStringName
		{
			get
			{
				return (string)this["sqlConnectionStringName"];
			}
			set
			{
				this["sqlConnectionStringName"] = value;
			}
		}


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

        [ConfigurationProperty("dataServiceBaseUriName", IsRequired = false)]
        public string DataServiceBaseUriName
        {
            get
            {
                return (string)this["dataServiceBaseUriName"];
            }
            set
            {
                this["dataServiceBaseUriName"] = value;
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
	}

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

        [ConfigurationProperty("promotionsTimeout", IsRequired = true)]
        public TimeSpan PromotionsTimeout
        {
            get
            {
                return (TimeSpan)this["promotionsTimeout"];
            }
            set
            {
                this["promotionsTimeout"] = value.ToString();
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
    }
}

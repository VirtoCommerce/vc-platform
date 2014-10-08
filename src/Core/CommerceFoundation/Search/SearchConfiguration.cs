using System;
using System.Configuration;
using System.Threading;

namespace VirtoCommerce.Foundation.Search
{
    /// <summary>
    /// Implemented as a thread-safe singleton class
    /// </summary>
    public class SearchConfiguration : ConfigurationSection
    {
		private static readonly Lazy<SearchConfiguration> _instance = new Lazy<SearchConfiguration>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

		public static SearchConfiguration Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private static SearchConfiguration CreateInstance()
		{
			var config = (SearchConfiguration)ConfigurationManager.GetSection("VirtoCommerce/Search") ??
			             new SearchConfiguration();

		    return config;
		}

        [ConfigurationProperty("Connection", IsRequired = true)]
        public SearchConnectionConfiguration Connection
        {
            get
            {
                return (SearchConnectionConfiguration)this["Connection"];
            }
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

    public class SearchConnectionConfiguration : ConfigurationElement
    {
        public SearchConnectionConfiguration() { }

        [ConfigurationProperty("sqlConnectionStringName", IsRequired = false)]
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

        /// <summary>
        /// Gets or sets the filters timeout.
        /// </summary>
        /// <value>
        /// The filters timeout.
        /// </value>
        [ConfigurationProperty("filtersTimeout", IsRequired = true)]
        public TimeSpan FiltersTimeout
        {
            get
            {
                return (TimeSpan)this["filtersTimeout"];
            }
            set
            {
                this["filtersTimeout"] = value.ToString();
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
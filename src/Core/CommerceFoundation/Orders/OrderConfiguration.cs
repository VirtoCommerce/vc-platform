using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;

namespace VirtoCommerce.Foundation.Orders
{
    public sealed class OrderConfiguration : ConfigurationSection
    {
		private static Lazy<OrderConfiguration> _instance = new Lazy<OrderConfiguration>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

		public static OrderConfiguration Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private static OrderConfiguration CreateInstance()
		{
			return (OrderConfiguration)ConfigurationManager.GetSection("VirtoCommerce/Order");
		}

	

        [ConfigurationProperty("Connection", IsRequired = true)]
        public OrderConnection Connection
        {
            get
            {
                return (OrderConnection)this["Connection"];
            }
        }

        [ConfigurationProperty("OrderServiceConnection", IsRequired = true)]
        public OrderServiceConnection OrderServiceConnection
        {
            get { return (OrderServiceConnection) this["OrderServiceConnection"]; }
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

    public class OrderConnection : ConfigurationElement
    {
        public OrderConnection() { }

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

        [ConfigurationProperty("ordersTimeout", IsRequired = false, DefaultValue="0:0:30")]
        public TimeSpan OrdersTimeout
        {
            get
            {
                return (TimeSpan)this["ordersTimeout"];
            }
            set
            {
                this["ordersTimeout"] = value.ToString();
            }
        }

        [ConfigurationProperty("cartTimeout", IsRequired = false, DefaultValue = "0:0:30")]
        public TimeSpan CartTimeout
        {
            get
            {
                return (TimeSpan)this["cartTimeout"];
            }
            set
            {
                this["cartTimeout"] = value.ToString();
            }
        }

        [ConfigurationProperty("shippingTimeout", IsRequired = false, DefaultValue = "0:0:30")]
        public TimeSpan ShippingTimeout
        {
            get
            {
                return (TimeSpan)this["shippingTimeout"];
            }
            set
            {
                this["shippingTimeout"] = value.ToString();
            }
        }

        [ConfigurationProperty("countryTimeout", IsRequired = false, DefaultValue = "0:0:30")]
        public TimeSpan CountryTimeout
        {
            get
            {
                return (TimeSpan)this["countryTimeout"];
            }
            set
            {
                this["countryTimeout"] = value.ToString();
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

    public class OrderServiceConnection:ConfigurationElement
    {
        public OrderServiceConnection() { }

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

		[ConfigurationProperty("serviceBaseUriName", IsRequired = false)]
		public string ServiceBaseUriName
		{
			get
			{
				return (string)this["serviceBaseUriName"];
			}
			set
			{
				this["serviceBaseUriName"] = value;
			}
		}

		[ConfigurationProperty("serviceUri", IsRequired = false)]
		public string ServiceUri
		{
			get
			{
				return (string)this["serviceUri"];
			}
			set
			{
				this["serviceUri"] = value;
			}
		}

		[ConfigurationProperty("forceHttps", IsRequired = false)]
		public bool ForceHttps
		{
			get
			{
				return (bool)this["forceHttps"];
			}
			set
			{
				this["forceHttps"] = value;
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

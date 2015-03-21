using System;
using System.Configuration;
using System.Threading;
using VirtoCommerce.Foundation.Inventories;
using VirtoCommerce.Foundation.Orders;

namespace VirtoCommerce.Foundation.Reporting
{
    public class ReportingConfiguration : ConfigurationSection
    {
        private static Lazy<ReportingConfiguration> _instance = new Lazy<ReportingConfiguration>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

        public static ReportingConfiguration Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private static ReportingConfiguration CreateInstance()
        {
            var aaa = ConfigurationManager.GetSection("VirtoCommerce/Reporting");
            return (ReportingConfiguration)ConfigurationManager.GetSection("VirtoCommerce/Reporting");
        }

        [ConfigurationProperty("Connection", IsRequired = true)]
        public ReportingConnection Connection
        {
            get
            {
                return (ReportingConnection)this["Connection"];
            }
        }

        [ConfigurationProperty("ReportingServiceConnection", IsRequired = true)]
        public ReportingServiceConnection ReportingService
        {
            get
            {
                return (ReportingServiceConnection)this["ReportingServiceConnection"];
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

    public class ReportingConnection : ConfigurationElement
    {
        public ReportingConnection()
        {
        }

        [ConfigurationProperty("wsEndPointName", IsRequired = false)]
        public string WSEndPointName
        {
            get { return (string) this["wsEndPointName"]; }
            set { this["wsEndPointName"] = value; }
        }

        [ConfigurationProperty("sqlConnectionStringName", IsRequired = false)]
        public string SqlConnectionStringName
        {
            get { return (string) this["sqlConnectionStringName"]; }
            set { this["sqlConnectionStringName"] = value; }
        }

        [ConfigurationProperty("dataServiceUri", IsRequired = false)]
        public string DataServiceUri
        {
            get { return (string) this["dataServiceUri"]; }
            set { this["dataServiceUri"] = value; }
        }

        [ConfigurationProperty("dataServiceBaseUriName", IsRequired = false)]
        public string DataServiceBaseUriName
        {
            get { return (string) this["dataServiceBaseUriName"]; }
            set { this["dataServiceBaseUriName"] = value; }
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

    public class ReportingServiceConnection : ConfigurationElement
    {
        public ReportingServiceConnection() { }

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
    }

    public class CacheConfiguration : ConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Orders.CacheConfiguration"/> class.
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

        [ConfigurationProperty("ordersTimeout", IsRequired = false, DefaultValue = "0:0:30")]
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
}

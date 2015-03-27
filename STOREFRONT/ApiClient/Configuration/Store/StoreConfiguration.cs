#region

using System;
using System.Configuration;
using System.Threading;

#endregion

namespace VirtoCommerce.ApiClient.Configuration.Store
{
    public class StoreConfiguration : ConfigurationSection
    {
        #region Static Fields

        private static readonly Lazy<StoreConfiguration> _instance = new Lazy<StoreConfiguration>(
            CreateInstance,
            LazyThreadSafetyMode.ExecutionAndPublication);

        #endregion

        #region Public Properties

        public static StoreConfiguration Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>
        ///     Config settings which define where caching is enabled and timeouts related to it.
        /// </summary>
        /// <value>The cache.</value>
        [ConfigurationProperty("Cache", IsRequired = true)]
        public CacheConfiguration Cache
        {
            get { return (CacheConfiguration)this["Cache"]; }
        }

        [ConfigurationProperty("Connection", IsRequired = true)]
        public StoreConnection Connection
        {
            get { return (StoreConnection)this["Connection"]; }
        }

        #endregion

        #region Methods

        private static StoreConfiguration CreateInstance()
        {
            return (StoreConfiguration)ConfigurationManager.GetSection("VirtoCommerce/Store");
        }

        #endregion
    }

    public class StoreConnection : ConfigurationElement
    {
        #region Constructors and Destructors

        public StoreConnection()
        {
        }

        #endregion

        #region Public Properties

        [ConfigurationProperty("dataServiceUri", IsRequired = false)]
        public string DataServiceUri
        {
            get { return (string)this["dataServiceUri"]; }
            set { this["dataServiceUri"] = value; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Configuration.ConfigurationElement" /> object is read-only.
        /// </summary>
        /// <returns>
        ///     true if the <see cref="T:System.Configuration.ConfigurationElement" /> object is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly()
        {
            return false;
        }

        #endregion
    }

    /// <summary>
    ///     Config settings which define where caching is enabled and timeouts related to it.
    /// </summary>
    public class CacheConfiguration : ConfigurationElement
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CacheConfiguration" /> class.
        /// </summary>
        public CacheConfiguration()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Attribute determines whether in-memory caching is enabled or not.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("enabled", IsRequired = true, DefaultValue = true)]
        public bool IsEnabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        /// <summary>
        ///     Configuration attribute which determines when the Catalog values are
        ///     automatically refreshed in memory (in seconds).
        /// </summary>
        /// <value>
        ///     The catalog collection timeout.
        /// </value>
        [ConfigurationProperty("storeTimeout", IsRequired = true)]
        public TimeSpan StoreTimeout
        {
            get { return (TimeSpan)this["storeTimeout"]; }
            set { this["storeTimeout"] = value.ToString(); }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Configuration.ConfigurationElement" /> object is read-only.
        /// </summary>
        /// <returns>
        ///     true if the <see cref="T:System.Configuration.ConfigurationElement" /> object is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly()
        {
            return false;
        }

        #endregion
    }
}

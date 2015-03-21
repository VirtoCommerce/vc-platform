#region

using System;
using System.Configuration;
using System.Threading;

#endregion

namespace VirtoCommerce.ApiClient.Configuration.Catalog
{
    public sealed class CatalogConfiguration : ConfigurationSection
    {
        #region Static Fields

        private static readonly Lazy<CatalogConfiguration> _instance = new Lazy<CatalogConfiguration>(
            CreateInstance,
            LazyThreadSafetyMode.ExecutionAndPublication);

        #endregion

        #region Public Properties

        public static CatalogConfiguration Instance
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
        public CatalogConnection Connection
        {
            get { return (CatalogConnection)this["Connection"]; }
            private set { this["Connection"] = value; }
        }

        #endregion

        #region Methods

        private static CatalogConfiguration CreateInstance()
        {
            var config = (CatalogConfiguration)ConfigurationManager.GetSection("VirtoCommerce/Catalog");

            if (config == null)
            {
                config = new CatalogConfiguration { Connection = new CatalogConnection() };
            }

            return config;
        }

        #endregion
    }

    public class CatalogConnection : ConfigurationElement
    {
        #region Constructors and Destructors

        public CatalogConnection()
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
        ///     Configuration attribute which determines when the CategoryCollections values are
        ///     automatically refreshed in memory (in seconds).
        /// </summary>
        /// <value>The catalog collection timeout.</value>
        [ConfigurationProperty("categoryCollectionTimeout", IsRequired = false, DefaultValue = "00:00:30")]
        public TimeSpan CategoryCollectionTimeout
        {
            get { return (TimeSpan)this["categoryCollectionTimeout"]; }
            set { this["categoryCollectionTimeout"] = value.ToString(); }
        }

        /// <summary>
        ///     Configuration attribute which determines when the Category value is
        ///     automatically refreshed in memory (in seconds).
        /// </summary>
        /// <value>The catalog node timeout.</value>
        [ConfigurationProperty("categoryTimeout", IsRequired = false, DefaultValue = "00:00:30")]
        public TimeSpan CategoryTimeout
        {
            get { return (TimeSpan)this["categoryTimeout"]; }
            set { this["categoryTimeout"] = value.ToString(); }
        }

        /// <summary>
        ///     Attribute determines whether in-memory caching is enabled or not.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool IsEnabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        /// <summary>
        ///     Configuration attribute which determines when the ItemCollections values are
        ///     automatically refreshed in memory (in seconds).
        /// </summary>
        /// <value>The catalog collection timeout.</value>
        [ConfigurationProperty("itemCollectionTimeout", IsRequired = false, DefaultValue = "00:00:30")]
        public TimeSpan ItemCollectionTimeout
        {
            get { return (TimeSpan)this["itemCollectionTimeout"]; }
            set { this["itemCollectionTimeout"] = value.ToString(); }
        }

        /// <summary>
        ///     Configuration attribute which determines when the Item value is
        ///     automatically refreshed in memory (in seconds).
        /// </summary>
        /// <value>The catalog entry timeout.</value>
        [ConfigurationProperty("itemTimeout", IsRequired = false, DefaultValue = "00:00:30")]
        public TimeSpan ItemTimeout
        {
            get { return (TimeSpan)this["itemTimeout"]; }
            set { this["itemTimeout"] = value.ToString(); }
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

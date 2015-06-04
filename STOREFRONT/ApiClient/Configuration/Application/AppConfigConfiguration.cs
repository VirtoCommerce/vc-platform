#region

using System;
using System.Configuration;
using System.Threading;
using System.Web.Configuration;

#endregion

namespace VirtoCommerce.ApiClient.Configuration.Application
{
    public class AppConfigConfiguration : ConfigurationSection
    {
        #region Constants

        public const string SectionName = "VirtoCommerce/AppConfig";

        #endregion

        #region Static Fields

        private static readonly Lazy<AppConfigConfiguration> _instance = new Lazy<AppConfigConfiguration>(
            CreateInstance,
            LazyThreadSafetyMode.ExecutionAndPublication);

        #endregion

        #region Public Properties

        public static AppConfigConfiguration Instance
        {
            get { return _instance.Value; }
        }

        [ConfigurationProperty("availableModules")]
        public ModulesCollection AvailableModules
        {
            get { return (ModulesCollection)this["availableModules"] ?? new ModulesCollection(); }
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
        public AppConfigConnection Connection
        {
            get { return (AppConfigConnection)this["Connection"]; }
        }

        [ConfigurationProperty("Setup", IsRequired = true)]
        public SetupConfiguration Setup
        {
            get { return (SetupConfiguration)this["Setup"]; }
        }

        #endregion

        #region Methods

        private static AppConfigConfiguration CreateInstance()
        {
            return (AppConfigConfiguration)ConfigurationManager.GetSection(SectionName);
        }

        #endregion
    }

    public class AppConfigConnection : ConfigurationElement
    {
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
        ///     Initializes a new instance of the <see cref="Commerce.ApiWebClient.Configuration.CacheConfiguration" /> class.
        /// </summary>
        public CacheConfiguration()
        {
        }

        #endregion

        #region Public Properties

        [ConfigurationProperty("displayTemplatesTimeout", IsRequired = false, DefaultValue = "0:2:0")]
        public TimeSpan DisplayTemplateMappingsTimeout
        {
            get { return (TimeSpan)this["displayTemplatesTimeout"]; }
            set { this["displayTemplatesTimeout"] = value.ToString(); }
        }

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
        ///     Configuration attribute which determines when the Localization values are
        ///     automatically refreshed in memory (in seconds).
        /// </summary>
        /// <value>
        ///     The localization collection timeout.
        /// </value>
        [ConfigurationProperty("localizationTimeout", IsRequired = true, DefaultValue = "0:1:0")]
        public TimeSpan LocalizationTimeout
        {
            get { return (TimeSpan)this["localizationTimeout"]; }
            set { this["localizationTimeout"] = value.ToString(); }
        }

        /// <summary>
        ///     Configuration attribute which determines when the seo keywords values are
        ///     automatically refreshed in memory (in seconds).
        /// </summary>
        /// <value>
        ///     The seo keywords timeout.
        /// </value>
        [ConfigurationProperty("seoKeywordsTimeout", IsRequired = false, DefaultValue = "0:1:0")]
        public TimeSpan SeoKeywordsTimeout
        {
            get { return (TimeSpan)this["seoKeywordsTimeout"]; }
            set { this["seoKeywordsTimeout"] = value.ToString(); }
        }

        /// <summary>
        ///     Configuration attribute which determines when the Settings values are
        ///     automatically refreshed in memory (in seconds).
        /// </summary>
        /// <value>
        ///     The settings collection timeout.
        /// </value>
        [ConfigurationProperty("settingsTimeout", IsRequired = true, DefaultValue = "0:1:0")]
        public TimeSpan SettingsTimeout
        {
            get { return (TimeSpan)this["settingsTimeout"]; }
            set { this["settingsTimeout"] = value.ToString(); }
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

    public class SetupConfiguration : ConfigurationElement
    {
        #region Public Properties

        [ConfigurationProperty("adminUrl", IsRequired = false, DefaultValue = "")]
        public string AdminUrl
        {
            get { return this["adminUrl"].ToString(); }
            set
            {
                this["adminUrl"] = value;
                var configFile = WebConfigurationManager.OpenWebConfiguration("~");
                var section = (AppConfigConfiguration)configFile.GetSection(AppConfigConfiguration.SectionName);
                section.Setup["adminUrl"] = value;
                configFile.Save(ConfigurationSaveMode.Modified);
            }
        }

        [ConfigurationProperty("completed", IsRequired = true, DefaultValue = true)]
        public bool IsCompleted
        {
            get { return (bool)this["completed"]; }
            set
            {
                this["completed"] = value.ToString();
                var configFile = WebConfigurationManager.OpenWebConfiguration("~");
                var section = (AppConfigConfiguration)configFile.GetSection(AppConfigConfiguration.SectionName);
                section.Setup["completed"] = value;
                configFile.Save(ConfigurationSaveMode.Modified);
            }
        }

        #endregion

        #region Public Methods and Operators

        public override bool IsReadOnly()
        {
            return false;
        }

        #endregion
    }

    public class ModuleConfigurationElement : ConfigurationElement
    {
        #region Public Properties

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        #endregion
    }

    public class ModulesCollection : ConfigurationElementCollection
    {
        #region Public Indexers

        public ModuleConfigurationElement this[int index]
        {
            get { return base.BaseGet(index) as ModuleConfigurationElement; }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        #endregion

        #region Methods

        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleConfigurationElement)element).Name;
        }

        #endregion
    }
}

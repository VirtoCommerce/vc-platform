using System;
using System.Configuration;
using System.Threading;
using System.Web.Configuration;

namespace VirtoCommerce.Foundation.AppConfig
{
	public class AppConfigConfiguration : ConfigurationSection
	{
		private static Lazy<AppConfigConfiguration> _instance = new Lazy<AppConfigConfiguration>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

        public const string SectionName = "VirtoCommerce/AppConfig";

		public static AppConfigConfiguration Instance
		{
			get
			{
				return _instance.Value;
			}
		}

	    private static AppConfigConfiguration CreateInstance()
		{

            return (AppConfigConfiguration)ConfigurationManager.GetSection(SectionName);
		}

		[ConfigurationProperty("Connection", IsRequired = true)]
		public AppConfigConnection Connection
		{
			get
			{
				return (AppConfigConnection)this["Connection"];
			}
		}

		[ConfigurationProperty("Scheduler", IsRequired = false)]
		public SchedulerConnection Scheduler
		{
			get
			{
				return (SchedulerConnection)this["Scheduler"];
			}
		}

        [ConfigurationProperty("Setup", IsRequired = true)]
        public SetupConfiguration Setup
	    {
	        get
	        {
                return (SetupConfiguration)this["Setup"];
	        }
	    }

        [ConfigurationProperty("availableModules")]
        public ModulesCollection AvailableModules
        {
            get
            {
                return (ModulesCollection)this["availableModules"] ?? new ModulesCollection();
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

	public class AppConfigConnection : ConfigurationElement
	{
		public AppConfigConnection() { }

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

	public class SchedulerConnection : ConfigurationElement
	{
		public SchedulerConnection() { }

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
		/// Configuration attribute which determines when the Settings values are
		/// automatically refreshed in memory (in seconds).
		/// </summary>
		/// <value>
		/// The settings collection timeout.
		/// </value>
        [ConfigurationProperty("settingsTimeout", IsRequired = true, DefaultValue = "0:1:0")]
		public TimeSpan SettingsTimeout
		{
			get
			{
				return (TimeSpan)this["settingsTimeout"];
			}
			set
			{
				this["settingsTimeout"] = value.ToString();
			}
		}

		/// <summary>
		/// Configuration attribute which determines when the Localization values are
		/// automatically refreshed in memory (in seconds).
		/// </summary>
		/// <value>
		/// The localization collection timeout.
		/// </value>
		[ConfigurationProperty("localizationTimeout", IsRequired = true, DefaultValue = "0:1:0")]
		public TimeSpan LocalizationTimeout
		{
			get
			{
				return (TimeSpan)this["localizationTimeout"];
			}
			set
			{
				this["localizationTimeout"] = value.ToString();
			}
		}

		/// <summary>
		/// Configuration attribute which determines when the Scheduled jobs are
		/// automatically refreshed in memory (in seconds).
		/// </summary>
		/// <value>
		/// The scheduled jobs collection timeout.
		/// </value>
        [ConfigurationProperty("schedulerTimeout", IsRequired = false, DefaultValue = "0:1:0")]
		public TimeSpan SchedulerTimeout
		{
			get
			{
				return (TimeSpan)this["schedulerTimeout"];
			}
			set
			{
				this["schedulerTimeout"] = value.ToString();
			}
		}


        [ConfigurationProperty("displayTemplatesTimeout", IsRequired = false, DefaultValue = "0:2:0")]
		public TimeSpan DisplayTemplateMappingsTimeout
		{
			get
			{
				return (TimeSpan)this["displayTemplatesTimeout"];
			}
			set
			{
				this["displayTemplatesTimeout"] = value.ToString();
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

    public class SetupConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("completed", IsRequired = true, DefaultValue = true)]
        public bool IsCompleted
        {
            get
            {
                return (bool)this["completed"];
            }
            set
            {
                this["completed"] = value.ToString();
                var configFile = WebConfigurationManager.OpenWebConfiguration("~");
                var section = (AppConfigConfiguration)configFile.GetSection(AppConfigConfiguration.SectionName);
                section.Setup["completed"] = value;
                configFile.Save(ConfigurationSaveMode.Modified);
            }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class ModuleConfigurationElement : ConfigurationElement
    {

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)base["type"];
            }
            set
            {
                base["type"] = value;
            }
        }


    }

    public class ModulesCollection : ConfigurationElementCollection
    {
        public ModuleConfigurationElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as ModuleConfigurationElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleConfigurationElement)element).Name;
        }
    }
}

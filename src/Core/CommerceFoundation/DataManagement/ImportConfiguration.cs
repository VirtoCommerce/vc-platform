using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;

namespace VirtoCommerce.Foundation.Importing
{
	public sealed class ImportConfiguration : ConfigurationSection
	{
		private static Lazy<ImportConfiguration> _instance = new Lazy<ImportConfiguration>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

		public static ImportConfiguration Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private static ImportConfiguration CreateInstance()
		{
			return (ImportConfiguration)ConfigurationManager.GetSection("VirtoCommerce/Import");
		}

		[ConfigurationProperty("Connection", IsRequired = true)]
		public ImportConnection Connection
		{
			get
			{
				return (ImportConnection)this["Connection"];
			}
		}

		[ConfigurationProperty("Importing", IsRequired = true)]
		public ImportingConnection ImportingService
		{
			get
			{
				return (ImportingConnection)this["Importing"];
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

	public class ImportConnection : ConfigurationElement
	{
		public ImportConnection() { }

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

	public class ImportingConnection : ConfigurationElement
	{
		public ImportingConnection() { }

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

		///<summary>
		/// Configuration attribute which determines when the ImportJob values are
		/// automatically refreshed in memory (in seconds).
		/// </summary>
		/// <value>The import job timeout.</value>
		[ConfigurationProperty("importJobTimeout", IsRequired = true)]
		public TimeSpan ImportJobTimeout
		{
			get
			{
				return (TimeSpan)this["importJobTimeout"];
			}
			set
			{
				this["importJobTimeout"] = value.ToString();
			}
		}

		///<summary>
		/// Configuration attribute which determines when the ColumnMapping values are
		/// automatically refreshed in memory (in seconds).
		/// </summary>
		/// <value>The column mapping timeout.</value>
		[ConfigurationProperty("columnMappingTimeout", IsRequired = true)]
		public TimeSpan ColumnMappingTimeoutTimeout
		{
			get
			{
				return (TimeSpan)this["columnMappingTimeout"];
			}
			set
			{
				this["columnMappingTimeout"] = value.ToString();
			}
		}

		/// <summary>
		/// Configuration attribute which determines when the MappingItem value is
		/// automatically refreshed in memory (in seconds).
		/// </summary>
		/// <value>The mapping item timeout.</value>
		[ConfigurationProperty("mappingItemTimeout", IsRequired = true)]
		public TimeSpan MappingItemTimeout
		{
			get
			{
				return (TimeSpan)this["mappingItemTimeout"];
			}
			set
			{
				this["mappingItemTimeout"] = value.ToString();
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

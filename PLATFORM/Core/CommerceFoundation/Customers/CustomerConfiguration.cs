using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;

namespace VirtoCommerce.Foundation.Customers
{
	public class CustomerConfiguration : ConfigurationSection
	{
		private static Lazy<CustomerConfiguration> _instance = new Lazy<CustomerConfiguration>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

		public static CustomerConfiguration Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private static CustomerConfiguration CreateInstance()
		{
			return (CustomerConfiguration)ConfigurationManager.GetSection("VirtoCommerce/Customer");
		}


		[ConfigurationProperty("Connection", IsRequired = true)]
		public CustomerServiceConnection Connection
		{
			get
			{
				return (CustomerServiceConnection)this["Connection"];
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


	public class CustomerServiceConnection : ConfigurationElement
	{
		public CustomerServiceConnection() { }

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
		/// Configuration attribute which determines when the Customer values are
		/// automatically refreshed in memory (in seconds).
		/// </summary>
		/// <value>
		/// The catalog collection timeout.
		/// </value>
		[ConfigurationProperty("customerTimeout", IsRequired = true)]
		public TimeSpan CustomerTimeout
		{
			get
			{
				return (TimeSpan)this["customerTimeout"];
			}
			set
			{
                this["customerTimeout"] = value.ToString();
			}
		}

		[ConfigurationProperty("rulesTimeout", IsRequired = true)]
		public TimeSpan CaseRuleTimeout
		{
			get
			{
				return (TimeSpan)this["rulesTimeout"];
			}
			set
			{
				this["rulesTimeout"] = value.ToString();
			}
		}

		[ConfigurationProperty("organizationTimeout", IsRequired = true)]
		public TimeSpan OrganizationTimeout
		{
			get
			{
				return (TimeSpan)this["organizationTimeout"];
			}
			set
			{
				this["organizationTimeout"] = value.ToString();
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

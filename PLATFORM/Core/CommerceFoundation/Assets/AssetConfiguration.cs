using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;

namespace VirtoCommerce.Foundation.Assets
{
	public class AssetConfiguration : ConfigurationSection
	{
		private static Lazy<AssetConfiguration> _instance = new Lazy<AssetConfiguration>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

		public static AssetConfiguration Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private static AssetConfiguration CreateInstance()
		{
			return (AssetConfiguration)ConfigurationManager.GetSection("VirtoCommerce/Asset");
		}
		
        [ConfigurationProperty("Connection", IsRequired = true)]
        public AssetConnection Connection
        {
            get
            {
                return (AssetConnection)this["Connection"];
            }
        }
	}

    public class AssetConnection : ConfigurationElement
    {
        public AssetConnection() { }

        [ConfigurationProperty("storageConnectionStringName", IsRequired = false, DefaultValue = "DataConnectionString")]
        public string StorageConnectionStringName
        {
            get
            {
                return (string)this["storageConnectionStringName"];
            }
            set
            {
                this["storageConnectionStringName"] = value;
            }
        }



        [Obsolete("Use StorageConnectionStringName instead")]
        [ConfigurationProperty("storageFolder", IsRequired = false)]
        public string StorageFolder
        {
            get
            {
                return (string)this["storageFolder"];
            }
            set
            {
                this["storageFolder"] = value;
            }
        }

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

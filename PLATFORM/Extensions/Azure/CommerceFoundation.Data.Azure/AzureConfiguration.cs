namespace VirtoCommerce.Foundation.Data.Azure
{
    using System;
    using System.Configuration;
    using System.Threading;

    using Microsoft.WindowsAzure.Storage;

    using VirtoCommerce.Foundation.Data.Infrastructure;

    public class AzureConfiguration : ConfigurationSection
	{
		private static Lazy<AzureConfiguration> _instance = new Lazy<AzureConfiguration>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

		public static AzureConfiguration Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private static AzureConfiguration CreateInstance()
		{
            var config = (AzureConfiguration)ConfigurationManager.GetSection("VirtoCommerce/AzureRepository");

            if (config == null)
            {
                config = new AzureConfiguration { Connection = new AzureConnection() };
            }

            return config;
		}


		private CloudStorageAccount _storageAccount;
		[CLSCompliant(false)]
		public CloudStorageAccount AzureStorageAccount
		{
			get
			{
				if (this._storageAccount == null)
				{
				    var connectionName = AzureConfiguration.Instance.Connection.StorageConnectionStringName;
				    var connectionString = ConnectionHelper.GetConnectionString(connectionName);

                    if (!CloudStorageAccount.TryParse(connectionString, out this._storageAccount))
					{
						throw new InvalidOperationException("Failed to get valid connection string");
					}
				}
				return this._storageAccount;
			}
		}

		[ConfigurationProperty("Connection", IsRequired = true)]
		public AzureConnection Connection
		{
			get
			{
				return (AzureConnection)this["Connection"];
			}
            private set { this["Connection"] = value; }
		}

		[ConfigurationProperty("CQRSOrderQueueName", IsRequired = false)]
		public string CQRSOrderQueueName
		{
			get
			{
				return (string)this["CQRSOrderQueueName"];
			}
		}

		[ConfigurationProperty("CQRSOrderIndexingQueueName", IsRequired = false)]
		public string CQRSOrderIndexingQueueName
		{
			get
			{
				return (string)this["CQRSOrderIndexingQueueName"];
			}
		}

		[ConfigurationProperty("CQRSCatalogQueueName", IsRequired = false)]
		public string CQRSCatalogQueueName
		{
			get
			{
				return (string)this["CQRSCatalogQueueName"];
			}
		}

		[ConfigurationProperty("timeToProcessQueueMessage", IsRequired = false)]
		public double TimeToProcessQueueMessage
		{
			get
			{
				return (double)this["timeToProcessQueueMessage"];
			}
		}

		[ConfigurationProperty("CQRSCatalogIndexingQueueName", IsRequired = false, DefaultValue = "catalogitems-in")]
		public string CQRSCatalogIndexingQueueName
		{
			get
			{
				return (string)this["CQRSCatalogIndexingQueueName"];
			}
		}
	}

	public class AzureConnection : ConfigurationElement
	{
		public AzureConnection() { }

        [ConfigurationProperty("storageConnectionStringName", IsRequired = true, DefaultValue = "AssetsConnectionString")]
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
}

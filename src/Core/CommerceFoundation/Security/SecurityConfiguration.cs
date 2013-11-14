using System;
using System.Configuration;
using System.Threading;

namespace VirtoCommerce.Foundation.Security
{
	public class SecurityConfiguration : ConfigurationSection
	{
		private static Lazy<SecurityConfiguration> _instance = new Lazy<SecurityConfiguration>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

		public static SecurityConfiguration Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private static SecurityConfiguration CreateInstance()
		{
			return (SecurityConfiguration)ConfigurationManager.GetSection("VirtoCommerce/Security");
		}

		[ConfigurationProperty("Connection", IsRequired = true)]
		public SecurityConnection Connection
		{
			get
			{
				return (SecurityConnection)this["Connection"];
			}
		}

		[ConfigurationProperty("Authentication", IsRequired = true)]
		public AuthenticationConnection Authentication
		{
			get
			{
				return (AuthenticationConnection)this["Authentication"];
			}
		}

        [ConfigurationProperty("TokenIssuer")]
        public TokenIssuerConfigurationElement TokenIssuer { get { return (TokenIssuerConfigurationElement)this["TokenIssuer"]; } }

        [ConfigurationProperty("TokenValidator")]
        public TokenValidatorConfigurationElement TokenValidator { get { return (TokenValidatorConfigurationElement)this["TokenValidator"]; } }
    }

	public class SecurityConnection : ConfigurationElement
	{
		public SecurityConnection() { }

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

	public class AuthenticationConnection : ConfigurationElement
	{
		public AuthenticationConnection() { }

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

    public class TokenIssuerConfigurationElement : ConfigurationElement
    {
        private const string _uri = "uri";
        private const string _lifetime = "lifetime";
        private const string _signatureKey = "signatureKey";

        [ConfigurationProperty(_uri)]
        public Uri Uri
        {
            get { return (Uri)this[_uri]; }
            set { this[_uri] = value; }
        }

        [ConfigurationProperty(_lifetime, DefaultValue = "1:0:0")]
        public TimeSpan Lifetime
        {
            get { return (TimeSpan)this[_lifetime]; }
            set { this[_lifetime] = value; }
        }

        [ConfigurationProperty(_signatureKey, IsRequired = true)]
        public string SignatureKey
        {
            get { return (string)this[_signatureKey]; }
            set { this[_signatureKey] = value; }
        }
    }

    public class TokenValidatorConfigurationElement : ConfigurationElement
    {
        private const string _trustedIssuerUri = "trustedIssuerUri";
        private const string _signatureKey = "signatureKey";

        [ConfigurationProperty(_trustedIssuerUri)]
        public Uri TrustedIssuerUri
        {
            get { return (Uri)this[_trustedIssuerUri]; }
            set { this[_trustedIssuerUri] = value; }
        }

        [ConfigurationProperty(_signatureKey, IsRequired = true)]
        public string SignatureKey
        {
            get { return (string)this[_signatureKey]; }
            set { this[_signatureKey] = value; }
        }
    }

}

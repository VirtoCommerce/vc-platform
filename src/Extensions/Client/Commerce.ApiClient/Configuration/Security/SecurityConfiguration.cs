#region

using System;
using System.Configuration;
using System.Threading;

#endregion

namespace VirtoCommerce.ApiClient.Configuration.Security
{
    public class SecurityConfiguration : ConfigurationSection
    {
        #region Static Fields

        private static readonly Lazy<SecurityConfiguration> _instance = new Lazy<SecurityConfiguration>(
            CreateInstance,
            LazyThreadSafetyMode.ExecutionAndPublication);

        #endregion

        #region Public Properties

        public static SecurityConfiguration Instance
        {
            get { return _instance.Value; }
        }

        [ConfigurationProperty("Connection", IsRequired = true)]
        public SecurityConnection Connection
        {
            get { return (SecurityConnection)this["Connection"]; }
        }

        #endregion

        #region Methods

        private static SecurityConfiguration CreateInstance()
        {
            return (SecurityConfiguration)ConfigurationManager.GetSection("VirtoCommerce/Security");
        }

        #endregion

        //[ConfigurationProperty("Authentication", IsRequired = true)]
        //public AuthenticationConnection Authentication
        //{
        //    get
        //    {
        //        return (AuthenticationConnection)this["Authentication"];
        //    }
        //}

        //[ConfigurationProperty("TokenIssuer")]
        //public TokenIssuerConfigurationElement TokenIssuer { get { return (TokenIssuerConfigurationElement)this["TokenIssuer"]; } }

        //[ConfigurationProperty("TokenValidator")]
        //public TokenValidatorConfigurationElement TokenValidator { get { return (TokenValidatorConfigurationElement)this["TokenValidator"]; } }
    }

    public class SecurityConnection : ConfigurationElement
    {
        #region Constructors and Destructors

        public SecurityConnection()
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

    public class AuthenticationConnection : ConfigurationElement
    {
        #region Constructors and Destructors

        public AuthenticationConnection()
        {
        }

        #endregion

        #region Public Properties

        [ConfigurationProperty("serviceBaseUriName", IsRequired = false)]
        public string ServiceBaseUriName
        {
            get { return (string)this["serviceBaseUriName"]; }
            set { this["serviceBaseUriName"] = value; }
        }

        [ConfigurationProperty("serviceUri", IsRequired = false)]
        public string ServiceUri
        {
            get { return (string)this["serviceUri"]; }
            set { this["serviceUri"] = value; }
        }

        [ConfigurationProperty("wsEndPointName", IsRequired = false)]
        public string WSEndPointName
        {
            get { return (string)this["wsEndPointName"]; }
            set { this["wsEndPointName"] = value; }
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

    public class TokenIssuerConfigurationElement : ConfigurationElement
    {
        #region Constants

        private const string _lifetime = "lifetime";

        private const string _signatureKey = "signatureKey";

        private const string _uri = "uri";

        #endregion

        #region Public Properties

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

        [ConfigurationProperty(_uri)]
        public Uri Uri
        {
            get { return (Uri)this[_uri]; }
            set { this[_uri] = value; }
        }

        #endregion
    }

    public class TokenValidatorConfigurationElement : ConfigurationElement
    {
        #region Constants

        private const string _signatureKey = "signatureKey";

        private const string _trustedIssuerUri = "trustedIssuerUri";

        #endregion

        #region Public Properties

        [ConfigurationProperty(_signatureKey, IsRequired = true)]
        public string SignatureKey
        {
            get { return (string)this[_signatureKey]; }
            set { this[_signatureKey] = value; }
        }

        [ConfigurationProperty(_trustedIssuerUri)]
        public Uri TrustedIssuerUri
        {
            get { return (Uri)this[_trustedIssuerUri]; }
            set { this[_trustedIssuerUri] = value; }
        }

        #endregion
    }
}

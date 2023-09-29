namespace VirtoCommerce.Platform.Security.ExternalSignIn
{
    public class ExternalSignInProviderConfiguration
    {
        /// <summary>
        /// AzureAD, Google etc
        /// </summary>
        public string AuthenticationType { get; set; }

        /// <summary>
        /// Provider implementation
        /// </summary>
        public IExternalSignInProvider Provider { get; set; }

        /// <summary>
        /// Provider logo url (ex '/Modules/$(ModuleId)/Content/provider-logo.png')
        /// </summary>
        public string LogoUrl { get; set; }
    }
}

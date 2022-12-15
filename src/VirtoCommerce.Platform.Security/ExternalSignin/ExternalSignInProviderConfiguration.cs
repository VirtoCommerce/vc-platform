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
    }
}

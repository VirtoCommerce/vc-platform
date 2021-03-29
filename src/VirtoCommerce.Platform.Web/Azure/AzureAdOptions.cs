namespace VirtoCommerce.Platform.Web.Azure
{
    public class AzureAdOptions
    {
        /// <summary>
        /// Determines whether the user authentication via Azure Active Directory is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Sets AuthenticationType value for Azure AD authentication provider.
        /// </summary>
        public string AuthenticationType { get; set; }

        /// <summary>
        /// Sets human-readable caption for Azure AD authentication provider. It is visible on sign-in page.
        /// </summary>
        public string AuthenticationCaption { get; set; }

        /// <summary>
        /// Application ID of the VirtoCommerce platform application registered in Azure Active Directory. It can be found 
        /// in the Azure control panel: Azure Active Directory > App registrations > (platform app) > Application ID
        /// (e.g. 01234567-89ab-cdef-0123-456789abcdef).
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// ID of the Azure AD domain that will be used for authentication. It can be found in the Azure control panel:
        /// Azure Active Directory > Properties > Directory ID (e.g. abcdef01-2345-6789-abcd-ef0123456789).
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// URL of the Azure AD endpoint used for authentication (usually https://login.microsoftonline.com/).
        /// </summary>
        public string AzureAdInstance { get; set; }

        /// <summary>
        /// Default user type for users created by Azure AD accounts.
        /// </summary>
        public string DefaultUserType { get; set; }

        //
        // Summary:
        //     Gets or sets the discovery endpoint for obtaining metadata
        public string MetadataAddress { get; set; }

    }
}

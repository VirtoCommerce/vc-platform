namespace VirtoCommerce.Platform.Core.Security.ExternalSignIn;

public class ExternalSignInRequest
{
    public string AuthenticationType { get; set; }
    public string ReturnUrl { get; set; }
    public string StoreId { get; set; }
    public string OidcUrl { get; set; }
    public string CallbackUrl { get; set; }
}

using OpenIddict.Abstractions;

namespace VirtoCommerce.Platform.Security.Model
{
    public class TokenLoginResponse : OpenIddictResponse
    {
        public string UserId { get; set; }
    }
}

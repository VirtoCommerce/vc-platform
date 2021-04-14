using Microsoft.AspNetCore.Identity;

namespace VirtoCommerce.Platform.Security
{
    public class CustomIdentityError : IdentityError
    {
        /// <summary>
        /// Auxiliary error parameter: E.g., RequiredLength, etc.
        /// </summary>
        public int ErrorParameter { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace VirtoCommerce.Platform.Security
{
    public class CustomIdentityError : IdentityError
    {
        /// <summary>
        /// Auxiliary error parameter: E.g., RequiredLength, etc.
        /// </summary>
        public string Parameter { get; set; }

        public CustomIdentityError()
        {
        }

        public CustomIdentityError(IdentityError basicError, object parameter)
        {
            Code = basicError.Code;
            Description = basicError.Description;
            Parameter = parameter.ToString();
        }
    }
}

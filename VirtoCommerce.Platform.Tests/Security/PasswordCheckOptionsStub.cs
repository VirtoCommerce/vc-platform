using VirtoCommerce.Platform.Data.Security;

namespace VirtoCommerce.Platform.Tests.Security
{
    public class PasswordCheckOptionsStub : IPasswordCheckOptions
    {
        public int RequiredPasswordLength { get; set; }
        public bool RequireUpperCaseLetters { get; set; }
        public bool RequireLowerCaseLetters { get; set; }
        public bool RequireDigits { get; set; }
        public bool RequireSpecialCharacters { get; set; }
    }
}

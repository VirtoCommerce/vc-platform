namespace VirtoCommerce.Platform.Data.Security
{
    public interface IPasswordCheckOptions
    {
        int RequiredPasswordLength { get; }

        bool RequireUpperCaseLetters { get; }

        bool RequireLowerCaseLetters { get; }

        bool RequireDigits { get; }

        bool RequireSpecialCharacters { get; }
    }
}

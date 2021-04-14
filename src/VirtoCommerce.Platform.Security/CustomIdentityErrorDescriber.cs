using Microsoft.AspNetCore.Identity;

namespace VirtoCommerce.Platform.Security
{
    /// <summary>
    /// Overriding https://github.com/dotnet/aspnetcore/blob/release/3.1/src/Identity/Extensions.Core/src/IdentityErrorDescriber.cs
    /// </summary>
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is not long enough.</param>
        /// <returns>An <see cref="IdentityError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.</returns>
        public override IdentityError PasswordTooShort(int length)
        {
            return new CustomIdentityError
            {
                Code = nameof(PasswordTooShort),
                ErrorParameter = length
            };
        }

        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password does not meet the minimum number <paramref name="uniqueChars"/> of unique chars.
        /// </summary>
        /// <param name="uniqueChars">The number of different chars that must be used.</param>
        /// <returns>An <see cref="IdentityError"/> indicating a password does not meet the minimum number <paramref name="uniqueChars"/> of unique chars.</returns>
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new CustomIdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                ErrorParameter = uniqueChars
            };
        }
    }
}

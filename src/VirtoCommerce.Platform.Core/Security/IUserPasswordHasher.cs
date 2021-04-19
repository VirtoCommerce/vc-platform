using System;
using Microsoft.AspNetCore.Identity;

namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Basic interface for platform password hashers
    /// </summary>
    [Obsolete("Use IPasswordHasher<ApplicationUser> instead. UserPasswordsHistory is available from ISecurityRepository")]
    public interface IUserPasswordHasher : IPasswordHasher<ApplicationUser>
    {
    }
}

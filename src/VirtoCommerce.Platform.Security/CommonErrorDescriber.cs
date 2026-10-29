using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Security;

public static class CommonErrorDescriber
{
    public static IdentityError OperationNotSupported()
    {
        return new IdentityError
        {
            Code = nameof(OperationNotSupported).ToSnakeCase(),
            Description = "This operation is not supported.",
        };
    }

    public static IdentityError UserNotEditable()
    {
        return new IdentityError
        {
            Code = nameof(UserNotEditable).ToSnakeCase(),
            Description = "It is forbidden to edit this user.",
        };
    }

    public static IdentityError UserNotFound()
    {
        return new IdentityError
        {
            Code = nameof(UserNotFound).ToSnakeCase(),
            Description = "User not found.",
        };
    }
}

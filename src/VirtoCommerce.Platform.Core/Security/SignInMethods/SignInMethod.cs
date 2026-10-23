using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Security.SignInMethods;

public class SignInMethod
{
    public string Kind { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string LogoUrl { get; set; }

    public int Priority { get; set; }

    public bool CanCreateKey { get; set; }

    public bool CanRenameKey { get; set; }

    public bool CanDeleteKey { get; set; }

    public IList<SignInKey> Keys { get; set; }
}

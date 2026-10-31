using System;

namespace VirtoCommerce.Platform.Core.Security.SignInMethods;

public class SignInKey
{
    public string Id { get; set; }

    public string Name { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string State { get; set; }
}

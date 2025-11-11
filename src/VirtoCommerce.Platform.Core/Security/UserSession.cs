using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security;

public class UserSession : Entity
{
    public DateTime CreatedDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string IpAddress { get; set; }

    public string UserAgent { get; set; }
}

using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace VirtoCommerce.Platform.Core.Security
{
    public class SecurityResult
    {
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public IdentityError[] IdentityErrors { get; set; }
    }
}

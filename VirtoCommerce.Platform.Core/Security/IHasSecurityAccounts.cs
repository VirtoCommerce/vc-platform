﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security
{
    public interface IHasSecurityAccounts
    {
        /// <summary>
        /// All security accounts 
        /// </summary>
        ICollection<ApplicationUserExtended> SecurityAccounts { get; }
    }
}

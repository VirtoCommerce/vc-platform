using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.CoreModule.Web.Model
{
    public class StorefrontUser : ApplicationUserExtended
    {
        /// <summary>
        /// List of stores which  user can sing in
        /// </summary>
        public IEnumerable<string> AlowedStores { get; set; }
    }
}
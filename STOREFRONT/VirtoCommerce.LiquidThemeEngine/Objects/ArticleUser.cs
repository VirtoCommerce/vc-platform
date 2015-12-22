using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class ArticleUser : Drop
    {
        #region Public Properties
        public string AccountOwner { get; set; }

        public string Bio { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string Homepage { get; set; }

        public string LastName { get; set; }
        #endregion
    }
}

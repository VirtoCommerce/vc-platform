using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Comment : Drop
    {
        #region Public Properties
        public string Author { get; set; }

        public string Content { get; set; }

        public string Email { get; set; }

        public string Id { get; set; }

        public string Status { get; set; }

        public string Url { get; set; }
        #endregion
    }
}

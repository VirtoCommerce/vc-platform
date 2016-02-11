using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.LiquidThemeEngine.Tags
{
    public class Part : Drop
    {
        #region Public Properties
        public bool IsLink { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Currency : Drop
    {
        public string CurrencyCode { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string EnglishName { get; set; }
    }
}

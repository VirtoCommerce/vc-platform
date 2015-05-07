using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.Web.Models.Searching
{
    public class FacetFilterValue : Drop
    {
        #region Public Properties

        public int Count { get; set; }

        public bool IsApplied { get; set; }

        public string Label { get; set; }

        public object Value { get; set; }

        #endregion
    }
}

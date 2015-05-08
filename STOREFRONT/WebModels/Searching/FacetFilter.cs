using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.Web.Models.Searching
{
    public class FacetFilter : Drop
    {
        #region Public Properties

        public string FacetType { get; set; }

        public string Field { get; set; }

        public string Label { get; set; }

        public FacetFilterValue[] Values { get; set; }

        #endregion
    }
}

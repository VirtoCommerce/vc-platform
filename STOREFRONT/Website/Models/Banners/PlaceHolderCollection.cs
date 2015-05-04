using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Web.Models.Banners
{
    public class PlaceHolderCollection : ItemCollection<PlaceHolder>
    {
        public PlaceHolderCollection(IEnumerable<PlaceHolder> collections)
            : base(collections)
        {
        }

        #region Overrides of ItemCollection<PlaceHolder>

        public override int TotalCount
        {
            get { return Size; }
        }

        #endregion
    }
}
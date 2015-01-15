using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using VirtoCommerce.Web.Core.DataContracts;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Converters
{
    public static class ItemConverters
    {

        public static ItemModel ToWebModel(this CatalogItem item)
        {
            var retVal = new ItemModel(item);
            return retVal;
        }
    }
}
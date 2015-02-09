using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class AssociationConverter
    {
        public static Association ToWebModel(this ProductAssociation source)
        {
            var retVal = new Association
            {
                ItemId = source.AssociatedProductId
            };

            retVal.InjectFrom(source);

            return retVal;
        }
    }
}

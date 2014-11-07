using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.MerchandisingModule.Data.Adaptors
{
    using VirtoCommerce.MerchandisingModule.Model;

    public static class CategoryAdaptor
    {
        public static Category AsCategory(this foundation.CategoryBase category)
        {
            if (category == null)
                return null;

            var retVal = new Category
            {
                Id = category.CategoryId,
                ParentId = category.ParentCategoryId,
                Code = category.Code
            };

            if (category is foundation.LinkedCategory)
            {
                retVal.Virtual = true;
            }

            return retVal;
        }
    }
}

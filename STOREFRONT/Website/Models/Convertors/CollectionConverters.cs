#region
using System;
using System.Linq;
using System.Web;
using Data = VirtoCommerce.ApiClient.DataContracts;

#endregion

namespace VirtoCommerce.Web.Models.Convertors
{
    public static class CollectionConverters
    {
        #region Public Methods and Operators
        public static Collection AsWebModel(this Data.Category category)
        {
            var path = VirtualPathUtility.ToAbsolute("~/collections/{0}");

            var newCollection = new Collection
                                {
                                    Id = category.Id,
                                    Handle = category.Code,
                                    Title = category.Name,
                                    Url = String.Format(path, category.Code),
                                    DefaultSortBy = "manual"
                                };

            if (category.Seo != null)
            {
                newCollection.Keywords = category.Seo.Select(k => k.AsWebModel());
            }

            if (category.Parents != null)
            {
                newCollection.Parents = category.Parents.Select(p => p.AsWebModel());
            }
            return newCollection;
        }
        #endregion
    }
}
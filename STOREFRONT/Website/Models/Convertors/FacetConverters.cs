#region
using Data = VirtoCommerce.ApiClient.DataContracts.Search;

#endregion

namespace VirtoCommerce.Web.Models.Convertors
{
    public static class FacetConverters
    {
        #region Public Methods and Operators
        public static Tag AsWebModel(this Data.FacetValue facetValue, string field)
        {
            return new Tag { Field = field, Label = facetValue.Label, Count = facetValue.Count };
        }
        #endregion
    }
}
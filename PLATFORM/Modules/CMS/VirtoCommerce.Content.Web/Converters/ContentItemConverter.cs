namespace VirtoCommerce.Content.Web.Converters
{
    #region

    using System;

    using VirtoCommerce.Content.Data.Models;

    #endregion

    public static class ContentItemConverter
    {
        #region Public Methods and Operators

        public static ContentItem ToDomainModel(this Models.ContentItem item)
        {
            var retVal = new ContentItem();

			retVal.CreatedDate = DateTime.UtcNow;
            retVal.Name = item.Name;
            retVal.Path = item.Path;
            
            return retVal;
        }

        public static Models.ContentItem ToWebModel(this ContentItem item)
        {
            var retVal = new Models.ContentItem();

            retVal.Name = item.Name;
            retVal.Path = item.Path;

            return retVal;
        }

        #endregion
    }
}
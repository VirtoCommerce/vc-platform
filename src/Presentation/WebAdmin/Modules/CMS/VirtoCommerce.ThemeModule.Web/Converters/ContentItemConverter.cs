namespace VirtoCommerce.ThemeModule.Web.Converters
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

            retVal.Content = item.Content;
            retVal.CreatedDate = DateTime.UtcNow;
            retVal.ContentType = item.Type == Models.ContentType.Directory ? ContentType.Directory : ContentType.File;
            retVal.Name = item.Name;
            retVal.Path = item.Path;
            retVal.ParentContentItemId = item.ParentContentItemId;

            return retVal;
        }

        public static Models.ContentItem ToWebModel(this ContentItem item)
        {
            var retVal = new Models.ContentItem();

            retVal.Content = item.Content;
            retVal.Name = item.Name;
            retVal.Path = item.Path;
            retVal.ParentContentItemId = item.ParentContentItemId;
            retVal.Type = item.ContentType == ContentType.Directory
                ? Models.ContentType.Directory
                : Models.ContentType.File;

            return retVal;
        }

        #endregion
    }
}
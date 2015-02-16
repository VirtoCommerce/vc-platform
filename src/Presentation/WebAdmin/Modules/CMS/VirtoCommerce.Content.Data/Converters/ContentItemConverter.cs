namespace VirtoCommerce.Content.Data.Converters
{
    #region

    using Octokit;

    using VirtoCommerce.Content.Data.Models;

    using ContentType = Octokit.ContentType;

    #endregion

    public static class ContentItemConverter
    {
        #region Public Methods and Operators

        public static ContentItem ToContentItem(this RepositoryContent repositoryContent)
        {
            ContentItem retVal = null;

            if (repositoryContent.Type == ContentType.File)
            {
                retVal = new ContentItem();

                retVal.Content = repositoryContent.Content;

                retVal.Name = repositoryContent.Name;
                retVal.Path = repositoryContent.Path;
            }

            return retVal;
        }

        #endregion
    }
}
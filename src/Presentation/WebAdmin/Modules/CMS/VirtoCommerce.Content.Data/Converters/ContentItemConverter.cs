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

        public static ContentItem RepositoryContent2ContentItem(RepositoryContent repositoryContent)
        {
            ContentItem retVal = null;

            if (repositoryContent.Type == ContentType.Dir || repositoryContent.Type == ContentType.File)
            {
                retVal = new ContentItem();

                retVal.Content = repositoryContent.Content;
                retVal.ContentType = repositoryContent.Type == ContentType.Dir
                    ? Models.ContentType.Directory
                    : Models.ContentType.File;

                retVal.Name = repositoryContent.Name;
                retVal.Path = repositoryContent.Path;
            }

            return retVal;
        }

        #endregion
    }
}
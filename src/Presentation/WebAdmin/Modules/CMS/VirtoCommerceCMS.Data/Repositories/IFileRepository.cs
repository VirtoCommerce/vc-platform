namespace VirtoCommerce.Content.Data.Repositories
{
    #region

    using VirtoCommerce.Content.Data.Models;

    #endregion

    public interface IFileRepository
    {
        #region Public Methods and Operators

        void DeleteContentItem(ContentItem item);

        ContentItem GetContentItem(string path);

        ContentItem[] GetContentItems(string path);

        void SaveContentItem(ContentItem item);

        #endregion
    }
}
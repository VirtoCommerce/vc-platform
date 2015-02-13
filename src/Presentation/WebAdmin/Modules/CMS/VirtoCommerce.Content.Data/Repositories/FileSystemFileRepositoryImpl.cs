namespace VirtoCommerce.Content.Data.Repositories
{
    #region

    using System;

    using VirtoCommerce.Content.Data.Models;

    #endregion

    public class FileSystemFileRepositoryImpl : IFileRepository
    {
        #region Fields

        #endregion

        #region Public Methods and Operators

        public void DeleteContentItem(ContentItem item)
        {
            throw new NotImplementedException();
        }

        //public FileSystemFileRepositoryImpl(string mainPath)
        //{
        //	_mainPath = mainPath;
        //}

        public ContentItem GetContentItem(string path)
        {
            throw new NotImplementedException();
        }

        public ContentItem[] GetContentItems(string path)
        {
            throw new NotImplementedException();
        }

        public void SaveContentItem(ContentItem item)
        {
            throw new NotImplementedException();
        }

        #endregion

        //private string 
    }
}
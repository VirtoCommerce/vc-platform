namespace VirtoCommerce.Content.Data.Repositories
{
    #region

    using System;

    using VirtoCommerce.Content.Data.Models;

    #endregion

    public class DatabaseFileRepositoryImpl : IFileRepository
	{
		private string _mainPath;

		#region

		public DatabaseFileRepositoryImpl(string mainPath)
		{
			_mainPath = mainPath;
		}

		#endregion

		#region Public Methods and Operators

		public void DeleteContentItem(ContentItem item)
        {
            throw new NotImplementedException();
        }

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
    }
}
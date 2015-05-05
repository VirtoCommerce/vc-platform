using Octokit;
using System.Text;
using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Converters
{
	public static class ContentItemConverter
	{
		#region Public Methods and Operators

		public static ContentItem ToContentItem(this RepositoryContent repositoryContent)
		{
			ContentItem retVal = null;

			if (repositoryContent.Type == ContentType.File)
			{
				retVal = new ContentItem();

				if (!string.IsNullOrEmpty(repositoryContent.Content))
				{
					retVal.ByteContent = Encoding.UTF8.GetBytes(repositoryContent.Content);
				}
				retVal.Name = repositoryContent.Name;
				retVal.Path = repositoryContent.Path;
			}

			return retVal;
		}

		#endregion
	}
}
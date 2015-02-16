namespace VirtoCommerce.Content.Data.Repositories
{
	#region

	using System.Linq;
	using VirtoCommerce.Content.Data.Models;

	#endregion

	public interface IFileRepository
	{
		#region Public Methods and Operators

		ContentItem GetContentItem(string path);

		ContentItem[] GetContentItems(string path, bool loadContent = false);

		Theme[] GetThemes(string storePath);

		void SaveContentItem(string path, ContentItem item);

		void DeleteContentItem(string path);

		#endregion
	}
}
namespace VirtoCommerce.Content.Data.Repositories
{
	#region

	using System.Linq;
	using VirtoCommerce.Content.Data.Models;

	#endregion

	public interface IFileRepository
	{
		#region Public Methods and Operators

		ContentItem GetContentItem(string themePath, string path);

		ContentItem[] GetContentItems(string themePath, string path);

		void SaveContentItem(string themePath, ContentItem item);

		void DeleteContentItem(string themePath, ContentItem item);

		#endregion
	}
}
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

		ContentItem[] GetContentItems(string path);

		void SaveContentItem(ContentItem item);

		void DeleteContentItem(ContentItem item);

		#endregion
	}
}
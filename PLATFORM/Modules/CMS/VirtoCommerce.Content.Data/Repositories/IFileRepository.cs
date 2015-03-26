using System.Collections.Generic;

namespace VirtoCommerce.Content.Data.Repositories
{
	#region

	using System.Linq;
	using System.Threading.Tasks;
	using VirtoCommerce.Content.Data.Models;

	#endregion

	public interface IFileRepository
	{
		#region Public Methods and Operators

		Task<ContentItem> GetContentItem(string path);

		Task<IEnumerable<ContentItem>> GetContentItems(string path, GetThemeAssetsCriteria criteria);

		Task<IEnumerable<Theme>> GetThemes(string storePath);

		Task<bool> SaveContentItem(string path, ContentItem item);

		Task<bool> DeleteContentItem(string path);

		Task<bool> DeleteTheme(string path);

		#endregion
	}
}
using System.Collections.Generic;

namespace VirtoCommerce.Content.Data.Repositories
{
	#region

	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using VirtoCommerce.Content.Data.Models;
	using VirtoCommerce.Platform.Core.Common;

	#endregion

	public interface IContentRepository : IDisposable
	{
		Task<ContentItem> GetContentItem(string path);

		Task<IEnumerable<ContentItem>> GetContentItems(string path, GetThemeAssetsCriteria criteria);

		Task<IEnumerable<Theme>> GetThemes(string storePath);

		Task<bool> SaveContentItem(string path, ContentItem item);

		Task<bool> DeleteContentItem(string path);

		Task<bool> DeleteTheme(string path);

		ContentPage GetPage(string path);

		IEnumerable<ContentPage> GetPages(string path, GetPagesCriteria criteria);

		void SavePage(string path, ContentPage page);

		void DeletePage(string path);
	}
}
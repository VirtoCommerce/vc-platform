using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Platform.Core.Common;
using System.Collections.Generic;

namespace VirtoCommerce.Content.Data.Repositories
{
	public interface IContentRepository : IDisposable
	{
		ContentItem GetContentItem(string path);

		IEnumerable<ContentItem> GetContentItems(string path, GetThemeAssetsCriteria criteria);

		IEnumerable<Theme> GetThemes(string storePath);

		void SaveContentItem(string path, ContentItem item);

		void DeleteContentItem(string path);

		void DeleteTheme(string path);

		ContentPage GetPage(string path);

		IEnumerable<ContentPage> GetPages(string path, GetPagesCriteria criteria);

		void SavePage(string path, ContentPage page);

		void DeletePage(string path);
	}
}
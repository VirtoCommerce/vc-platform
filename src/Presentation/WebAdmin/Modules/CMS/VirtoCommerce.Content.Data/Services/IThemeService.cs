using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Services
{
	public interface IThemeService
	{
		ThemeItem[] GetThemes(string storeId);
		void SetThemeAsActive(string storeId, string themeName);

		ContentItem[] GetContentItems(string path);
		ContentItem GetContentItem(string path);

		void SaveContentItem(ContentItem item);
		void DeleteContentItem(ContentItem item);
	}
}

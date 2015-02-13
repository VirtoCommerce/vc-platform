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
		//void SetThemeAsActive(string storeId, string themeName);

		ContentItem[] GetContentItems(string storeId, string themeName, string path);
		ContentItem GetContentItem(string storeId, string themeName, string path);

		void SaveContentItem(string storeId, string themeName, ContentItem item);
		void DeleteContentItem(string storeId, string themeName, ContentItem item);
	}
}

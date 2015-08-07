using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Services
{
	public interface IThemeService
	{
		IEnumerable<Theme> GetThemes(string storeId);

		IEnumerable<ThemeAsset> GetThemeAssets(string storeId, string themeId, GetThemeAssetsCriteria criteria);
		ThemeAsset GetThemeAsset(string storeId, string themeId, string assetKey);

		void DeleteTheme(string storeId, string themeId);

		void SaveThemeAsset(string storeId, string themeId, ThemeAsset asset);
		void DeleteThemeAssets(string storeId, string themeId, params string[] assetKey);

		void UploadTheme(string storeId, string themeName, ZipArchive archive);

		void CreateDefaultTheme(string storeId, string themePath);
	}
}

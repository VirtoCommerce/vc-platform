using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Services
{
	public interface IThemeService
	{
		Task<IEnumerable<Theme>> GetThemes(string storeId);

		Task<IEnumerable<ThemeAsset>> GetThemeAssets(string storeId, string themeId, GetThemeAssetsCriteria criteria);
		Task<ThemeAsset> GetThemeAsset(string storeId, string themeId, string assetKey);

		Task SaveThemeAsset(string storeId, string themeId, ThemeAsset asset);
		Task DeleteThemeAssets(string storeId, string themeId, params string[] assetKey);

		Task UploadTheme(string storeId, string themeName, ZipArchive archive);
	}
}

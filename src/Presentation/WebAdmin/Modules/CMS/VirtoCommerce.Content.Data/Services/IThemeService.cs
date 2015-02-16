using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Services
{
	public interface IThemeService
	{
		Theme[] GetThemes(string storeId);

		ThemeAsset[] GetThemeAssets(string storeId, string themeId, bool loadContent = false);
		ThemeAsset GetThemeAsset(string storeId, string themeId, string assetKey);

		void SaveThemeAsset(string storeId, string themeId, ThemeAsset asset);
		void DeleteThemeAssets(string storeId, string themeId, params string[] assetKey);
	}
}

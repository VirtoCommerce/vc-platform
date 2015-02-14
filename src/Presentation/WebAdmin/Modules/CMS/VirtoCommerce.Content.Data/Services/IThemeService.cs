using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Services
{
	public interface IThemeService
	{
		Theme[] GetThemes(string storeId);
		//void SetThemeAsActive(string storeId, string themeName);

		ThemeAsset[] GetThemeAssets(string storeId, string themeId);
        ThemeAsset GetThemeAsset(string storeId, string themeId, string assetKey);

		void SaveThemeAsset(string storeId, string themeName, ThemeAsset asset);
		void DeleteThemeAssets(string storeId, string themeName, string[] assetKey);
	}
}

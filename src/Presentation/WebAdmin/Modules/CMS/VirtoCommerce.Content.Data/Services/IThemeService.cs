using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Services
{
	public interface IThemeService
	{
		Theme[] GetThemes(string storeId);

		ThemeAsset[] GetThemeAssets(string storeId, string themeId, bool loadContent = false);
		ThemeAsset GetThemeAsset(string assetKey);

		void SaveThemeAsset(ThemeAsset asset);
		void DeleteThemeAssets(string[] assetKey);
	}
}

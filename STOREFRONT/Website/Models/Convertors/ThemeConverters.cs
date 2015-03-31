#region
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Web.Models.Storage;
using Data = VirtoCommerce.ApiClient.DataContracts.Themes;

#endregion

namespace VirtoCommerce.Web.Models.Convertors
{
    public static class ThemeConverters
    {
        public static Theme[] AsWebModel(this Data.Theme[] themes)
        {
            return themes == null ? null : themes.Select(t => t.AsWebModel()).ToArray();
        }

        #region Public Methods and Operators
        public static Theme AsWebModel(this Data.Theme theme)
        {
            var themeModel = new Theme { Id = theme.Name, Name = theme.Name, Path = theme.Path, Role = "main" };

            return themeModel;
        }

        public static ThemeAsset AsWebModel(this Data.ThemeAsset asset)
        {
            var ret = new ThemeAsset();
            ret.InjectFrom(asset);
            return ret;
        }

        public static FileAsset AsFileModel(this Data.ThemeAsset asset)
        {
            var ret = new FileAsset();
            ret.InjectFrom(asset);
            return ret;
        }

        public static FileAsset[] AsFileModel(this Data.ThemeAsset[] assets)
        {
            return assets == null ? null : assets.Select(t => t.AsFileModel()).ToArray();
        }
        #endregion
    }
}
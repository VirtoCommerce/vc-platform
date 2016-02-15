using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace VirtoCommerce.LiquidThemeEngine
{
    public interface ILiquidThemeEngine
    {
        string ResolveTemplatePath(string templateName, bool searchInGlobalThemeOnly = false);
        string RenderTemplateByName(string templateName, Dictionary<string, object> parameters);
        string RenderTemplate(string templateContent, Dictionary<string, object> parameters);
        IDictionary GetSettings(string defaultValue = null);
        JObject ReadLocalization();
        Stream GetAssetStream(string fileName, bool searchInGlobalThemeOnly = false);
        string GetAssetAbsoluteUrl(string assetName);
        string GetGlobalAssetAbsoluteUrl(string assetName);
    }
}

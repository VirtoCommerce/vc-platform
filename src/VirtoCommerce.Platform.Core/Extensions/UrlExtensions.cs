using System.Text;

namespace VirtoCommerce.Platform.Core.Extensions;

public static class UrlExtensions
{
    public static string TrimLastSlash(this string url)
    {
        var result = url.EndsWith('/') ? url[..^1] : url;

        return result;
    }

    /// <summary>
    /// Normalize values like "/reset/" and "reset" to "/reset"
    /// </summary>
    public static string NormalizeUrlSuffix(this string urlSuffix)
    {
        if (urlSuffix == "/")
        {
            return urlSuffix;
        }

        var result = new StringBuilder(urlSuffix);

        if (!string.IsNullOrEmpty(urlSuffix))
        {
            if (!urlSuffix.StartsWith('/'))
            {
                result.Insert(0, '/');
            }

            if (urlSuffix.EndsWith('/'))
            {
                result.Remove(result.Length - 1, 1);
            }
        }

        return result.ToString();
    }
}

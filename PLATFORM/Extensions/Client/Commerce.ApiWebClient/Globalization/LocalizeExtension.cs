using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace VirtoCommerce.ApiWebClient.Globalization
{
    /// <summary>
    /// Class LocalizeExtension.
    /// </summary>
    public static class LocalizeExtension
    {
        #region Methods
        /// <summary>
        /// Localizes the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>localized string.</returns>
        public static string Localize(this string source, CultureInfo culture)
        {
            return source.Localize(null, null, culture);
        }

        /// <summary>
        /// Localizes the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>localized string</returns>
        public static string Localize(this string source)
        {
            return source.Localize((string)null);
        }

        /// <summary>
        /// Localizes the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="format">string format</param>
        /// <returns>localized string</returns>
        public static string Localize(this string source, params object[] format)
        {
            return string.Format(source.Localize((string)null), format);
        }


        /// <summary>
        /// Localizes the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="category">The category.</param>
        /// <returns>localized string</returns>
        public static string Localize(this string source, string key, string category = "")
        {
            return source.Localize(key, category, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// Localizes the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="key">The key</param>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>localized string</returns>
        public static string Localize(this string source, string key, string category, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            var keySource = string.IsNullOrEmpty(key) ? source : key;
            key = Regex.Replace(keySource, @"\s+", "");
            key = key.Substring(0, Math.Min(key.Length, 128));

            //TODO: call API to localize
            //return source.Map(key, category, culture ?? System.Threading.Thread.CurrentThread.CurrentUICulture).Value;

            return source;
        }

        ///// <summary>
        ///// Maps the specified source.
        ///// </summary>
        ///// <param name="source">The source.</param>
        ///// <param name="key">The key.</param>
        ///// <param name="category">The category.</param>
        ///// <returns>Mapped Element.</returns>
        //public static Element Map(this string source, string key, string category = "")
        //{
        //    return source.Map(key, category, System.Threading.Thread.CurrentThread.CurrentUICulture);
        //}

        ///// <summary>
        ///// Maps the specified source.
        ///// </summary>
        ///// <param name="source">The source.</param>
        ///// <param name="key">The key.</param>
        ///// <param name="category">The category.</param>
        ///// <param name="culture">The culture.</param>
        ///// <returns>Mapped Element.</returns>
        //public static Element Map(this string source, string key, string category, CultureInfo culture)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //    {
        //        return Element.Empty;
        //    }

        //    var repository = ServiceLocator.Current.GetInstance<IElementRepository>();
        //    var element = repository.Get(key, category, culture.Name);
        //    if (element == null)
        //    {
        //        element = new Element { Name = key, Category = category, Culture = culture.Name, Value = source };
        //        repository.Add(element);
        //    }
        //    return element;
        //}

        ///// <summary>
        ///// Localizes the specified element.
        ///// </summary>
        ///// <param name="element">The element.</param>
        ///// <returns>localized string</returns>
        //public static string Localize(this Element element)
        //{
        //    return element.Value
        //        .Map(element.Name, element.Category, CultureInfo.GetCultureInfo(element.Culture))
        //        .Value;
        //}

        #endregion
    }
}

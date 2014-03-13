using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Markup;

// Register the types in the Microsoft's default namespaces
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "VirtoCommerce.ManagementClient.Localization")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2007/xaml/presentation", "VirtoCommerce.ManagementClient.Localization")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2008/xaml/presentation", "VirtoCommerce.ManagementClient.Localization")]

namespace VirtoCommerce.ManagementClient.Localization
{
    public static class LocalizationScope
    {
        #region Attached properties

        #region Culture

        /// <summary>
        /// The <see cref="CultureInfo"/> according to which values are formatted.
        /// </summary>
        /// <remarks>
        /// CAUTION! Setting this property does NOT automatically update localized values.
        /// Call <see cref="LocalizationScope.UpdateValues"/> for that purpose.
        /// </remarks>
        public static readonly DependencyProperty CultureProperty = DependencyProperty.RegisterAttached(
            "Culture",
            typeof(CultureInfo),
            typeof(LocalizationScope),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits
                )
            );

        public static CultureInfo GetCulture(DependencyObject obj)
        {
            return (CultureInfo)obj.GetValue(CultureProperty);
        }

        public static void SetCulture(DependencyObject obj, CultureInfo value)
        {
            obj.SetValue(CultureProperty, value);
        }

        #endregion

        #region UICulture

        /// <summary>
        /// The <see cref="CultureInfo"/> used to retrieve resources from <see cref="ResourceManager"/>.
        /// </summary>
        /// <remarks>
        /// CAUTION! Setting this property does NOT automatically update localized values.
        /// Call <see cref="LocalizationScope.UpdateValues"/> for that purpose.
        /// </remarks>
        public static readonly DependencyProperty UICultureProperty = DependencyProperty.RegisterAttached(
            "UICulture",
            typeof(CultureInfo),
            typeof(LocalizationScope),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits
                )
            );

        public static CultureInfo GetUICulture(DependencyObject obj)
        {
            return (CultureInfo)obj.GetValue(UICultureProperty);
        }

        public static void SetUICulture(DependencyObject obj, CultureInfo value)
        {
            obj.SetValue(UICultureProperty, value);
        }

        #endregion


        #region Category

        public const string DefaultCategory = "CM";

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.RegisterAttached("Category", typeof(string), typeof(LocalizationScope), new FrameworkPropertyMetadata(DefaultCategory, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetCategory(DependencyObject obj, string value)
        {
            obj.SetValue(CategoryProperty, value);
        }

        public static string GetCategory(DependencyObject obj)
        {
            return (string)obj.GetValue(CategoryProperty);
        }
        #endregion
    }

        #endregion

}

using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Localization
{
    public class LocalizingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
#if DEBUG
            if ((bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue)
            {
                value = null;
            }
#endif


            if (value != null)
            {
                var category = value.GetType().ToString();
                var idx = category.LastIndexOf(".Model");
                if (idx > 0)
                {
                    category = category.Substring(0, idx);
                    idx = category.LastIndexOf('.');
                    if (idx > 0)
                    {
                        category = category.Substring(idx + 1);
                    }
                }

                if (category.Length > 128)
                    category = category.Substring(0, 128);

                return AddSpacesToSentence(value.ToString()).Localize(null, category);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        static string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (char.IsUpper(text[i - 1]) && i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}

using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Localization
{
    public class SingleInstanceBase<T> where T : new()
    {
        private static readonly ThreadLocal<T> _instance = new ThreadLocal<T>(() => new T());
        public static T Current
        {
            get
            {
                return _instance.Value;
            }
        }
    }

    public class LocalizingConverter : SingleInstanceBase<LocalizingConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
#if DEBUG
            if ((bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue)
            {
                return value;
            }
#endif

            var result = value;

            string category;
            if (parameter != null)
            {
                category = parameter.ToString();
                result = AddSpacesToSentence(value.ToString()).Localize(null, category);
            }
            else if (value != null && targetType == typeof(string))// this converter can convert to string type only
            {
                category = value.GetType().ToString();
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
                else if (category.StartsWith("System") || category.StartsWith("VirtoCommerce.ManagementClient."))
                {
                    category = LocalizationScope.DefaultCategory;
                }

                if (category.Length > 128)
                    category = category.Substring(0, 128);

                result = AddSpacesToSentence(value.ToString()).Localize(null, category);
            }

            return result;
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
                if (char.IsUpper(text[i]) &&
                    (char.IsLower(text[i - 1]) ||
                        (char.IsUpper(text[i - 1]) && i < text.Length - 1 && !char.IsWhiteSpace(text[i + 1]) && !char.IsUpper(text[i + 1]))))
                    newText.Append(' ');

                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}

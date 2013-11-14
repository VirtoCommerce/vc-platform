using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
    public class ListViewItemIndexCheckConverter : IValueConverter
	{
		#region IValueConverter Members

        /// <summary>
        /// ListViewItem index checking
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>true, if ListViewItem index equals parameter</returns>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            int requiredIndex;
            var listViewItemIndex = int.MinValue;
            if (int.TryParse((string)parameter, out requiredIndex))
            {
                var item = (ListViewItem)value;

                //This following line returns a null ListView when the ListView.Resources block  
                //is uncommented out  
                var listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
                listViewItemIndex = listView.ItemContainerGenerator.IndexFromContainer(item);
            }

            return listViewItemIndex == requiredIndex;
		}
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

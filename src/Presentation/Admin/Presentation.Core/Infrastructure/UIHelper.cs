using System.Windows;
using System.Windows.Media;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public static class UIHelper
	{
		public static T FindAncestor<T>(DependencyObject obj) where T : DependencyObject
		{
			var retVal = default(T);
			while (obj != null)
			{
                //obj = VisualTreeHelper.GetParent(obj);
			    obj = GetParent(obj);

				var ancessor = obj as T;
				if (ancessor != null)
				{
					retVal = ancessor;
					break;
				}
			}
			return retVal;
		}

	    private static DependencyObject GetParent(object obj)
	    {
            ContentElement contentElement = obj as ContentElement;
            if (contentElement != null)
            {
                return ContentOperations.GetParent(contentElement);
            }
            
            FrameworkElement frameworkElement = obj as FrameworkElement;
            if (frameworkElement != null && frameworkElement.Parent != null)
	        {
	            return frameworkElement.Parent;
	        }

	        return VisualTreeHelper.GetParent(obj as DependencyObject);
	    }

	    public static T FindAncestor<T>(this UIElement obj) where T : UIElement
		{
			return FindAncestor<T>((DependencyObject)obj);
		}

		public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
		{
			T retVal = null;

			for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
			{
				var child  = VisualTreeHelper.GetChild(obj, i);

				if (child is T)
				{
					retVal = child as T;
					break;
				}

				child = FindVisualChild<T>(child);

				if (child is T)
				{
					retVal = child as T;
					break;
				}
			}
			return retVal;
		}
	}
}

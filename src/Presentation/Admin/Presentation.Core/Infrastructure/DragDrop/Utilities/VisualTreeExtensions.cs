using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.DragDrop.Utilities
{
    public static class VisualTreeExtensions
    {
        public static T GetVisualAncestor<T>(this DependencyObject d) where T : class
        {
            DependencyObject obj = VisualTreeHelper.GetParent(d);

            while (obj != null)
            {
                var ancessor = obj as T;
                if (ancessor != null) return ancessor;
                obj = VisualTreeHelper.GetParent(obj);
            }

            return null;
        }

        public static DependencyObject GetVisualAncestor(this DependencyObject d, Type type)
        {
            DependencyObject obj = VisualTreeHelper.GetParent(d);

            while (obj != null)
            {
                if (obj.GetType() == type) return obj;
                obj = VisualTreeHelper.GetParent(obj);
            }

            return null;
        }
        
        public static T GetVisualDescendent<T>(this DependencyObject d) where T : DependencyObject
        {
            return d.GetVisualDescendents<T>().FirstOrDefault();
        }
        
        public static IEnumerable<T> GetVisualDescendents<T>(this DependencyObject obj) where T : DependencyObject
        {
            
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    yield return (T)child;
                }

                foreach (T match in GetVisualDescendents<T>(child))
                {
                    yield return match;
                }
            }

            yield break;
        }
    }
}

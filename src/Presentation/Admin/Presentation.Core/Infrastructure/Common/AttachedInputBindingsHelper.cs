using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
    public static class AttachedInputBindingsHelper
    {
        public static readonly DependencyProperty InputBindingsProperty =
            DependencyProperty.RegisterAttached("InputBindings", typeof(InputBindingCollection), typeof(AttachedInputBindingsHelper),
            new FrameworkPropertyMetadata(new InputBindingCollection(),
            (sender, e) =>
            {
                var element = sender as UIElement;
                if (element == null) return;
                element.InputBindings.Clear();
                element.InputBindings.AddRange((InputBindingCollection)e.NewValue);
            }));

        public static InputBindingCollection GetInputBindings(UIElement element)
        {
            return (InputBindingCollection)element.GetValue(InputBindingsProperty);
        }

        public static void SetInputBindings(UIElement element, InputBindingCollection inputBindings)
        {
            element.SetValue(InputBindingsProperty, inputBindings);
        }

    }
}

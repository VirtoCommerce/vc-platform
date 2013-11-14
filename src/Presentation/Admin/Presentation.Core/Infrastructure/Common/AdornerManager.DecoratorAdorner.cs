using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
    partial class AdornerManager
    {
        private class DecoratorAdorner : Adorner
        {
            private UIElement _child;
            private AdornerLayer _adornerLayer;

            public DecoratorAdorner(FrameworkElement source, DataTemplate adorner)
                : base(source)
            {
                Debug.Assert(source != null);
                Debug.Assert(adorner != null);
                ContentPresenter contentPresenter = new ContentPresenter();
                contentPresenter.Content = source;
                contentPresenter.ContentTemplate = adorner;
                _child = contentPresenter;
                AddLogicalChild(_child);
                AddVisualChild(_child);
            }

            public DecoratorAdorner(FrameworkElement source, UIElement adorner)
                : base(source)
            {
                DataContext = source;
                _child = adorner;
                AddVisualChild(_child);
            }

            private FrameworkElement Source
            {
                get { return AdornedElement as FrameworkElement; }
            }

            protected override Size ArrangeOverride(Size finalSize)
            {
                _child.Arrange(new Rect(new Point(), finalSize));
                return finalSize;
            }

            protected override Visual GetVisualChild(int index)
            {
                if (index != 0)
                    throw new ArgumentOutOfRangeException("index");
                return _child;
            }

            protected override int VisualChildrenCount
            {
                get { return 1; }
            }

            public void Show()
            {
                Debug.Assert(_adornerLayer == null);

                if (!Source.IsLoaded)
                    Source.Loaded += new RoutedEventHandler(OnLoaded);
                else
                    AddToAdornerLayer();
            }

            private void OnLoaded(object sender, RoutedEventArgs e)
            {
                Source.Loaded -= new RoutedEventHandler(OnLoaded);
                AddToAdornerLayer();
            }

            private void AddToAdornerLayer()
            {
                _adornerLayer = AdornerLayer.GetAdornerLayer(AdornedElement);
                if (_adornerLayer != null)
                    _adornerLayer.Add(this);
            }

            public void Close()
            {
                Source.Loaded -= new RoutedEventHandler(OnLoaded);
                if (_adornerLayer != null)
                {
                    _adornerLayer.Remove(this);
                    _adornerLayer = null;
                }
            }
        }
    }
}

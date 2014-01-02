using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    /// <summary>
    /// Interaction logic for ExpressionBuilder.xaml
    /// </summary>
    public partial class ExpressionBuilder
    {
		public ExpressionBuilder()
        {
            InitializeComponent();
        }

        #region dependency properties
        public ExpressionElement RootExpression
        {
            get { return (ExpressionElement)GetValue(RootExpressionProperty); }
            set { SetValue(RootExpressionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RootExpression.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RootExpressionProperty =
            DependencyProperty.Register("RootExpression", typeof(ExpressionElement), typeof(ExpressionBuilder));
        #endregion

        private void CustomSelectorElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var parentElement = (FrameworkElement)sender;
            var parentExpression = parentElement.DataContext as CustomSelectorElement;
            parentExpression.ValueSelector();
        }

		//private void CustomDictionaryElement_PreviewTextInput(object sender, TextCompositionEventArgs e)
		//{
		//	foreach (var i in ((ComboBox)sender).Items)
		//	{
		//		if (i.ToString().ToUpper().Contains(e.TextComposition.CompositionText.ToUpper()))
		//		{
		//			((ComboBox)sender).SelectedItem = i;
		//			break;
		//		}
		//	}
		//	e.Handled = true;
		//}

		
		//private void AddBlock_Click(object sender, RoutedEventArgs e)
        //{
        //    var element = sender as FrameworkElement;
        //    element.ContextMenu.PlacementTarget = element;
        //    element.ContextMenu.IsOpen = true;
        //}

        void AddNewExpression_Click(object sender, RoutedEventArgs args)
        {
            
            // executing code on GUI thread in order to close the context menu before adding a child
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
				///this is not the best way to add element to the context but it works
				if (((MenuItem)sender).DataContext is TypedExpressionElementBase)
				{
					var control = sender as FrameworkElement;

					//// the commented part is the good way to get DataContext but it doesn't work
					//DependencyObject depObj = control;
					//while (depObj != null && !(depObj is ContextMenu))
					//{
					//	//depObj = LogicalTreeHelper.GetParent(depObj);
					//	depObj = VisualTreeHelper.GetParent(depObj);
					//}

					//if (depObj != null)
					//{
					//	ContextMenu menuItem = (ContextMenu)depObj;
					//	var parentControl = menuItem.PlacementTarget as FrameworkElement;
					//	var parentExpression = parentControl.DataContext as CompositeElement;
					//	parentExpression.Children.Add(control.DataContext as ExpressionElement);
					//}

					//DataContext passed in Tag
					var parentExpression = control.Tag as CompositeElement;
					parentExpression.Children.Add(control.DataContext as ExpressionElement);
					parentExpression.IsValid = parentExpression.Validate();
				}
            }));
        }

        void SelectNewValue_Click(object sender, RoutedEventArgs args)
        {
            // executing code on GUI thread in order to close the context menu before adding a child
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                var control = sender as FrameworkElement;

                DependencyObject depObj = control;
                while (!(depObj is ContextMenu))
                {
                    depObj = VisualTreeHelper.GetParent(depObj);
                }

                var menuItem = (ContextMenu)depObj;
                var parentControl = menuItem.PlacementTarget as FrameworkElement;
                var parentExpression = parentControl.DataContext as DictionaryElement;
                parentExpression.InputValue = control.DataContext;
            }));
        }

		void SelectDictNewValue_Click(object sender, RoutedEventArgs args)
		{
			// executing code on GUI thread in order to close the context menu before adding a child
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
			{
				var control = sender as FrameworkElement;

				DependencyObject depObj = control;
				while (!(depObj is ContextMenu))
				{
					depObj = VisualTreeHelper.GetParent(depObj);
				}

				var menuItem = (ContextMenu)depObj;
				var parentControl = menuItem.PlacementTarget as FrameworkElement;
				var parentExpression = parentControl.DataContext as KeyValueDictionaryElement;
				parentExpression.InputValue = ((System.Collections.Generic.KeyValuePair<string,string>) control.DataContext).Key;
				parentExpression.InputDisplayName = ((System.Collections.Generic.KeyValuePair<string, string>)control.DataContext).Value;
			}));
		}
    }
}

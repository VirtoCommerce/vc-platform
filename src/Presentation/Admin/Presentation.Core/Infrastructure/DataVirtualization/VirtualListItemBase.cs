using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace  VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization
{
    /// <summary>Base class of VirtualListItem&ltT&gt, should be named as VirtualListItem for better code readability
    /// however this will crash Visual Studio WPF Designer.</summary>
    public abstract class VirtualListItemBase
    {
        public static readonly DependencyProperty AutoLoadProperty = DependencyProperty.RegisterAttached("AutoLoad", typeof(bool), typeof(VirtualListItemBase),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnAutoLoadChanged)));

        static void OnAutoLoadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ContentControl.ContentProperty, typeof(DependencyObject));
            if (dpd == null)
                return;

            bool isEnabled = (bool)e.NewValue;
            if (isEnabled)
                dpd.AddValueChanged(d, OnContentChanged);
            else
                dpd.RemoveValueChanged(d, OnContentChanged);
        }

        static void OnContentChanged(object sender, EventArgs e)
        {
            VirtualListItemBase item = ((DependencyObject)sender).GetValue(ContentControl.ContentProperty) as VirtualListItemBase;
            if (item != null)
                item.LoadAsync();
        }

        public static bool GetAutoLoad(DependencyObject d)
        {
            return (bool)d.GetValue(AutoLoadProperty);
        }

        public static void SetAutoLoad(DependencyObject d, bool value)
        {
            d.SetValue(AutoLoadProperty, value);
        }

        public abstract bool IsLoaded { get; }

        public object Data
        {
            get { return GetData(); }
        }

        internal abstract object GetData();

        public abstract void Load();

        public abstract void LoadAsync();

    }
}

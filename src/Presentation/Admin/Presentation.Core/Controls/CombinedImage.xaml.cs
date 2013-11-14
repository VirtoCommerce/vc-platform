using System.Windows;
using System.Windows.Controls;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    /// <summary>
    /// control for combining two images
    /// </summary>
    public partial class CombinedImage : UserControl
    {
        #region .ctor
        public CombinedImage()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        public object ImageSource1
        {
            get { return (object)GetValue(ImageSource1Property); }
            set { SetValue(ImageSource1Property, value); }
        }
        public static readonly DependencyProperty ImageSource1Property =
            DependencyProperty.Register("ImageSource1", typeof(object), typeof(CombinedImage), new UIPropertyMetadata(null));

        public object ImageSource2
        {
            get { return (object)GetValue(ImageSource2Property); }
            set { SetValue(ImageSource2Property, value); }
        }
        public static readonly DependencyProperty ImageSource2Property =
            DependencyProperty.Register("ImageSource2", typeof(object), typeof(CombinedImage), new UIPropertyMetadata(null));

        #endregion
    }
}


using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    public class VectorImage: UserControl
    {
        static VectorImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(VectorImage),
                new FrameworkPropertyMetadata(typeof(VectorImage)));
        }

        public VectorImage()
            : base()
        {
        }

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(VectorImage), new FrameworkPropertyMetadata(OnSourceChange));

        public object ImageResource
        {
            get { return (string)GetValue(ImageResourceProperty); }
            set { SetValue(ImageResourceProperty, value); }
        }

        public static readonly DependencyProperty ImageResourceProperty =
            DependencyProperty.Register("ImageResource", typeof(object), typeof(VectorImage));

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(VectorImage));


        private static void OnSourceChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vectorImage = (VectorImage)sender;
            if (vectorImage != null)
            {
                vectorImage.ImageResource = vectorImage.ImageSource != null
                                                          ? Application.Current.TryFindResource(vectorImage.ImageSource)
                                                          : null;
            }
        }
    }
}

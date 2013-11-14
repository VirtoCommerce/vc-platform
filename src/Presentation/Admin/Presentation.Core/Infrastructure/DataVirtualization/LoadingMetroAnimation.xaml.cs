using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization
{
    public enum AnimationScaleEnum
    {
        Small,
        Medium,
        Large
    }

    /// <summary>
    /// Interaction logic for LoadingAnimation.xaml
    /// </summary>
    public partial class LoadingMetroAnimation : UserControl
    {
        public LoadingMetroAnimation()
        {
            InitializeComponent();
        }


        #region Dependency Properties

        public static readonly DependencyProperty AnimationColorProperty =
            DependencyProperty.Register("AnimationColor",
                                        typeof(Brush),
                                        typeof(
                                            LoadingMetroAnimation
                                            ),
                                        new PropertyMetadata
                                            (new SolidColorBrush(Colors.White)));

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor",
                                        typeof(Brush),
                                        typeof(
                                            LoadingMetroAnimation
                                            ),
                                        new PropertyMetadata
                                            (new SolidColorBrush(Colors.Transparent
                                            )));

        public static readonly DependencyProperty ShowTextProperty =
            DependencyProperty.Register("ShowText",
                                        typeof(bool),
                                        typeof(
                                            LoadingMetroAnimation
                                            ),
                                        new PropertyMetadata
                                            (false));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                                        typeof(string),
                                        typeof(
                                            LoadingMetroAnimation
                                            ),
                                        new PropertyMetadata
                                            ("Loading..."));

        public static readonly DependencyProperty AnimationScaleProperty = DependencyProperty.Register("AnimationScale",
                                                                                                       typeof(AnimationScaleEnum),
                                                                                                       typeof(
                                                                                                           LoadingMetroAnimation),
                                                                                                       new FrameworkPropertyMetadata(AnimationScaleEnum.Small,
                                                                                                           new PropertyChangedCallback(OnAnimationScaleChanged)));


        #endregion


        #region Properties

        public Brush AnimationColor
        {
            get { return (Brush)GetValue(AnimationColorProperty); }
            set { SetValue(AnimationColorProperty, value); }
        }

        public Brush BackgroundColor
        {
            get { return (Brush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public bool ShowText
        {
            get { return (bool)GetValue(ShowTextProperty); }
            set { SetValue(ShowTextProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public AnimationScaleEnum AnimationScale
        {
            get { return (AnimationScaleEnum)GetValue(AnimationScaleProperty); }
            set { SetValue(AnimationScaleProperty, value); }
        }

        #endregion


        #region Handlers

        private static void OnAnimationScaleChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            AnimationScaleEnum enumValue = (AnimationScaleEnum)e.NewValue;
            LoadingMetroAnimation control = depObj as LoadingMetroAnimation;

            if (control == null)
                return;

            switch (enumValue)
            {
                case AnimationScaleEnum.Small:
                    control.scaleTransform.ScaleX = 1;
                    control.scaleTransform.ScaleY = 1;
                    control.path.RenderTransformOrigin = new Point(0.5, 0.5);
                    break;
                case AnimationScaleEnum.Medium:
                    control.scaleTransform.ScaleX = 1.5;
                    control.scaleTransform.ScaleY = 1.5;
                    control.path.RenderTransformOrigin = new Point(0.75, 0.75);
                    break;
                case AnimationScaleEnum.Large:
                    control.scaleTransform.ScaleX = 2;
                    control.scaleTransform.ScaleY = 2;
                    control.path.RenderTransformOrigin = new Point(1, 1);
                    break;
            }
        }

        #endregion
    }
}

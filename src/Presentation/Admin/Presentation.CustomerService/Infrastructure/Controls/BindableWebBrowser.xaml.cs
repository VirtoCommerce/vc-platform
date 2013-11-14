using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Controls
{
    /// <summary>
    /// Interaction logic for BindableWebBrowser.xaml
    /// </summary>
    public partial class BindableWebBrowser : UserControl
    {
        private const string _SkipSourceChange = "Skip";

        public BindableWebBrowser()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseBack, BrowseBack, CanBrowseBack));
            CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseForward, BrowseForward, CanBrowseForward));
            CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, Refresh, TrueCanExecute));
        }


        public string BindableSource
        {
            get { return (string)GetValue(BindableSourceProperty); }
            set { SetValue(BindableSourceProperty, value); }
        }

        public bool ShouldHandleNavigated
        {
            get { return (bool)GetValue(ShouldHandleNavigatedProperty); }
            set { SetValue(ShouldHandleNavigatedProperty, value); }
        }

        public static readonly DependencyProperty BindableSourceProperty =
                                                        DependencyProperty.RegisterAttached(
                                                                "BindableSource",
                                                                typeof(string),
                                                                typeof(BindableWebBrowser));

        public static readonly DependencyProperty ShouldHandleNavigatedProperty =
                                                        DependencyProperty.RegisterAttached(
                                                                "ShouldHandleNavigated",
                                                                typeof(Boolean),
                                                                typeof(BindableWebBrowser));

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == BindableSourceProperty)
            {
                BindableSourcePropertyChanged(e);
            }
            else if (e.Property == ShouldHandleNavigatedProperty)
            {
                ShouldHandleNavigatedPropertyChanged(e);
            }
        }

        public void ShouldHandleNavigatedPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (browser != null)
            {
                if ((bool)e.NewValue)
                {
                    browser.Navigated += new NavigatedEventHandler(Browser_Navigated);
                }
                else
                {
                    browser.Navigated -= new NavigatedEventHandler(Browser_Navigated);
                }
            }
        }

        public void BindableSourcePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (browser != null)
            {
                string newContent = e.NewValue as string;
                if (!_SkipSourceChange.Equals(browser.Tag))
                {
                    if (string.IsNullOrEmpty(newContent))
                    {
                        browser.Source = null;
                    }
                    else if (newContent.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                    {
                        browser.Source = new Uri(newContent);
                    }
                    else
                    {
                        browser.NavigateToString(newContent);
                    }
                }
            }
        }

        private void Browser_Navigated(object sender, NavigationEventArgs e)
        {
            WebBrowser browser = sender as WebBrowser;
            if (browser != null)
            {
                if (BindableSource != e.Uri.ToString())
                {
                    browser.Tag = _SkipSourceChange;
                    this.BindableSource = browser.Source.AbsoluteUri;
                    browser.Tag = null;
                }
            }
        }

        private void CanBrowseBack(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = browser.CanGoBack;
        }

        private void BrowseBack(object sender, ExecutedRoutedEventArgs e)
        {
            browser.GoBack();
        }

        private void CanBrowseForward(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = browser.CanGoForward;
        }

        private void BrowseForward(object sender, ExecutedRoutedEventArgs e)
        {
            browser.GoForward();
        }

        private void TrueCanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }

        private void Refresh(object sender, ExecutedRoutedEventArgs e)
        {
            try { browser.Refresh(); }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
    }

}

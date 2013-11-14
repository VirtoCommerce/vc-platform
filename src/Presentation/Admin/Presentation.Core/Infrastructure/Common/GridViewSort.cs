using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Documents;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Common
{
    public static class GridViewSort
    {
        #region Public attached properties

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(GridViewSort),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnCommandPropertyChanged)));

        private static void OnCommandPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ItemsControl listView = o as ItemsControl;
            if (listView != null)
            {
                if (!GetAutoSort(listView)) // Don't change click handler if AutoSort enabled
                {
                    if (e.OldValue != null && e.NewValue == null)
                    {
                        listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                    }
                    if (e.OldValue == null && e.NewValue != null)
                    {
                        listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                    }
                }
            }
        }

        public static bool GetAutoSort(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoSortProperty);
        }

        public static void SetAutoSort(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoSortProperty, value);
        }

        public static readonly DependencyProperty AutoSortProperty = DependencyProperty.RegisterAttached("AutoSort", typeof(bool), typeof(GridViewSort),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnAutoSortPropertyChanged)));

        private static void OnAutoSortPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ListView listView = o as ListView;
            if (listView != null)
            {
                if (GetCommand(listView) == null) // Don't change click handler if a command is set
                {
                    bool oldValue = (bool)e.OldValue;
                    bool newValue = (bool)e.NewValue;
                    if (oldValue && !newValue)
                        listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                    if (!oldValue && newValue)
                        listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                }
            }
        }

        public static string GetPropertyName(DependencyObject obj)
        {
            return (string)obj.GetValue(PropertyNameProperty);
        }

        public static void SetPropertyName(DependencyObject obj, string value)
        {
            obj.SetValue(PropertyNameProperty, value);
        }

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.RegisterAttached("PropertyName", typeof(string), typeof(GridViewSort),
                new FrameworkPropertyMetadata(null));

        private static readonly DependencyPropertyKey SortOrderPropertyKey = DependencyProperty.RegisterAttachedReadOnly("SortOrder", typeof(SortOrder), typeof(GridViewSort),
            new FrameworkPropertyMetadata(SortOrder.None));
        public static readonly DependencyProperty SortOrderProperty = SortOrderPropertyKey.DependencyProperty;

        public static SortOrder GetSortOrder(GridViewColumnHeader element)
        {
            return (SortOrder)element.GetValue(SortOrderProperty);
        }

        private static void SetSortOrder(GridViewColumnHeader element, SortOrder value)
        {
            element.SetValue(SortOrderPropertyKey, value);
        }

        public static ImageSource GetSortGlyphAscending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphAscendingProperty);
        }

        public static void SetSortGlyphAscending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphAscendingProperty, value);
        }

        // Using a DependencyProperty as the backing store for SortGlyphAscending.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SortGlyphAscendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphAscending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

        public static ImageSource GetSortGlyphDescending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphDescendingProperty);
        }

        public static void SetSortGlyphDescending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphDescendingProperty, value);
        }

        // Using a DependencyProperty as the backing store for SortGlyphDescending.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SortGlyphDescendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphDescending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));
        #endregion

        #region Private attached properties

        private static GridViewColumnHeader GetSortedColumnHeader(DependencyObject obj)
        {
            return (GridViewColumnHeader)obj.GetValue(SortedColumnHeaderProperty);
        }

        private static void SetSortedColumnHeader(DependencyObject obj, GridViewColumnHeader value)
        {
            obj.SetValue(SortedColumnHeaderProperty, value);
        }

        private static readonly DependencyProperty SortedColumnHeaderProperty = DependencyProperty.RegisterAttached("SortedColumnHeader", typeof(GridViewColumnHeader), typeof(GridViewSort),
            new FrameworkPropertyMetadata(null));

        #endregion

        #region Column header click event handler

        private static void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null && headerClicked.Column != null)
            {
                string propertyName = GetSortPropertyName(headerClicked.Column);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    ListView listView = GetAncestor<ListView>(headerClicked);
                    if (listView != null)
                    {
                        ICommand command = GetCommand(listView);
                        if (command != null)
                        {
                            if (command.CanExecute(propertyName))
                                command.Execute(propertyName);
                        }
                        else if (GetAutoSort(listView))
                        {
                            ApplySort(listView, headerClicked, propertyName);
                        }
                    }
                }
            }
        }

        private static string GetSortPropertyName(GridViewColumn gridViewColumn)
        {
            string result = null;
            var propertyName = GetPropertyName(gridViewColumn);
            if (!string.IsNullOrEmpty(propertyName))
            {
                result = propertyName;
            }
            else
            {
                System.Windows.Data.Binding aa;
                if ((aa = gridViewColumn.DisplayMemberBinding as System.Windows.Data.Binding) != null)
                {
                    result = aa.Path.Path;
                }
            }
            return result;
        }

        #endregion

        #region Helper methods

        public static T GetAncestor<T>(DependencyObject reference)
            where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(reference);
            while (!(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            if (parent != null)
                return (T)parent;
            else
                return null;
        }

        static ListSortDirection GetSortDirection(ListView listView, GridViewColumnHeader currentSortedColumnHeader, GridViewColumnHeader clickedColumnHeader, string propertyName)
        {
            if (currentSortedColumnHeader != clickedColumnHeader)
                return ListSortDirection.Ascending;

            ICollectionView view = listView.Items;
            if (view.SortDescriptions.Count == 0)
                return ListSortDirection.Ascending;

            SortDescription currentSort = view.SortDescriptions[0];
            if (currentSort.PropertyName == propertyName)
            {
                if (currentSort.Direction == ListSortDirection.Ascending)
                    return ListSortDirection.Descending;
                else
                    return ListSortDirection.Ascending;
            }
            else
                return ListSortDirection.Ascending;
        }

        static void ApplySort(ListView listView, GridViewColumnHeader clickedColumnHeader, string propertyName)
        {
            GridViewColumnHeader currentSortedColumnHeader = GetSortedColumnHeader(listView);
            ListSortDirection direction = GetSortDirection(listView, currentSortedColumnHeader, clickedColumnHeader, propertyName);

            ICollectionView view = listView.Items;
            using (view.DeferRefresh())
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
            }
            
            if (currentSortedColumnHeader != null)
            {
                RemoveSortGlyph(currentSortedColumnHeader);
                if (currentSortedColumnHeader != clickedColumnHeader)
                {
                    SetSortOrder(currentSortedColumnHeader, SortOrder.None);
                }
            }
            AddSortGlyph(
                clickedColumnHeader,
                direction,
                direction == ListSortDirection.Ascending ? GetSortGlyphAscending(listView) : GetSortGlyphDescending(listView));
            SetSortedColumnHeader(listView, clickedColumnHeader);
            SetSortOrder(clickedColumnHeader, direction == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending);
        }

        private static void AddSortGlyph(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            adornerLayer.Add(
                new SortGlyphAdorner(
                    columnHeader,
                    direction,
                    sortGlyph
                    ));
        }

        private static void RemoveSortGlyph(GridViewColumnHeader columnHeader)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            Adorner[] adorners = adornerLayer.GetAdorners(columnHeader);
            if (adorners != null)
            {
                foreach (Adorner adorner in adorners)
                {
                    if (adorner is SortGlyphAdorner)
                        adornerLayer.Remove(adorner);
                }
            }
        }
        #endregion

        #region SortGlyphAdorner nested class

        private class SortGlyphAdorner : Adorner
        {
            private GridViewColumnHeader _columnHeader;
            private ListSortDirection _direction;
            private ImageSource _sortGlyph;

            public SortGlyphAdorner(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
                : base(columnHeader)
            {
                _columnHeader = columnHeader;
                _direction = direction;
                _sortGlyph = sortGlyph;
            }

            private Geometry GetDefaultGlyph()
            {
                double x1 = _columnHeader.ActualWidth - 13;
                double x2 = x1 + 10;
                double x3 = x1 + 5;
                double y1 = _columnHeader.ActualHeight / 2 - 3;
                double y2 = y1 + 5;

                if (_direction == ListSortDirection.Ascending)
                {
                    double tmp = y1;
                    y1 = y2;
                    y2 = tmp;
                }

                PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
                pathSegmentCollection.Add(new LineSegment(new Point(x2, y1), true));
                pathSegmentCollection.Add(new LineSegment(new Point(x3, y2), true));

                PathFigure pathFigure = new PathFigure(
                    new Point(x1, y1),
                    pathSegmentCollection,
                    true);

                PathFigureCollection pathFigureCollection = new PathFigureCollection();
                pathFigureCollection.Add(pathFigure);

                PathGeometry pathGeometry = new PathGeometry(pathFigureCollection);
                return pathGeometry;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (_sortGlyph != null)
                {
                    double x = _columnHeader.ActualWidth - 13;
                    double y = _columnHeader.ActualHeight / 2 - 5;
                    Rect rect = new Rect(x, y, 10, 10);
                    drawingContext.DrawImage(_sortGlyph, rect);
                }
                else
                {
                    drawingContext.DrawGeometry(Brushes.LightGray, new Pen(Brushes.Gray, 1.0), GetDefaultGlyph());
                }
            }
        }

        #endregion
    }
}
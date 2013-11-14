using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using models = VirtoCommerce.Foundation.Customers.Model;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel;
using System.Waf.Applications.Services;
using VirtoCommerce.ManagementClient.Customers.Model.Comparers;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Controls
{
    /// <summary>
    /// Interaction logic for LabelList.xaml
    /// </summary>
    public partial class LabelList : UserControl
    {
        public LabelList()
        {
            InitializeComponent();

            DeleteRequest = new InteractionRequest<Confirmation>();
        }



        #region Fields

        private ObservableCollection<models.Label> _labelsForListView = new ObservableCollection<models.Label>();
        private ObservableCollection<models.Label> _labelsForCombobox = new ObservableCollection<models.Label>();

        private ListCollectionView _labelCollectionView;

        #endregion

        #region DependencyPropeties

        public static readonly DependencyProperty AllLabelsProperty = DependencyProperty.Register("AllLabels", typeof(List<models.Label>), typeof(LabelList), new PropertyMetadata(new List<models.Label>(), AllLabelsPropertyChangedCallback));

        public static readonly DependencyProperty ExistingLabelsProperty = DependencyProperty.Register("ExistingLabels", typeof(ObservableCollection<models.Label>), typeof(LabelList), new PropertyMetadata(new ObservableCollection<models.Label>(), ExistingLabelsPropertyChangedCallback));




        #endregion

        #region Requests

        public InteractionRequest<Confirmation> DeleteRequest { get; private set; }


        #endregion

        #region Properties

        public List<models.Label> AllLabels
        {
            get { return (List<models.Label>)GetValue(AllLabelsProperty); }
            set
            {
                SetValue(AllLabelsProperty, value);
            }
        }

        public ObservableCollection<models.Label> ExistingLabels
        {
            get { return (ObservableCollection<models.Label>)GetValue(ExistingLabelsProperty); }
            set { SetValue(ExistingLabelsProperty, value); }
        }

        #endregion

        #region Callbacks

        static void AllLabelsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        static void ExistingLabelsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LabelList).UpdateContent();

        }

        #endregion

        #region PrivateMethods

        private void UpdateContent()
        {
            _labelsForCombobox = new ObservableCollection<models.Label>(AllLabels.Except(ExistingLabels, new LabelComparer()));
            _labelsForListView = ExistingLabels;
            nullItemsAdapter.ItemsSource = _labelsForCombobox;
            lview.ItemsSource = _labelsForListView;

            _labelCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(_labelsForCombobox);
            _labelCollectionView.SortDescriptions.Add(new SortDescription("LabelId", ListSortDirection.Ascending));
        }


        #endregion



        #region Handlers

        /// <summary>
        /// выбираем элемент в комбобоксе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLabels_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            cmbLabels.SelectionChanged -= cmbLabels_SelectionChanged_1;

            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                models.Label addedToListView = (e.AddedItems as IList<object>)[0] as models.Label;
                _labelsForListView.Add(addedToListView);
                _labelsForCombobox.Remove(addedToListView);
                e.Handled = true;

                if (nullItemsAdapter.ItemsSource != null)
                {

                    models.Label nullLabel = nullItemsAdapter.NullItems[0] as models.Label;
                    if (nullLabel != null)
                    {
                        cmbLabels.SelectedItem = nullLabel;
                    }
                }

            }

            cmbLabels.SelectionChanged += cmbLabels_SelectionChanged_1;
        }

        /// <summary>
        /// удаляем элемент из listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click_1(object sender, RoutedEventArgs e)
        {
            //if (DeleteRequest != null)
            //{

            //    DeleteRequest.Raise(
            //        new Confirmation()
            //            {
            //                Title = "Remove label?",
            //                Content = "Are you sure you want to delete the note?"
            //            },
            //            (x) =>
            //            {
            //                if (x.Confirmed)
            //                {
                                models.Label labelToCombo = (sender as Button).DataContext as models.Label;
                                _labelsForCombobox.Add(labelToCombo);
                                _labelsForListView.Remove(labelToCombo);
                            //}

                            _labelCollectionView.Refresh();
            //            }
            //        );
            //}
        }

        #endregion
    }



}

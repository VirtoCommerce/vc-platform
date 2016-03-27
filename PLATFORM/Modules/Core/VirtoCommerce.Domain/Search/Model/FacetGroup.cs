using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace VirtoCommerce.Domain.Search.Model
{
    public struct FacetTypes
    {
        public const string Attribute = "attr";
        public const string Range = "range";
        public const string PriceRange = "pricerange";
        public const string Category = "category";
    }

    public class FacetGroup : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; private set; }

        /// <summary>
        /// Gets the facet group labels.
        /// </summary>
        public FacetLabel[] Labels { get; private set; }

        /// <summary>
        /// Specifies facet type.
        /// </summary>
        /// <value>The name.</value>
        public string FacetType { get; set; }

        FacetCollection<Facet> _facets;
        /// <summary>
        /// Gets the facets.
        /// </summary>
        /// <value>The facets.</value>
        public FacetCollection<Facet> Facets
        {
            get
            {
                if (_facets == null)
                    _facets = new FacetCollection<Facet>(this);

                return _facets;
            }
        }

        public FacetGroup()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacetGroup"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="labels">Display names of the field.</param>
        public FacetGroup(string fieldName, FacetLabel[] labels)
        {
            FieldName = fieldName;
            Labels = labels;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var tmp = PropertyChanged;
            if (tmp != null)
            {
                tmp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class FacetCollection<T> : ObservableCollection<T> where T : Facet
    {
        protected string ParentKeyProperty { get; set; }
        protected string LocalParentKeyProperty { get; set; }

        public FacetGroup Parent { get; set; }


        public FacetCollection()
        {
        }


        public FacetCollection(FacetGroup parent)
        {
            Parent = parent;
            if (parent != null)
            {
                ((INotifyPropertyChanged)parent).PropertyChanged += StorageEntityCollection_PropertyChanged;
            }
        }


        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newItems = e.NewItems.Cast<T>();
                foreach (var item in newItems)
                {
                    item.Group = Parent;
                }
            }
        }

        private void StorageEntityCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}

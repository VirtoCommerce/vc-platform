using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace VirtoCommerce.Foundation.Search.Facets
{
	[DataContract/*Serializable*/]
	public class FacetGroup : INotifyPropertyChanged
	{
		/// <summary>
		/// Gets the name of the field.
		/// </summary>
		/// <value>The name of the field.</value>
		[DataMember]
		public string FieldName
		{
			get;
			private set;
		}

		/*
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		[DataMember]
		public string Name
		{
			get;
			private set;
		}
		 * */

		FacetCollection<Facet> _Facets = null;
		/// <summary>
		/// Gets the facets.
		/// </summary>
		/// <value>The facets.</value>
		[DataMember]
		public FacetCollection<Facet> Facets
		{
			get
			{
				if (_Facets == null)
					_Facets = new FacetCollection<Facet>(this);

				return _Facets;
			}
		}

		public FacetGroup()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FacetGroup"/> class.
		/// </summary>
		/// <param name="fieldName">Name of the field.</param>
		public FacetGroup(string fieldName/*, string name*/)
		{
			this.FieldName = fieldName;
			//this.Name = name;
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

	[Serializable]
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
			if (parent is INotifyPropertyChanged)
			{
				((INotifyPropertyChanged)parent).PropertyChanged += new PropertyChangedEventHandler(StorageEntityCollection_PropertyChanged);
			}
		}


		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				var newItems = e.NewItems.Cast<T>();
				foreach (T item in newItems)
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

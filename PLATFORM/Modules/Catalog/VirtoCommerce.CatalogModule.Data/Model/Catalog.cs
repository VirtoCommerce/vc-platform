using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.CatalogModule.Data.Model
{
    public class Catalog : CatalogBase
    {
		public Catalog()
		{
			PropertySets = new ObservableCollection<PropertySet>();
			CatalogLanguages = new ObservableCollection<CatalogLanguage>();
			CatalogPropertyValues = new ObservableCollection<CatalogPropertyValue>();
		}

		public int WeightMeasure { get; set; }

        #region Navigation Properties

		public virtual ObservableCollection<CatalogLanguage> CatalogLanguages { get; set; }

		public virtual ObservableCollection<PropertySet> PropertySets { get; set; }

		[StringLength(128)]
		[ForeignKey("PropertySet")]
		public string PropertySetId { get; set; }

		public virtual PropertySet PropertySet { get; set; }


		public virtual ObservableCollection<CatalogPropertyValue> CatalogPropertyValues { get; set; }

	    #endregion
    }
}

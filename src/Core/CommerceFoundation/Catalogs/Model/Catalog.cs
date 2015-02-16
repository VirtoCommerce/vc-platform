using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using System.Data.Services.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
    [DataContract]
    [EntitySet("CatalogBases")]
    public class Catalog : CatalogBase
    {
        private int _WeightMeasure;
        [DataMember]
        public int WeightMeasure
        {
            get
            {
                return _WeightMeasure;
            }
            set
            {
                SetValue(ref _WeightMeasure, () => this.WeightMeasure, value);
            }
        }

        #region Navigation Properties

        ObservableCollection<CatalogLanguage> _CatalogLanguages = null;
        [DataMember]
        public virtual ObservableCollection<CatalogLanguage> CatalogLanguages
        {
	        get
            {
                if (_CatalogLanguages == null)
                    _CatalogLanguages = new ObservableCollection<CatalogLanguage>();

                return _CatalogLanguages;
            }
	        set { _CatalogLanguages = value; }
        }

	    ObservableCollection<PropertySet> _Properties = null;
        [DataMember]
        public virtual ObservableCollection<PropertySet> PropertySets
        {
	        get
            {
                if (_Properties == null)
                    _Properties = new ObservableCollection<PropertySet>();

                return _Properties;
            }
			set { _Properties = value; }
        }

		private string _PropertySetId;
		[DataMember]
		[StringLength(128)]
		[ForeignKey("PropertySet")]
		public string PropertySetId
		{
			get
			{
				return _PropertySetId;
			}
			set
			{
				SetValue(ref _PropertySetId, () => this.PropertySetId, value);
			}
		}

		[DataMember]
		public virtual PropertySet PropertySet { get; set; }


		ObservableCollection<CatalogPropertyValue> _PropertyValues = null;
		[DataMember]
		public virtual ObservableCollection<CatalogPropertyValue> CatalogPropertyValues
		{
			get
			{
				if (_PropertyValues == null)
					_PropertyValues = new ObservableCollection<CatalogPropertyValue>();

				return _PropertyValues;
			}
			set { _PropertyValues = value; }
		}

	    #endregion
    }
}

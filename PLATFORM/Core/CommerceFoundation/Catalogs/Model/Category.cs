using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Foundation.Frameworks;
using System.Data.Services.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
    [DataContract]
	[EntitySet("CategoryBases")]
    public class Category : CategoryBase
    {
        private string _Name;
        [Required(ErrorMessage = "Field 'Category Name' is required.")]
        [DataMember]
        [StringLength(128)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetValue(ref _Name, () => this.Name, value);
            }
        }

        private DateTime _StartDate;
        [DataMember]
        public DateTime StartDate
        {
            get
            {
                return _StartDate;
            }
            set
            {
                SetValue(ref _StartDate, () => this.StartDate, value);
            }
        }

        private DateTime? _EndDate;
        [DataMember]
        public DateTime? EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                SetValue(ref _EndDate, () => this.EndDate, value);
            }
        }

        #region Navigation Properties

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

        ObservableCollection<CategoryPropertyValue> _PropertyValues = null;
        [DataMember]
        public virtual ObservableCollection<CategoryPropertyValue> CategoryPropertyValues
        {
	        get
            {
                if (_PropertyValues == null)
                    _PropertyValues = new ObservableCollection<CategoryPropertyValue>();

                return _PropertyValues;
            }
			set { _PropertyValues = value; }
        }

	    #endregion
    }
}

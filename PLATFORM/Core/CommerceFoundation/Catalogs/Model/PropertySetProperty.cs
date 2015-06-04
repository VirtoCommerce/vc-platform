using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
    [DataContract]
    [DataServiceKey("PropertySetPropertyId")]
    [EntitySet("PropertySetProperties")]
    public class PropertySetProperty : StorageEntity
    {
        public PropertySetProperty()
        {
            _PropertySetPropertyId = GenerateNewKey();
        }
        private string _PropertySetPropertyId;
        [Key]
        [StringLength(128)]
        [DataMember]
        public string PropertySetPropertyId
        {
            get
            {
                return _PropertySetPropertyId;
            }
            set
            {
                SetValue(ref _PropertySetPropertyId, () => this.PropertySetPropertyId, value);
            }
        }

        private int _Priority;
        [DataMember]
        public int Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                SetValue(ref _Priority, () => this.Priority, value);
            }
        }

        #region Navigation Properties

        private string _PropertyId;
        [DataMember]
        [StringLength(128)]
        [Required]
        public string PropertyId
        {
            get
            {
                return _PropertyId;
            }
            set
            {
                SetValue(ref _PropertyId, () => this.PropertyId, value);
            }
        }

        [DataMember]
        public virtual Property Property { get; set; }

        private string _PropertySetId;
        [DataMember]
        [StringLength(128)]
        [Required]
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
        [Parent]
        [ForeignKey("PropertySetId")]
        public virtual PropertySet PropertySet { get; set; }
        #endregion
    }
}

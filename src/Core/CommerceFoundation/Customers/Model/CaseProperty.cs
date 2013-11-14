using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Customers.Model
{
    [DataContract]
    [EntitySet("CaseProperties")]
    [DataServiceKey("CasePropertyId")]
    public class CaseProperty : StorageEntity
    {
        public CaseProperty()
        {
            _CasePropertyId = GenerateNewKey();
            Priority = 1;
        }

        private string _CasePropertyId;
        [Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [DataMember]
        public string CasePropertyId
        {
            get { return _CasePropertyId; }
            set
            {
                SetValue(ref _CasePropertyId, () => CasePropertyId, value);
            }
        }

        private int _Priority;
        [DataMember]
        public int Priority
        {
            get { return _Priority; }
            set { SetValue(ref _Priority, () => Priority, value); }
        }

        private string _Name;
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string Name
        {
            get { return _Name; }
            set { SetValue(ref _Name, () => Name, value); }
        }

        private string _FieldName;
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string FieldName
        {
            get { return _FieldName; }
            set { SetValue(ref _FieldName, () => FieldName, value); }
        }

        #region NavigationProperties

        private string _CasePropertySetId;

        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string CasePropertySetId
        {
            get { return _CasePropertySetId; }
            set
            {
                SetValue(ref _CasePropertySetId, () => CasePropertySetId, value);
            }
        }

        [DataMember]
        [ForeignKey("CasePropertySetId")]
        [Parent]
        public virtual CasePropertySet CasePropertySet { get; set; }
        #endregion
    }
}

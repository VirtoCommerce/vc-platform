using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Customers.Model
{
    [DataContract]
    [EntitySet("CasePropertySets")]
    [DataServiceKey("CasePropertySetId")]
    public class CasePropertySet : StorageEntity
    {
        public CasePropertySet()
        {
            _CasePropertySetId = GenerateNewKey();
            Priority = 1;
        }

        private string _CasePropertySetId;
        [Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [DataMember]
        public string CasePropertySetId
        {
            get { return _CasePropertySetId; }
            set
            {
                SetValue(ref _CasePropertySetId, () => CasePropertySetId, value);
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

        #region NavigationProperties

        private ObservableCollection<CaseProperty> _CaseProperties = null;
        [DataMember]
        public ObservableCollection<CaseProperty> CaseProperties
        {
            get
            {
                if (_CaseProperties == null)
                    _CaseProperties = new ObservableCollection<CaseProperty>();

                return _CaseProperties;
            }
        }

        #endregion
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Customers.Model
{
    [DataContract]
    [EntitySet("CaseRules")]
    [DataServiceKey("CaseRuleId")]
    public class CaseRule : StorageEntity
    {
        public CaseRule()
        {
            _CaseRuleId = GenerateNewKey();
            Priority = 1;
        }

        private string _CaseRuleId;
        [Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [DataMember]
        public string CaseRuleId
        {
            get { return _CaseRuleId; }
            set
            {
                SetValue(ref _CaseRuleId, () => CaseRuleId, value);
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

        private string _Description;
        [DataMember]
		[StringLength(512, ErrorMessage = "Only 512 characters allowed.")]
        public string Description
        {
            get { return _Description; }
            set { SetValue(ref _Description, () => Description, value); }
        }

        private int _Status;
        [DataMember]
        public int Status
        {
            get { return _Status; }
            set { SetValue(ref _Status, () => Status, value); }
        }

        private string _PredicateSerialized;
        [DataMember]
        public string PredicateSerialized
        {
            get
            {
                return _PredicateSerialized;
            }
            set { SetValue(ref _PredicateSerialized, () => PredicateSerialized, value); }
        }

        private string _PredicateVisualTreeSerialized;
        [DataMember]
        public string PredicateVisualTreeSerialized
        {
            get
            {
                return _PredicateVisualTreeSerialized;
            }
            set { SetValue(ref _PredicateVisualTreeSerialized, () => PredicateVisualTreeSerialized, value); }
		}

		#region Navigation Properties
		private ObservableCollection<CaseAlert> _alerts = null;
		[DataMember]
		public ObservableCollection<CaseAlert> Alerts
		{
			get
			{
				if (_alerts == null)
					_alerts = new ObservableCollection<CaseAlert>();

				return _alerts;
			}
		}
		#endregion
	}
}

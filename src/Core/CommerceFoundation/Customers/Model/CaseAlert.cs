using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Customers.Model
{
    [DataContract]
	[EntitySet("CaseAlerts")]
	[DataServiceKey("CaseAlertId")]
	public class CaseAlert: StorageEntity
    {
        public CaseAlert()
        {
			_CaseAlertId = GenerateNewKey();
        }

        private string _CaseAlertId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [DataMember]
        public string CaseAlertId
        {
            get { return _CaseAlertId; }
            set
            {
				SetValue(ref _CaseAlertId, () => CaseAlertId, value);
            }
        }

		private string _XslTemplate;
		[DataMember]
		public string XslTemplate
		{
			get { return _XslTemplate; }
			set { SetValue(ref _XslTemplate, () => XslTemplate, value); }
		}
        
        private string _HtmlBody;
        [DataMember]
        public string HtmlBody
        {
            get { return _HtmlBody; }
			set { SetValue(ref _HtmlBody, () => HtmlBody, value); }
        }

		private string _RedirectUrl;
		[DataMember]
		[StringLength(512, ErrorMessage = "Only 512 characters allowed.")]
		public string RedirectUrl
		{
			get { return _RedirectUrl; }
			set { SetValue(ref _RedirectUrl, () => RedirectUrl, value); }
		}

		#region Navigation

		private string _caseRuleId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string CaseRuleId
		{
			get { return _caseRuleId; }
			set { SetValue(ref _caseRuleId, () => CaseRuleId, value); }
		}

        [DataMember]
        [ForeignKey("CaseRuleId")]
        [Parent]
		public virtual CaseRule CaseRule { get; set; }
		#endregion
	}
}

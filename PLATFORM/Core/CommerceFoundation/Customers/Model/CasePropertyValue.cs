using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Customers.Model
{
    [DataContract]
    [EntitySet("CasePropertyValues")]
    public class CasePropertyValue : PropertyValueBase
    {
        #region NavigationProperties

        private string _CaseId;
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string CaseId
        {
            get { return _CaseId; }
            set { SetValue(ref _CaseId, () => CaseId, value); }
        }

        [DataMember]
        [ForeignKey("CaseId")]
        [Parent]
        public virtual Case Case { get; set; }
        #endregion
    }
}

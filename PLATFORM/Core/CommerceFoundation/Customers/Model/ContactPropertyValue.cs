using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Customers.Model
{
    [DataContract]
    [EntitySet("ContactPropertyValues")]
    public class ContactPropertyValue : PropertyValueBase
    {
        #region NavigationProperties

        private string _ContactId;
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ContactId
        {
            get { return _ContactId; }
            set { SetValue(ref _ContactId, () => ContactId, value); }
        }

        [DataMember]
        [ForeignKey("ContactId")]
        [Parent]
        public virtual Contact Contact { get; set; }
        #endregion
    }
}

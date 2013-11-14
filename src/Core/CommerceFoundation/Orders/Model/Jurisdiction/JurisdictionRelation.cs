using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model.Jurisdiction
{
    [DataContract]
    [EntitySet("JurisdictionRelations")]
    [DataServiceKey("JurisdictionRelationId")]
    public class JurisdictionRelation : StorageEntity
    {
        public JurisdictionRelation()
        {
            JurisdictionRelationId = GenerateNewKey();
        }

        private string _JurisdictionRelationId;
        [Key]
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string JurisdictionRelationId
        {
            get { return _JurisdictionRelationId; }
            set { SetValue(ref _JurisdictionRelationId, () => JurisdictionRelationId, value); }
        }

        #region NavigationProperties

        private string _JurisdictionId;
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string JurisdictionId
        {
            get
            {
                return _JurisdictionId;
            }
            set
            {
                SetValue(ref _JurisdictionId, () => this.JurisdictionId, value);
            }
        }

        [DataMember]
        [ForeignKey("JurisdictionId")]
        public virtual Jurisdiction Jurisdiction { get; set; }


        private string _JurisdictionGroupId;
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string JurisdictionGroupId
        {
            get
            {
                return _JurisdictionGroupId;
            }
            set
            {
                SetValue(ref _JurisdictionGroupId, () => this.JurisdictionGroupId, value);
            }
        }

        [DataMember]
        [ForeignKey("JurisdictionGroupId")]
        [Parent]
        public virtual JurisdictionGroup JurisdictionGroup { get; set; }

        #endregion
    }
}

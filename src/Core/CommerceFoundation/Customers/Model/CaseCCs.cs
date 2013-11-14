using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Customers.Model
{
    [DataContract]
    [EntitySet("CaseCCs")]
    [DataServiceKey("CaseCCId")]
    public class CaseCC : StorageEntity
    {

        public CaseCC()
        {
            CaseCCId = GenerateNewKey();
        }



        private string _caseCcId;
        [Key]
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string CaseCCId
        {
            get { return _caseCcId; }
            set { SetValue(ref _caseCcId, () => CaseCCId, value); }
        }



        #region NavigationProperty

        private string _caseId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string CaseId
        {
            get { return _caseId; }
            set
            {
                SetValue(ref _caseId, () => CaseCCId, value);
            }
        }

        [DataMember]
        [ForeignKey("CaseId")]
        [Parent]
        public virtual Case Case { get; set; }

        private string _accountId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [Required]
        public string AccountId
        {
            get { return _accountId; }
            set
            {
                SetValue(ref _accountId, () => AccountId, value);
            }
        }

        #endregion

    }
}

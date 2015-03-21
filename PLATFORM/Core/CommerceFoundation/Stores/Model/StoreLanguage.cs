using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Stores.Model
{
    [DataContract]
    [EntitySet("StoreLanguages")]
    [DataServiceKey("StoreLanguageId")]
    public class StoreLanguage : StorageEntity
    {
        public StoreLanguage()
        {
            _StoreLanguageId = GenerateNewKey();
        }

        private string _StoreLanguageId;
        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string StoreLanguageId
        {
            get
            {
                return _StoreLanguageId;
            }
            set
            {
                SetValue(ref _StoreLanguageId, () => this.StoreLanguageId, value);
            }
        }


        private string _LanguageCode;
        [Required]
        [StringLength(32)]
        [DataMember]
        public string LanguageCode
        {
            get
            {
                return _LanguageCode;
            }
            set
            {
                SetValue(ref _LanguageCode, () => this.LanguageCode, value);
            }
        }

        #region Navigation Properties

        private string _StoreId;
        [ForeignKey("Store")]
        [DataMember]
        [Required]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string StoreId
        {
            get
            {
                return _StoreId;
            }
            set
            {
                SetValue(ref _StoreId, () => this.StoreId, value);
            }
        }

        [DataMember]
        public Store Store { get; set; }

        #endregion
    }
}

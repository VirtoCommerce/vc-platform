using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Orders.Model.Taxes
{
    [DataContract]
    [EntitySet("TaxLanguages")]
    [DataServiceKey("TaxLanguageId")]
    public class TaxLanguage : StorageEntity
    {
        public TaxLanguage()
		{
			TaxLanguageId = GenerateNewKey();
		}

        private string _TaxLanguageId;
		[DataMember]
		[Key]
		[StringLength(128)]
        public string TaxLanguageId
		{
			get
			{
                return _TaxLanguageId;
			}
			set
			{
                SetValue(ref _TaxLanguageId, () => this.TaxLanguageId, value);
			}
		}

        private string _DisplayName;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [Required]
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set
            {
                SetValue(ref _DisplayName, () => this.DisplayName, value);
            }
        }

        private string _LanguageCode;
        [DataMember]
        [StringLength(32)]
        [Required]
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

        private string _TaxId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string TaxId
        {
            get
            {
                return _TaxId;
            }
            set
            {
                SetValue(ref _TaxId, () => this.TaxId, value);
            }
        }

        [DataMember]
        [ForeignKey("TaxId")]
        public Tax Tax { get; set; }

        #endregion
    }
}

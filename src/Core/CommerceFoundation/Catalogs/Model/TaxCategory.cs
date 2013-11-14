using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
    [DataContract]
    [DataServiceKey("TaxCategoryId")]
    [EntitySet("TaxCategories")]
    public class TaxCategory : StorageEntity
    {
        public TaxCategory()
        {
            TaxCategoryId = GenerateNewKey();
        }

        private string _TaxCategoryId;
        [Key]
        [StringLength(64)]
        [DataMember]
        public string TaxCategoryId
        {
            get
            {
                return _TaxCategoryId;
            }
            set
            {
                SetValue(ref _TaxCategoryId, () => this.TaxCategoryId, value);
            }
        }

        private string _Name;
        [StringLength(128)]
        [DataMember]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetValue(ref _Name, () => this.Name, value);
            }
        }
    }
}

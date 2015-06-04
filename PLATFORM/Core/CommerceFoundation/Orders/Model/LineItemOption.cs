using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model
{
    /// <summary>
    /// Contains information about line item options that has been selected.
    /// </summary>
    [DataContract]
    [EntitySet("LineItemOptions")]
    [DataServiceKey("LineItemOptionId")]
    public class LineItemOption : StorageEntity
    {
        public LineItemOption()
        {
            LineItemOptionId = GenerateNewKey();
        }

        private string _LineItemOptionId;
        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string LineItemOptionId
        {
            get
            {
                return _LineItemOptionId;
            }
            set
            {
                SetValue(ref _LineItemOptionId, () => this.LineItemOptionId, value);
            }
        }

        private string _LineItemId;
        [StringLength(128)]
        [DataMember]
        public string LineItemId
        {
            get
            {
                return _LineItemId;
            }
            set
            {
                SetValue(ref _LineItemId, () => this.LineItemId, value);
            }
        }

        private string _OptionName;
        [StringLength(64)]
        [Required]
        [DataMember]
        public string OptionName
        {
            get
            {
                return _OptionName;
            }
            set
            {
                SetValue(ref _OptionName, () => this.OptionName, value);
            }
        }

        private string _OptionValue;
        [StringLength(1024)]
        [DataMember]
        public string OptionValue
        {
            get
            {
                return _OptionValue;
            }
            set
            {
                SetValue(ref _OptionValue, () => this.OptionValue, value);
            }
        }

        [DataMember]
        [ForeignKey("LineItemId")]
        [Parent]
        public LineItem LineItem { get; set; }
    }
}

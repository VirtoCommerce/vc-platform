using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model.Countries
{
    [DataContract]
    [DataServiceKey("RegionId")]
    [EntitySet("Regions")]
    public class Region : StorageEntity
    {
        public Region()
        {
            RegionId = GenerateNewKey();
        }

        private string _RegionId;
        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string RegionId
        {
            get
            {
                return _RegionId;
            }
            set
            {
                SetValue(ref _RegionId, () => this.RegionId, value);
            }
        }

        private string _CountryId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string CountryId
        {
            get
            {
                return _CountryId;
            }
            set
            {
                SetValue(ref _CountryId, () => this.CountryId, value);
            }
        }

        private bool _IsVisible;
        [DataMember]
        public bool IsVisible
        {
            get
            {
                return _IsVisible;
            }
            set
            {
                SetValue(ref _IsVisible, () => this.IsVisible, value);
            }
        }

        private int _Priority;
        [DataMember]
        public int Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                SetValue(ref _Priority, () => this.Priority, value);
            }
        }

        private string _Name;
        [DataMember]
        [StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
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

        private string _DisplayName;
        [DataMember]
        [StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
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

        [DataMember]
        [ForeignKey("CountryId")]
        [Parent]
        public Country Country { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Orders.Model.Jurisdiction
{
    [DataContract]
    [EntitySet("Jurisdictions")]
    [DataServiceKey("JurisdictionId")]
    public class Jurisdiction : StorageEntity
    {
        public Jurisdiction()
        {
            JurisdictionId = GenerateNewKey();
        }

        private string _JurisdictionId;
        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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

        private string _DisplayName;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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

        private string _StateProvinceCode;
        [DataMember]
        [StringLength(32)]
        public string StateProvinceCode
        {
            get
            {
                return _StateProvinceCode;
            }
            set
            {
                SetValue(ref _StateProvinceCode, () => this.StateProvinceCode, value);
            }
        }

        private string _CountryCode;
        [DataMember]
        [StringLength(64)]
        [Required]
        public string CountryCode
        {
            get
            {
                return _CountryCode;
            }
            set
            {
                SetValue(ref _CountryCode, () => this.CountryCode, value);
            }
        }

        private int _JurisdictionType;
        /// <summary>
        /// Gets or sets the type of the jurisdiction. Jurisdictions are used for both taxes and shipping. These allows to distinguish one from the other.
        /// </summary>
        /// <value>
        /// The type of the jurisdiction.
        /// </value>
        [DataMember]
        public int JurisdictionType
        {
            get
            {
                return _JurisdictionType;
            }
            set
            {
                SetValue(ref _JurisdictionType, () => this.JurisdictionType, value);
            }
        }

        /** add these fields
         * 
	[ZipPostalCodeStart] [nvarchar](50) NULL,
	[ZipPostalCodeEnd] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[District] [nvarchar](50) NULL,
	[County] [nvarchar](50) NULL,
	[GeoCode] [nvarchar](255) NULL,
	[Code] [nvarchar](50) NOT NULL

         ** */

        private string _ZipPostalCodeStart;
        [DataMember]
        [StringLength(64)]
        public string ZipPostalCodeStart
        {
            get
            {
                return _ZipPostalCodeStart;
            }
            set
            {
                SetValue(ref _ZipPostalCodeStart, () => this.ZipPostalCodeStart, value);
            }
        }

        private string _ZipPostalCodeEnd;
        [DataMember]
        [StringLength(64)]
        public string ZipPostalCodeEnd
        {
            get
            {
                return _ZipPostalCodeEnd;
            }
            set
            {
                SetValue(ref _ZipPostalCodeEnd, () => this.ZipPostalCodeEnd, value);
            }
        }

        private string _City;
        [DataMember]
        [StringLength(64)]
        public string City
        {
            get
            {
                return _City;
            }
            set
            {
                SetValue(ref _City, () => this.City, value);
            }
        }

        private string _District;
        [DataMember]
        [StringLength(64)]
        public string District
        {
            get
            {
                return _District;
            }
            set
            {
                SetValue(ref _District, () => this.District, value);
            }
        }

        private string _County;
        [DataMember]
        [StringLength(64)]
        public string County
        {
            get
            {
                return _County;
            }
            set
            {
                SetValue(ref _County, () => this.County, value);
            }
        }

        private string _GeoCode;
        [DataMember]
        [StringLength(256)]
        public string GeoCode
        {
            get
            {
                return _GeoCode;
            }
            set
            {
                SetValue(ref _GeoCode, () => this.GeoCode, value);
            }
        }

        private string _Code;
        [DataMember]
        [StringLength(64)]
        [Required]
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                SetValue(ref _Code, () => this.Code, value);
            }
        }
    }
}

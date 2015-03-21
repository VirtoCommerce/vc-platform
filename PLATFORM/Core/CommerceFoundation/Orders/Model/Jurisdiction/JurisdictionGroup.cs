using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;

namespace VirtoCommerce.Foundation.Orders.Model.Jurisdiction
{
    [DataContract]
    [EntitySet("JurisdictionGroups")]
    [DataServiceKey("JurisdictionGroupId")]
    public class JurisdictionGroup : StorageEntity
    {
        public JurisdictionGroup()
        {
            JurisdictionGroupId = GenerateNewKey();
        }

        private string _JurisdictionGroupId;
        [DataMember]
        [Key]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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

        /* addd these fields
    [DisplayName] [nvarchar](250) NOT NULL,
    [JurisdictionType] [int] NOT NULL,
    [Code] [nvarchar](50) NOT NULL         
         */

        private string _DisplayName;
        [DataMember]
        [StringLength(256)]
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

        private int _JurisdictionType;
        [DataMember]
        [Required]
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

        #region NavigationProperties
        
        private ObservableCollection<JurisdictionRelation> _JurisdictionRelations;
        [DataMember]
        public ObservableCollection<JurisdictionRelation> JurisdictionRelations
        {
            get
            {
                if (_JurisdictionRelations == null)
                    _JurisdictionRelations = new ObservableCollection<JurisdictionRelation>();

                return _JurisdictionRelations;
            }
        }


        private ObservableCollection<ShippingMethodJurisdictionGroup> _shippingMethodJurisdictionGroups =
            null;
        [DataMember]
        public virtual ObservableCollection<ShippingMethodJurisdictionGroup> ShippingMethodJurisdictionGroups
        {
            get
            {
                if (_shippingMethodJurisdictionGroups == null)
                {
                    _shippingMethodJurisdictionGroups=new ObservableCollection<ShippingMethodJurisdictionGroup>();
                }
                return _shippingMethodJurisdictionGroups;
            }
        }

        #endregion
    }
}

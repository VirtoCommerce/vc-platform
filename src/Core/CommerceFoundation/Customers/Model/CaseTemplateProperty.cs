using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Collections.ObjectModel;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Customers.Model
{
    [DataContract]
    [EntitySet("CaseTemplateProperties")]
    [DataServiceKey("CaseTemplatePropertyId")]
    public class CaseTemplateProperty : StorageEntity
    {
        public CaseTemplateProperty()
        {
            _CaseTemplatePropertyId = GenerateNewKey();
        }
        
        private string _CaseTemplatePropertyId;
        [Key]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        [DataMember]
        public string CaseTemplatePropertyId
        {
            get
            {
                return _CaseTemplatePropertyId;
            }
            set
            {
                SetValue(ref _CaseTemplatePropertyId, () => this.CaseTemplatePropertyId, value);
            }
        }

        private string _Name;
        [Required(ErrorMessage = "Name is required")]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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

        private int _ValueType;
        [Required]
        [DataMember]
        public int ValueType
        {
            get
            {
                return _ValueType;
            }
            set
            {
                SetValue(ref _ValueType, () => this.ValueType, value);
            }
        }

        #region NavigationProperties

        private string _CaseTemplateId;
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string CaseTemplateId
        {
            get { return _CaseTemplateId; }
            set
            {
                SetValue(ref _CaseTemplateId, () => this.CaseTemplateId, value);
            }
        }

        [DataMember]
        [ForeignKey("CaseTemplateId")]
        [Parent]
        public virtual CaseTemplate CaseTemplate { get; set; }
        
        #endregion
    }
}

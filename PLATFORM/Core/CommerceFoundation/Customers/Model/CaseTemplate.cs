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

namespace VirtoCommerce.Foundation.Customers.Model
{
    [DataContract]
    [EntitySet("CaseTemplates")]
    [DataServiceKey("CaseTemplateId")]
    public class CaseTemplate : StorageEntity
    {

        public CaseTemplate()
        {
            _CaseTemplateId = GenerateNewKey();
        }

        private string _CaseTemplateId;
        [Key]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        [DataMember]
        public string CaseTemplateId
        {
            get
            {
                return _CaseTemplateId;
            }
            set
            {
                SetValue(ref _CaseTemplateId, () => this.CaseTemplateId, value);
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

        private bool _IsActive;
        [DataMember]
        [Required]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                SetValue(ref _IsActive, () => this.IsActive, value);
            }
        }

        private string _description;
        [DataMember]
        [StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
        public string Description
        {
            get { return _description; }
            set { SetValue(ref _description, () => Description, value); }
        }

        #region Navigation Properties

        ObservableCollection<CaseTemplateProperty> _CaseTemplateProperties = null;
        [DataMember]
        public virtual ObservableCollection<CaseTemplateProperty> CaseTemplateProperties
        {
            get
            {
                if (_CaseTemplateProperties == null)
                    _CaseTemplateProperties = new ObservableCollection<CaseTemplateProperty>();

                return _CaseTemplateProperties;
            }
        }
        
        #endregion
    }
}

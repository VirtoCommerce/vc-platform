using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

using System.Data.Services.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Customers.Model
{
    [DataContract]
    [EntitySet("Cases")]
    [DataServiceKey("CaseId")]
    public class Case : StorageEntity
    {

        public Case()
        {
            _CaseId = GenerateNewKey();
        }


        private string _CaseId;
        [Key]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [DataMember]
        public string CaseId
        {
            get
            {
                return _CaseId;
            }
            set
            {
                SetValue(ref _CaseId, () => this.CaseId, value);
            }
        }


        private string _Number;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string Number
        {
            get
            {
                return _Number;
            }
            set
            {
                SetValue(ref _Number, () => this.Number, value);
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


        private string _AgentId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string AgentId
        {
            get
            {
                return _AgentId;
            }
            set
            {
                SetValue(ref _AgentId, () => this.AgentId, value);
            }
        }


        private string _AgentName;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string AgentName
        {
            get
            {
                return _AgentName;
            }
            set
            {
                SetValue(ref _AgentName, () => this.AgentName, value);
            }
        }


        private string _Title;
        [DataMember]
        [StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                SetValue(ref _Title, () => this.Title, value);
            }
        }
        
        private string _Description;
        [DataMember]
        [StringLength(512, ErrorMessage = "Only 512 characters allowed.")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                SetValue(ref _Description, () => this.Description, value);
            }
        }


        private string _Status;
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                SetValue(ref _Status, () => this.Status, value);
            }
        }


        private string _Channel;
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string Channel
        {
            get
            {
                return _Channel;
            }
            set
            {
                SetValue(ref _Channel, () => this.Channel, value);
            }
        }

        private string _ContactDisplayName;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ContactDisplayName
        {
            get { return _ContactDisplayName; }
            set
            {
                SetValue(ref _ContactDisplayName, () => this.ContactDisplayName, value);
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
        public virtual CaseTemplate CaseTemplate { get; set; }
        
        private string _ContactId;

        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ContactId
        {
            get { return _ContactId; }
            set
            {
                SetValue(ref _ContactId, () => this.ContactId, value);
            }
        }

        [DataMember]
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }

        private ObservableCollection<Note> _Notes = null;
        [DataMember]
        public ObservableCollection<Note> Notes
        {
            get
            {
                if (_Notes == null)
                    _Notes = new ObservableCollection<Note>();

                return _Notes;
            }
        }


        private ObservableCollection<Label> _Labels = null;
        [DataMember]
        public ObservableCollection<Label> Labels
        {
            get
            {
                if (_Labels == null)
                    _Labels = new ObservableCollection<Label>();

                return _Labels;
            }
        }


        private ObservableCollection<CommunicationItem> _CommunicationItems = null;
        [DataMember]
        public ObservableCollection<CommunicationItem> CommunicationItems
        {
            get
            {
                if (_CommunicationItems == null)
                    _CommunicationItems = new ObservableCollection<CommunicationItem>();

                return _CommunicationItems;
            }
        }


        private ObservableCollection<CaseCC> _caseCcs;
        [DataMember]
        public ObservableCollection<CaseCC> CaseCc
        {
            get
            {
                if (_caseCcs == null)
                {
                    _caseCcs=new ObservableCollection<CaseCC>();
                }
                return _caseCcs;
            }
        }

        private ObservableCollection<CasePropertyValue> _casePropertyValues;
        [DataMember]
        public ObservableCollection<CasePropertyValue> CasePropertyValues
        {
            get
            {
                if (_casePropertyValues == null)
                {
                    _casePropertyValues = new ObservableCollection<CasePropertyValue>();
                }
                return _casePropertyValues;
            }
        }

        #endregion


    }
}

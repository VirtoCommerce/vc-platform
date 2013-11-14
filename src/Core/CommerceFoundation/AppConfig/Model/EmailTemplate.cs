using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.AppConfig.Model
{


    [DataContract]
    [EntitySet("EmailTemplates")]
    [DataServiceKey("EmailTemplateId")]
    public class EmailTemplate:StorageEntity 
    {
        public EmailTemplate()
        {
            EmailTemplateId = GenerateNewKey();
        }


        private string _emailTemplateId;
        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string EmailTemplateId
        {
            get { return _emailTemplateId; }
            set
            {
                SetValue(ref _emailTemplateId, () => EmailTemplateId, value);
            }
        }


        private string _name;
        [DataMember]
        [Required]
        [StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
        public string Name
        {
            get { return _name; }
            set
            {
                SetValue(ref _name,()=>Name,value);
            }
        }


        private string _type;
        [DataMember]
        [StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
        public string Type
        {
            get { return _type; }
            set
            {
                SetValue(ref _type,()=>Type,value);
            }
        }



        private string _body;
        [DataMember]
        [Required]
        public string Body
        {
            get { return _body; }
            set
            {
                SetValue(ref _body,()=>Body,value);
            }
        }

        private string _defaultLanguageCode;
        [DataMember]
        [Required]
        public string DefaultLanguageCode
        {
            get { return _defaultLanguageCode; }
            set
            {
                SetValue(ref _defaultLanguageCode,()=>DefaultLanguageCode,value);
            }
        }


        private string _subject;
        [DataMember]
        [StringLength(128)]
        public string Subject
        {
            get { return _subject; }
            set { SetValue(ref _subject, () => Subject, value); }
        }


        #region NavigationProperties

        private ObservableCollection<EmailTemplateLanguage> _emailTemplateLanguages = null;
        [DataMember]
        public virtual ObservableCollection<EmailTemplateLanguage> EmailTemplateLanguages
        {
            get
            {
                if (_emailTemplateLanguages == null)
                {
                    _emailTemplateLanguages = new ObservableCollection<EmailTemplateLanguage>();
                }
                return _emailTemplateLanguages;
            }
        }

        #endregion

    }
}

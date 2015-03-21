using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
    [DataContract]
    [EntitySet("EmailTemplateLanguages")]
    [DataServiceKey("EmailTemplateLanguageId")]
    public class EmailTemplateLanguage : StorageEntity
    {
        public EmailTemplateLanguage()
        {
            EmailTemplateLanguageId = GenerateNewKey();
        }


        private string _emailTemplateLanguageId;
        [Key]
        [StringLength(128)]
        [DataMember]
        public string EmailTemplateLanguageId
        {
            get { return _emailTemplateLanguageId; }
            set
            {
                SetValue(ref _emailTemplateLanguageId, () => EmailTemplateLanguageId, value);
            }
        }


        private string _languageCode;
        [StringLength(32)]
        [DataMember]
        [Required]
        public string LanguageCode
        {
            get { return _languageCode; }
            set
            {
                SetValue(ref _languageCode, () => LanguageCode, value);
            }
        }


        private string _subject;
        [StringLength(128)]
        [DataMember]
        [Required]
        public string Subject
        {
            get { return _subject; }
            set
            {
                SetValue(ref _subject, () => Subject, value);
            }
        }


        private string _body;
        [DataMember]
        public string Body
        {
            get { return _body; }
            set { SetValue(ref _body, () => Body, value); }
        }

        private int _priority;

        [DataMember]
        public int Priority
        {
            get { return _priority; }
            set { SetValue(ref _priority, () => Priority, value); }
        }


        #region NavigationProperties

        private string _emailTemplateId;
        [DataMember]
        [StringLength(128)]
        public string EmailTemplateId
        {
            get { return _emailTemplateId; }
            set
            {
                SetValue(ref _emailTemplateId, () => EmailTemplateId, value);
            }
        }
        [DataMember]
        [ForeignKey("EmailTemplateId")]
        [Parent]
        public virtual EmailTemplate EmailTemplate { get; set; }

        #endregion

    }
}

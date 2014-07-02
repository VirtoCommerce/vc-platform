using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks.Email;
using VirtoCommerce.Foundation.Frameworks.Events;
using VirtoCommerce.Foundation.Frameworks.Templates;
using VirtoCommerce.Web.Client.Services.Emails;

namespace VirtoCommerce.Web.Client.Services.Listeners
{
    /// <summary>
    /// Class PublicReplyEventListener.
    /// </summary>
    public class PublicReplyEventListener : ChangeEntityEventListener<PublicReplyItem>
    {
        /// <summary>
        /// The _app configuration repository
        /// </summary>
        private readonly IAppConfigRepository _appConfigRepository;
        /// <summary>
        /// The _customer repository
        /// </summary>
        private readonly ICustomerRepository _customerRepository;
        /// <summary>
        /// The _email service
        /// </summary>
        private readonly IEmailService _emailService;
        /// <summary>
        /// The _template service
        /// </summary>
        private readonly ITemplateService _templateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicReplyEventListener"/> class.
        /// </summary>
        /// <param name="emailService">The email service.</param>
        /// <param name="templateService">The template service.</param>
        /// <param name="customerRepository">The customer repository.</param>
        /// <param name="appConfigRepository">The application configuration repository.</param>
        public PublicReplyEventListener(IEmailService emailService, ITemplateService templateService,
                                        ICustomerRepository customerRepository, IAppConfigRepository appConfigRepository)
        {
            _emailService = emailService;
            _templateService = templateService;
            _customerRepository = customerRepository;
            _appConfigRepository = appConfigRepository;
        }

        /// <summary>
        /// Called when [after insert].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="e">The <see cref="EntityEventArgs"/> instance containing the event data.</param>
        public override void OnAfterInsert(PublicReplyItem item, EntityEventArgs e)
        {
            SendNewPublicReply(item);
        }

        /// <summary>
        /// Sends the new public reply.
        /// </summary>
        /// <param name="item">The item.</param>
        private void SendNewPublicReply(PublicReplyItem item)
        {
            //Create a context object
            IDictionary<string, object> context = new Dictionary<string, object>();
            context.Add("publicReply", item);

            var lang = Helpers.StoreHelper.CustomerSession.Language;
            lang = string.IsNullOrWhiteSpace(lang) ? "en-us" : lang;

            //Send case-created-notification email
            var template = _templateService.ProcessTemplate("case-public-reply", context, new CultureInfo(lang));

            if (template != null)
            {
                var userMail = _customerRepository.Emails.FirstOrDefault(e => e.MemberId == item.Case.ContactId);
                var agentMail = _customerRepository.Emails.FirstOrDefault(e => e.MemberId == item.Case.AgentId);

                var toMails = new List<string>();
                if (userMail != null)
                {
                    toMails.Add(userMail.Address);
                }
                if (agentMail != null)
                {
                    toMails.Add(agentMail.Address);
                }

                SendEmail(template, toMails.ToArray());
            }
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="recipients">The recipients.</param>
        private void SendEmail(IProcessedTemplate template, params string[] recipients)
        {
            if (recipients == null || template == null || string.IsNullOrEmpty(template.Body))
            {
                return;
            }
            var isHtml = template.Type != EmailTemplateTypes.Text;
            IEmailMessage message = new EmailMessage(recipients, template.Body, isHtml);
            message.Subject = template.Subject;
            var emailFromSetting = _appConfigRepository.Settings.FirstOrDefault(s => s.Name.Equals("DefaultEmailFrom"));
            if (emailFromSetting != null)
            {
                var emailFrom = emailFromSetting.SettingValues.FirstOrDefault();
                if (emailFrom != null)
                {
                    message.From = emailFrom.ToString();
                }
            }

            try
            {
                _emailService.SendEmail(message);
            }
            catch
            {
                //Log
                return;
            }
        }
    }
}
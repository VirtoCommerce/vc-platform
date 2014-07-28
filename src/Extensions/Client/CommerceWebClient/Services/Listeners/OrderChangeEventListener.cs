using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Frameworks.Email;
using VirtoCommerce.Foundation.Frameworks.Events;
using VirtoCommerce.Foundation.Frameworks.Templates;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Stores.Services;
using VirtoCommerce.Web.Client.Services.Emails;

namespace VirtoCommerce.Web.Client.Services.Listeners
{
    /// <summary>
    /// Class OrderChangeEventListener.
    /// </summary>
    public class OrderChangeEventListener : ChangeEntityEventListener<Order>
    {
        /// <summary>
        /// The _email service
        /// </summary>
        private readonly IEmailService _emailService;
        /// <summary>
        /// The _store service
        /// </summary>
        private readonly IStoreService _storeService;
        /// <summary>
        /// The _template service
        /// </summary>
        private readonly ITemplateService _templateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderChangeEventListener"/> class.
        /// </summary>
        /// <param name="emailService">The email service.</param>
        /// <param name="templateService">The template service.</param>
        /// <param name="storeService">The store service.</param>
        public OrderChangeEventListener(IEmailService emailService, ITemplateService templateService,
                                        IStoreService storeService)
        {
            _emailService = emailService;
            _templateService = templateService;
            _storeService = storeService;
        }

        /// <summary>
        /// Called when [after insert].
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="e">The <see cref="EntityEventArgs"/> instance containing the event data.</param>
        public override void OnAfterInsert(Order order, EntityEventArgs e)
        {
            SendNewOrderNotificationsAsync(order);
        }

        /// <summary>
        /// Sends the new order notifications asynchronous.
        /// </summary>
        /// <param name="order">The order.</param>
        private void SendNewOrderNotificationsAsync(Order order)
        {
            // the whole function should be async
            //TODO: cannot use async here because of PerRequestLifetimeManager for EF repositories
            //Task.Run(() =>
            //    {
                    //Create a context object
                    IDictionary<string, object> context = new Dictionary<string, object>();
                    context.Add("order", order);

                    var lang = Helpers.StoreHelper.CustomerSession.Language;
                    lang = string.IsNullOrWhiteSpace(lang) ? "en-us" : lang;

                    //Send order-confirmation email
                    var confirmTempate = _templateService.ProcessTemplate("order-confirmation", context,
                                                                          new CultureInfo(lang));

                    var recipientAddress =
                        order.OrderAddresses.FirstOrDefault(oa => oa.OrderAddressId == order.AddressId);
                    if (recipientAddress != null)
                    {
                        SendEmail(confirmTempate, recipientAddress.Email);
                    }

                    //Send order-notification email
                    var notifyTemplate = _templateService.ProcessTemplate("order-notify", context,
                                                                          new CultureInfo(lang));

                    if (_storeService != null)
                    {
                        var store = _storeService.Stores.FirstOrDefault(s => s.StoreId == order.StoreId);

                        if (store != null)
                        {
                            SendEmail(notifyTemplate, store.Email);
                        }
                    }
               // });
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="recipient">The recipient.</param>
        private void SendEmail(IProcessedTemplate template, string recipient)
        {
            if (string.IsNullOrEmpty(recipient) || template == null || string.IsNullOrEmpty(template.Body))
            {
                return;
            }
            var isHtml = template.Type != EmailTemplateTypes.Text;
            IEmailMessage message = new EmailMessage(recipient, template.Body, isHtml);
            message.Subject = template.Subject;
            var eMessage = new MailMessage();
            var smtpDefault = eMessage.From.ToString();
            message.From = string.IsNullOrWhiteSpace(smtpDefault) ? "orders@virtoway.com" : smtpDefault;

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
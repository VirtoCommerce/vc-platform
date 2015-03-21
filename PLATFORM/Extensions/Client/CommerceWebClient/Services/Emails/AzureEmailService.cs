using SendGridMail;
using SendGridMail.Transport;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using VirtoCommerce.Foundation.Frameworks.Email;

namespace VirtoCommerce.Web.Client.Services.Emails
{
    using System.Threading.Tasks;

    /// <summary>
    /// Class AzureEmailService.
    /// </summary>
    public class AzureEmailService : IEmailService
    {
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns><c>true</c> if email sent succesfully, <c>false</c> otherwise.</returns>
        public bool SendEmail(IEmailMessage message)
        {
            if (message == null)
            {
                return false;
            }

            //Smtp settings ?? is it OK to read setting directly here from config?
            var smtp = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            // Create the email object first, then add the properties.
            var eMessage = SendGrid.GetInstance();

            eMessage.From = !string.IsNullOrEmpty(message.From) ? new MailAddress(message.From) : new MailAddress(smtp.From);

            // Add multiple addresses to the To field.
            eMessage.AddTo(message.To);

            eMessage.Subject = message.Subject;

            //Add the HTML and Text bodies
            eMessage.Html = message.Html;
            eMessage.Text = message.Text;

            //Add attachments
            if (message.Attachments != null && message.Attachments.Count > 0)
            {
                foreach (var attachment in message.Attachments)
                {
                    try
                    {
                        using (var stream = new MemoryStream(attachment.Length))
                        {
                            stream.Write(attachment, 0, attachment.Length);
                            stream.Position = 0;
                            eMessage.AddAttachment(stream, "file_" + message.Attachments.IndexOf(attachment));
                        }

                    }
                    catch { return false; }
                }
            }

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential(smtp.Network.UserName, smtp.Network.Password);

            // Create an SMTP transport for sending email.
            var transportSmtp = SMTP.GetInstance(credentials, smtp.Network.Host, smtp.Network.Port);

            // Send the email.
            try
            {
                //Task.Run(() => transportSMTP.Deliver(eMessage));
                transportSmtp.Deliver(eMessage);
            }
            catch
            {

                return false;
            }

            return true;
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using VirtoCommerce.Foundation.Frameworks.Email;

namespace VirtoCommerce.Web.Client.Services.Emails
{
    using System.Threading.Tasks;

    public class NetEmailService : IEmailService
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

            var eMessage = new MailMessage();

            if (!string.IsNullOrEmpty(message.From))
            {
                eMessage.From = new MailAddress(message.From);
            }

            foreach (var to in message.To.Where(to => !string.IsNullOrEmpty(to)))
            {
                eMessage.To.Add(new MailAddress(to));
            }

            eMessage.Subject = message.Subject;

            if (!string.IsNullOrEmpty(message.Html))
            {
                eMessage.Body = message.Html;
                eMessage.IsBodyHtml = true;
            }
            else if (!string.IsNullOrEmpty(message.Text))
            {
                eMessage.Body = message.Text;
            }

            //Add attachments
            var openedStream = new List<Stream>();
            if (message.Attachments != null & message.Attachments.Count > 0)
            {
                foreach (var attachment in message.Attachments)
                {
                    try
                    {
                        var stream = new MemoryStream(attachment.Length);
                        stream.Write(attachment, 0, attachment.Length);
                        stream.Position = 0;
                        var eAttachment = new Attachment(stream, (string)null);
                        eMessage.Attachments.Add(eAttachment);
                        openedStream.Add(stream);

                    }
                    catch { return false; }
                }
            }

            var client = new SmtpClient();

	        try
	        {
                //Task.Run(() => client.Send(eMessage));
	            client.Send(eMessage);
	        }
	        catch
	        {
	            return false;
	        }
            finally
            {
                //Must close streams here otherwise client.Send will fail with exception
                openedStream.ForEach(s => s.Dispose());
            }

            return true;
        }
    }
}

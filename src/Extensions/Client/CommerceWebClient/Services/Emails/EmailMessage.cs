using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks.Email;

namespace VirtoCommerce.Web.Client.Services.Emails
{
    /// <summary>
    /// Class EmailMessage.
    /// </summary>
    public class EmailMessage : IEmailMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        public EmailMessage()
        {
            To = new List<string>();
            Attachments = new List<byte[]>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="message">The message.</param>
        public EmailMessage(string to, string message)
            : this(to, message, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="message">The message.</param>
        /// <param name="isHtml">if set to <c>true</c> [is HTML].</param>
        public EmailMessage(string to, string message, bool isHtml)
            : this(new []{to}, message, isHtml)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="message">The message.</param>
        /// <param name="isHtml">if set to <c>true</c> [is HTML].</param>
        public EmailMessage(IEnumerable<string> to, string message, bool isHtml): this()
        {
            To.AddRange(to);

            if (isHtml)
            {
                Html = message;
            }
            else
            {
                Text = message;
            }
        }

        /// <summary>
        /// List of email recipient addresses
        /// </summary>
        /// <value>To.</value>
        public List<string> To { get; private set; }

        /// <summary>
        /// Address from where the email is sent
        /// </summary>
        /// <value>From.</value>
        public string From
        {
            get;
            set;
        }

        /// <summary>
        /// Html body of emai message
        /// </summary>
        /// <value>The HTML.</value>
        public string Html
        {
            get;
            set;
        }

        /// <summary>
        /// Text body of email message
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// The subject of the email message
        /// </summary>
        /// <value>The subject.</value>
        public string Subject
        {
            get;
            set;
        }

        /// <summary>
        /// Additional attachemts that can be sent by email
        /// </summary>
        /// <value>The attachments.</value>
        public List<byte[]> Attachments { get; private set; }
    }
}

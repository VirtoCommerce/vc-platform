using System.Collections.Generic;

namespace VirtoCommerce.Foundation.Frameworks.Email
{
    public interface IEmailMessage
    {
		/// <summary>
		/// List of email recipient addresses
		/// </summary>
        List<string> To { get; }
		/// <summary>
		/// Address from where the email is sent
		/// </summary>
        string From { get; set; }
		/// <summary>
		/// Html body of emai message
		/// </summary>
        string Html { get; set; }
		/// <summary>
		/// Text body of email message
		/// </summary>
        string Text { get; set; }
		/// <summary>
		/// The subject of the email message
		/// </summary>
        string Subject { get; set; }
		/// <summary>
		/// Additional attachemts that can be sent by email
		/// </summary>
        List<byte[]> Attachments { get; }
    }
}

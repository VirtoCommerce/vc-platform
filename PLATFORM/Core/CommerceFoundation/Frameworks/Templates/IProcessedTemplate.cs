using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.Foundation.Frameworks.Templates
{
    public interface IProcessedTemplate
    {
		/// <summary>
		/// The processed body of template for sending email
		/// </summary>
        string Body { get; set; }
		/// <summary>
		/// Template subject
		/// </summary>
        string Subject { get; set; }
		/// <summary>
		/// The type of processed template
		/// </summary>
        EmailTemplateTypes Type { get; set; }
    }
}

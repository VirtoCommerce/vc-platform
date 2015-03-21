using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Frameworks.Templates;

namespace VirtoCommerce.Web.Client.Services.Templates
{
    /// <summary>
    /// Class ProcessedTemplate.
    /// </summary>
    public class ProcessedTemplate : IProcessedTemplate
    {
        /// <summary>
        /// The processed body of template for sending email
        /// </summary>
        /// <value>The body.</value>
        public string Body
        {
            get;
            set;
        }

        /// <summary>
        /// Template subject
        /// </summary>
        /// <value>The subject.</value>
        public string Subject
        {
            get;
            set;
        }

        /// <summary>
        /// The type of processed template
        /// </summary>
        /// <value>The type.</value>
        public EmailTemplateTypes Type
        {
            get;
            set;
        }
    }
}

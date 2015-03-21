using System.Collections.Generic;
using System.Globalization;

namespace VirtoCommerce.Foundation.Frameworks.Templates
{
    public interface ITemplateService
    {
        IProcessedTemplate ProcessTemplate(string templateName, IDictionary<string, object> context, CultureInfo culture);
    }
}

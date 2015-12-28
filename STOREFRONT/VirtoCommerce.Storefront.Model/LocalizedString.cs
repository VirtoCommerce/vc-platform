using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model
{
    public class LocalizedString
    {
        public LocalizedString()
        {
            Language = Language.InvariantLanguage;
            Value = null;
        }
        public LocalizedString(Language language, string value)
        {
            Language = language;
            Value = value;

        }
        public Language Language { get; set; }
        public string Value { get; set; }
    }
}

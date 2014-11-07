using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{

    public class CatalogLanguage
    {
        public CatalogLanguage()
        {        
        }

        public CatalogLanguage(string languageCode)
        {
            LanguageCode = languageCode;
        }
        public string CatalogId { get; set; }
        public bool IsDefault { get; set; }

        public string LanguageCode { get; set; }

        public string DisplayName
        {
            get
            {
                var retVal = LanguageCode;

                try
                {
                    var culture = CultureInfo.CreateSpecificCulture(LanguageCode);
                    return culture.DisplayName;
                }
                catch
                {
                    return retVal;
                }
            }
        }
    }
}

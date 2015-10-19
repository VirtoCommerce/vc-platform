using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Models
{
    interface IWorkContext
    {
        string GetStoreUrl(bool useSsl);
        string WorkingLanguage { get; set; }
        string[] StoreLanguages { get; set; }
    }
}

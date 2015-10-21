using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Main working context contains all data which could be used in presentation logic
    /// </summary>
    public class WorkContext 
    {
        public WorkContext()
        {
            AllStores = new List<Store>();
        }
        /// <summary>
        /// Current customer
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// Language culture name format (en-US etc)
        /// </summary>
        public string CurrentLanguage { get; set; }
        /// <summary>
        /// Currency code in ISO 4217 format (USD etc)
        /// </summary>
        public string CurrentCurrency { get; set; }

        public Store CurrentStore { get; set; }

        /// <summary>
        /// List of all supported stores
        /// </summary>
        public ICollection<Store> AllStores { get; set; }
    }
}

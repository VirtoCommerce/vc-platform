using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class EditorialReview : Entity
    {
        /// <summary>
        /// Editorial review content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Editorial review type
        /// </summary>
        public string ReviewType { get; set; }

        /// <summary>
        /// Editorial review language
        /// </summary>
        public string LanguageCode { get; set; }
    }
}

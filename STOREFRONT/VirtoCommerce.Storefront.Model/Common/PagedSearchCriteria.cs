using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common
{
    public class PagedSearchCriteria
    {
        public PagedSearchCriteria(NameValueCollection queryString)
        {
            PageNumber = Convert.ToInt32(queryString.Get("page") ?? 1.ToString());
            PageSize = Convert.ToInt32(queryString.Get("count") ?? 10.ToString());
        }
        public int Start
        {
            get
            {
                return (PageNumber - 1) * PageSize;
            }
        }
                
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)


                hash = hash * 59 + this.PageNumber.GetHashCode();
                hash = hash * 59 + this.PageSize.GetHashCode();


                return hash;
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.Domain.Customer.Model
{
    public class Vendor : Member, ISeoSupport
    {
        /// <summary>
        /// Vendor description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Vendor site url
        /// </summary>
        public string SiteUrl { get; set; }
        /// <summary>
        /// Vendor logo url
        /// </summary>
        public string LogoUrl { get; set; }
        /// <summary>
        /// Vendor group
        /// </summary>
        public string GroupName { get; set; }

        #region MyRegion
        public string SeoObjectType { get { return GetType().Name; } }
        public ICollection<SeoInfo> SeoInfos { get; set; }
        #endregion
    }
}

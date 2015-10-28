using System;
using System.Globalization;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Main working context contains all data which could be used in presentation logic
    /// </summary>
    public class WorkContext : IDisposable
    {
        /// <summary>
        /// Current customer
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// Language culture name format (e.g. en-US)
        /// </summary>
        public string CurrentLanguage { get; set; }
        /// <summary>
        /// Currency code in ISO 4217 format (e.g. USD)
        /// </summary>
        public string CurrentCurrency { get; set; }

        public CultureInfo CurrentCulture
        {
            get
            {
                var retVal = CultureInfo.CurrentCulture;
                if(CurrentLanguage != null)
                {
                    retVal = CultureInfo.GetCultureInfo(CurrentLanguage);
                }
                return retVal;
            }

        }

        public RegionInfo CurrentRegionInfo
        {
            get
            {
                return new RegionInfo(CurrentCulture.Name);
            }

        }

        public Store CurrentStore { get; set; }

        /// <summary>
        /// List of all supported stores
        /// </summary>
        public Store[] AllStores { get; set; }

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        #endregion
    }
}

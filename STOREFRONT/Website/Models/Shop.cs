#region
using System.Collections.Generic;
using System.Runtime.Serialization;
using DotLiquid;
using VirtoCommerce.ApiClient.DataContracts.Stores;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Shop : Drop
    {
        #region Public Properties
        public string Currency { get; set; }

        public bool CustomerAccountsEnabled { get; set; }

        public bool CustomerAccountsOptional { get; set; }

        public string DefaultLanguage { get; set; }

        public string Description { get; set; }

        public string Domain { get; set; }

        public string Email { get; set; }

        public string[] EnabledPaymentTypes { get; set; }

        public IEnumerable<SeoKeyword> Keywords { get; set; }

        public string[] Languages { get; set; }

        public string MoneyFormat { get; set; }

        public string Name { get; set; }

        public string SecureUrl { get; set; }

        public StoreState State { get; set; }

        public string StoreId { get; set; }

        public string Url { get; set; }

        public string Catalog { get; set; }

        public string[] Currencies { get; set; }

        #endregion
    }
}
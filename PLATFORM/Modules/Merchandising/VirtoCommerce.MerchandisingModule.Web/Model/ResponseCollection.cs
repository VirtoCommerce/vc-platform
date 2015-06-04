using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class ResponseCollection<T>
    {
        #region Constructors and Destructors

        public ResponseCollection()
        {
            this.Items = new Collection<T>();
        }

        #endregion

        #region Public Properties

        [JsonProperty("items")]
        public ICollection<T> Items { get; set; }

        [JsonProperty("total")]
        public int TotalCount { get; set; }

        #endregion
    }
}

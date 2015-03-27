#region

using System.Collections.ObjectModel;
using Newtonsoft.Json;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts
{

    #region

    #endregion

    public class ResponseCollection<T>
    {
        #region Fields

        private Collection<T> _items;

        #endregion

        #region Public Properties

        [JsonProperty("items")]
        public Collection<T> Items
        {
            get { return _items ?? (_items = new Collection<T>()); }
        }

        [JsonProperty("total")]
        public int TotalCount { get; set; }

        #endregion
    }
}

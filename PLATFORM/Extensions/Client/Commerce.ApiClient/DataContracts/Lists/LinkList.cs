using System.Collections.Generic;
using Newtonsoft.Json;

namespace VirtoCommerce.ApiClient.DataContracts.Lists
{
    public class LinkList
    {
        #region Public Properties

        public string Id { get; set; }
        public string Language { get; set; }

        [JsonProperty("menulinks")]
        public IEnumerable<Link> Links { get; set; }

        public string Name { get; set; }

        #endregion
    }
}

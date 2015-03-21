using System.Collections.Generic;
using Newtonsoft.Json;

namespace VirtoCommerce.ApiClient.DataContracts
{
    public abstract class Resource
    {
        private List<Link> _links = null;

        [JsonProperty(Order = 100)]
        public IEnumerable<Link> Links { get { return _links; } }

        public void AddLink(Link link)
        {
            if (_links == null)
            {
                _links = new List<Link>();
            }

            _links.Add(link);
        }

        public void AddLinks(params Link[] links)
        {
            foreach (var link in links)
            {
                AddLink(link);
            }
        }
    }
}

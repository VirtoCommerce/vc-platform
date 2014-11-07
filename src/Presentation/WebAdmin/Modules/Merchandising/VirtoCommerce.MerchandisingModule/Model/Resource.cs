using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace VirtoCommerce.MerchandisingModule.Model
{
    public abstract class Resource
    {
        private List<Link> _links = null;

        [JsonProperty(Order = 100)]
        public IEnumerable<Link> Links { get { return _links; } }

        public void AddLink(Link link)
        {
            /*
            if (_links == null)
            {
                _links = new List<Link>();
            }

            _links.Add(link);
             * */
        }

        public void AddLinks(params Link[] links)
        {
            links.ForEach(this.AddLink);
        }
    }
}

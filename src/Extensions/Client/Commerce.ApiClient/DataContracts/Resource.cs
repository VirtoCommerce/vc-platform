#region

using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts
{

    #region

    #endregion

    public abstract class Resource
    {
        #region Fields

        private List<Link> _links = null;

        #endregion

        #region Public Properties

        [JsonProperty(Order = 100)]
        public IEnumerable<Link> Links
        {
            get { return _links; }
        }

        #endregion

        #region Public Methods and Operators

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

        #endregion
    }
}

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
            get { return this._links; }
        }

        #endregion

        #region Public Methods and Operators

        public void AddLink(Link link)
        {
            if (this._links == null)
            {
                this._links = new List<Link>();
            }

            this._links.Add(link);
        }

        public void AddLinks(params Link[] links)
        {
            foreach (var link in links)
            {
                this.AddLink(link);
            }
        }

        #endregion
    }
}

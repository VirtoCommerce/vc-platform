#region

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DotLiquid;
using VirtoCommerce.Web.Models.Services;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Search : Drop
    {
        //private bool _productsLoaded = false;
        #region Constructors and Destructors
        public Search()
        {
            this.Performed = true;
        }
        #endregion

        #region Public Properties
        public bool Performed { get; set; }

        private ItemCollection<object> _Results = null;

        public ItemCollection<object> Results
        {
            get { this.LoadSearchResults();return _Results; }
            set { _Results = value; }
        }

        public int ResultsCount
        {
            get
            {
                var response = Results;
                if (response != null)
                {
                    return response.Count;
                }

                return 0;
            }
        }

        public string Terms { get; set; }
        #endregion

        #region Methods
        private void LoadSearchResults()
        {
            if (!this.Performed)
            {
                return;
            }

            var pageSize = this.Context == null ? 20 : this.Context["paginate.page_size"].ToInt(20);
            var skip = this.Context == null ? 0 : this.Context["paginate.current_offset"].ToInt();
            var terms = this.Terms; //this.Context["current_query"] as string;
            var type = this.Context == null ? "product" : this.Context["current_type"] as string;

            var siteContext = SiteContext.Current;
            var service = new CommerceService();
            var response = Task.Run(() => service.SearchAsync<object>(siteContext, type, terms, String.Empty, skip, pageSize)).Result;
            this.Results = response;

            this.Performed = false;
        }
        #endregion
    }
}
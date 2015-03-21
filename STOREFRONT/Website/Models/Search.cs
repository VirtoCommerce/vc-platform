#region
using System.Collections.Generic;
using System.Runtime.Serialization;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{

    #region
    #endregion

    [DataContract]
    public class Search : Drop
    {
        #region Constructors and Destructors
        public Search()
        {
            this.Results = new List<object>();
        }
        #endregion

        #region Public Properties
        public bool Performed { get; set; }

        public List<object> Results { get; set; }

        public int ResultsCount { get; set; }

        public string Terms { get; set; }
        #endregion
    }
}
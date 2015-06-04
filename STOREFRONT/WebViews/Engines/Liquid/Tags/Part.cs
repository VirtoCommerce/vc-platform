#region
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid.Tags
{

    #region
    #endregion

    public class Part : Drop
    {
        #region Public Properties
        public bool IsLink { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
        #endregion
    }
}
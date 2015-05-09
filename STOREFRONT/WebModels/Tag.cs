#region
using System;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{
    public class Tag : Drop
    {
        #region Public Properties
        public int Count { get; set; }

        public string Field { get; set; }

        public string Label { get; set; }
        #endregion

        #region Public Methods and Operators
        public override object ToLiquid()
        {
            return this.ToString();
        }

        public override string ToString()
        {
            //return String.Format("{0}_{1} ({2})", Field, Label, Count);

            // eliminate count for now, since it problematic to make it work in some templates, especially when determine active tag
            return String.Format("{0}_{1}", this.Field, this.Label).ToLower();
        }
        #endregion
    }
}
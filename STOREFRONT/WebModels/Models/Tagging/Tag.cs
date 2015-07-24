#region

using System;
using DotLiquid;
using VirtoCommerce.ApiClient.Extensions;

#endregion

namespace VirtoCommerce.Web.Models.Tagging
{
    public class Tag : Drop
    {
        #region Public Properties
        public int Count { get; set; }

        public string Field { get; set; }

        public string Label { get; set; }

        public object Value { get; set; }

        public string Id
        {
            get { return String.Format("{0}_{1}", this.Field, this.Value); }
        }
        #endregion


        #region Public Methods and Operators
        static public explicit operator string (Tag tag)
        {
            return tag.ToString();
        }

        public override string ToString()
        {
            var filters = this.Context["collection_sidebar_filters"].ToNullOrString();

            if (filters == "groups")
            {
                // eliminate count for now, since it problematic to make it work in some templates, especially when determine active tag
                return String.Format("{0}_{1}", this.Field, this.Value).ToLower();
            }

            if (filters == "facets")
            {
                return String.Format("{0}_{1} ({2})", this.Field, this.Label, this.Count);
            }

            return this.Id;
        }

        public override object ToLiquid()
        {
            return this;
        }

        /// <summary>
        /// makes tostring default method
        /// </summary>
        /// <returns></returns>
        public object ConvertToValueType()
        {
            return this.ToString();
        }

        #endregion
    }


}
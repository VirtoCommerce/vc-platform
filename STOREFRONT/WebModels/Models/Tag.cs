#region
using System;
using DotLiquid;
using VirtoCommerce.ApiClient.Extensions;

#endregion

namespace VirtoCommerce.Web.Models
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
            get { return String.Format("{0}_{1}", Field, Value); }
        }
        #endregion


        #region Public Methods and Operators
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
                return String.Format("{0}_{1} ({2})", Field, Label, Count);
            }

            return this.Value.ToString();
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

        public class TagDrop : IContextAware
        {
            public int Count
            {
                get { return Tag.Count; }
            }

            public string Field
            {
                get { return Tag.Field; }
            }

            public string Label
            {
                get { return Tag.Label; }
            }

            public object Value
            {
                get { return Tag.Value; }
            }

            public Tag Tag { get; set; }

            public Context Context { get; set; }

            public TagDrop(Tag tag)
            {
                Tag = tag;
            }

            public override string ToString()
            {
                return Tag.ToString();
            }
        }
    }
}
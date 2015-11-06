using System;
using DotLiquid;
using VirtoCommerce.ApiClient.Extensions;

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
            get { return string.Format("{0}_{1}", Field, Value); }
        }
        #endregion


        #region Public Methods and Operators
        static public explicit operator string (Tag tag)
        {
            return tag.ToString();
        }

        public override string ToString()
        {
            var filters = Context["collection_sidebar_filters"].ToNullOrString();

            if (filters == "groups")
            {
                // eliminate count for now, since it problematic to make it work in some templates, especially when determine active tag
                return string.Format("{0}_{1}", Field, Value).ToLower();
            }

            if (filters == "facets")
            {
                return string.Format("{0}_{1} ({2})", Field, Label, Count);
            }

            return Id;
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
            return ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is string)
            {
                return Value.Equals((string)obj) || Label.Equals((string)obj);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}

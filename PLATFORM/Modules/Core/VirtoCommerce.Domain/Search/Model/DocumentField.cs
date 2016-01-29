using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Domain.Search.Model
{
    public class DocumentField : IDocumentField
    {
        public string Name { get; set; }

        public object[] Values { get; set; }

        public object[] Attributes { get; set; }

        public object Value
        {
            get
            {
                if (Values != null && Values.Length > 0)
                    return Values[0];

                return null;
            }
        }

        public DocumentField(string name, object value)
        {
            Name = name;
            Values = new[] { value };
        }

        public DocumentField(string name, object[] value)
        {
            Name = name;
            Values = value;
        }

        public DocumentField(string name, object value, string[] attributes)
        {
            Name = name;
            Values = new[] { value };
            Attributes = attributes;
        }

        /// <summary>
        /// Determines whether the specified value contains value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value contains value; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsValue(string value)
        {
            if (Values == null || Values.Length == 0)
                return false;

            foreach (object val in Values)
            {
                if (val == null)
                    continue;

                if (val.ToString().Equals(value, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified value contains attribute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value contains attribute; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsAttribute(string value)
        {
            if (Attributes == null || Attributes.Length == 0)
                return false;

            foreach (object val in Attributes)
            {
                if (val == null)
                    continue;

                if (val.ToString().Equals(value, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Adds the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void AddValue(object value)
        {
            if (!Values.Contains(value))
            {
                var values = new List<object>(Values) { value };
                Values = values.ToArray();
            }
        }
    }
}

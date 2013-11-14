using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Search
{
    [DataContract]
    public class DocumentField : IDocumentField
    {
        private object[] _Values = null;

        [DataMember]
        public object[] Attributes
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        public object Value
        {
            get
            {
                if (_Values != null && _Values.Length > 0)
                    return _Values[0];

                return null;
            }
        }

        [DataMember]
        public object[] Values
        {
            get { return _Values; }
            set { _Values = value; }
        }

        public DocumentField(string name, object value)
        {
            this.Name = name;
            this.Values = new object[] { value };
        }

        public DocumentField(string name, object[] value)
        {
            this.Name = name;
            this.Values = value;
        }

        public DocumentField(string name, object value, string[] attributes)
        {
            this.Name = name;
            this.Values = new object[] { value };
            this.Attributes = attributes;
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
            if (this.Values == null || this.Values.Length == 0)
                return false;

            foreach (object val in this.Values)
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
            if (this.Attributes == null || this.Attributes.Length == 0)
                return false;

            foreach (object val in this.Attributes)
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
        public void AddValue(object value)
        {
            List<object> vals = new List<object>(this.Values);
            vals.Add(value);
            _Values = vals.ToArray();
        }
    }
}

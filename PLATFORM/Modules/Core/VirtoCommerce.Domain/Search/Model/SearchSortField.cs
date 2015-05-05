using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Search
{
    public class SearchSortField
    {
        //
        // Summary:
        //     Sort using term values as encoded Bytes. Sort values are Byte and lower values
        //     are at the front.
        public const int BYTE = 10;
 
        //
        // Summary:
        //     Sort using term values as encoded Doubles. Sort values are Double and lower
        //     values are at the front.
        public const int DOUBLE = 7;
        //
        // Summary:
        //     Sort using term values as encoded Floats. Sort values are Float and lower
        //     values are at the front.
        public const int FLOAT = 5;
        //
        // Summary:
        //     Sort using term values as encoded Integers. Sort values are Integer and lower
        //     values are at the front.
        public const int INT = 4;
        //
        // Summary:
        //     Sort using term values as encoded Longs. Sort values are Long and lower values
        //     are at the front.
        public const int LONG = 6;
        //
        // Summary:
        //     Sort by document score (relevancy). Sort values are Float and higher values
        //     are at the front.
        public const int SCORE = 0;
        //
        // Summary:
        //     Sort using term values as encoded Shorts. Sort values are Short and lower
        //     values are at the front.
        public const int SHORT = 8;
        //
        // Summary:
        //     Sort using term values as Strings. Sort values are String and lower values
        //     are at the front.
        public const int STRING = 3;
        //
        // Summary:
        //     Sort using term values as Strings, but comparing by value (using String.compareTo)
        //     for all comparisons.  This is typically slower than Lucene.Net.Search.SortField.STRING,
        //     which uses ordinals to do the sorting.
        public const int STRING_VAL = 11;

        private string _FieldName;
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

		public int DataType
        {
            get;set;
        }

        private bool _isDescending = false;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is descending.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is descending; otherwise, <c>false</c>.
        /// </value>
        public bool IsDescending
        {
            get { return _isDescending; }
            set { _isDescending = value; }
        }

        private bool _IgnoredUnmapped = false;
        public bool IgnoredUnmapped
        {
            get { return _IgnoredUnmapped; }
            set { _IgnoredUnmapped = value; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSortField"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public SearchSortField(string fieldName, bool isDescending)
        {
            _FieldName = fieldName;
            _isDescending = isDescending;
            DataType = STRING;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSortField"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="type">The type.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public SearchSortField(string fieldName, int type, bool isDescending)
        {
            _FieldName = fieldName;
            _isDescending = isDescending;
            DataType = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSortField"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public SearchSortField(string fieldName)
        {
            _FieldName = fieldName;
            DataType = STRING;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return FieldName + "-" + IsDescending.ToString();
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Search
{
    public class SearchSort
    {
        // internal representation of the sort criteria
        SearchSortField[] _Fields;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSort"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public SearchSort(string fieldName, bool isDescending)
        {
            SetSort(fieldName, isDescending);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSort"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public SearchSort(string fieldName)
        {
            SetSort(fieldName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSort"/> class.
        /// </summary>
        /// <param name="fieldNames">The field names.</param>
        public SearchSort(string[] fieldNames)
        {
            SetSort(fieldNames);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSort"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        public SearchSort(SearchSortField field)
        {
            SetSort(field);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSort"/> class.
        /// </summary>
        /// <param name="fields">The fields.</param>
        public SearchSort(SearchSortField[] fields)
        {
            SetSort(fields);
        }

        /// <summary>
        /// Sets the sort to the terms in <code>field</code> then by index order
        /// (document number).
        /// </summary>
        /// <param name="field">The field.</param>
        public void SetSort(string field)
        {
            SetSort(field, false);
        }

        /// <summary>
        /// Sets the sort.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public virtual void SetSort(string field, bool isDescending)
        {
            SearchSortField[] nfields = new SearchSortField[] { new SearchSortField(field, isDescending) };
            _Fields = nfields;
        }

        /// <summary>
        /// Sets the sort to the terms in each field in succession.
        /// </summary>
        /// <param name="fieldNames">The fieldnames.</param>
        public virtual void SetSort(string[] fieldNames)
        {
            int n = fieldNames.Length;

            SearchSortField[] nfields = new SearchSortField[n];
            for (int i = 0; i < n; ++i)
            {
                nfields[i] = new SearchSortField(fieldNames[i]);
            }

            _Fields = nfields;
        }

        /// <summary>
        /// Sets the sort to the given criteria.
        /// </summary>
        /// <param name="field">The field.</param>
        public virtual void SetSort(SearchSortField field)
        {
            this._Fields = new SearchSortField[] { field };
        }

        /// <summary>
        /// Sets the sort.
        /// </summary>
        /// <param name="fields">The fields.</param>
        public virtual void SetSort(SearchSortField[] fields)
        {
            _Fields = fields;
        }

        /// <summary> Representation of the sort criteria.</summary>
        /// <returns> Array of SortField objects used in this sort criteria
        /// </returns>
        public virtual SearchSortField[] GetSort()
        {
            return _Fields;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder();

            for (int i = 0; i < _Fields.Length; i++)
            {
                buffer.Append(_Fields[i].ToString());
                if ((i + 1) < _Fields.Length)
                    buffer.Append(',');
            }

            return buffer.ToString();
        }
    }

}

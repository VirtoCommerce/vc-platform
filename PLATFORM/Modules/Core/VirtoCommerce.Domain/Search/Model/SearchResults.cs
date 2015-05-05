using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Search
{
    public class SearchResults : ISearchResults
    {
        ResultDocumentSet[] _Documents = null;
        ISearchCriteria _SearchCriteria = null;

        public virtual ISearchCriteria SearchCriteria
        {
            get { return _SearchCriteria; }
        }

        /// <summary>
        /// Gets or sets the word suggestions.
        /// </summary>
        /// <value>The suggestions.</value>
        public virtual string[] Suggestions
        {
            get;
            set;
        }

        public virtual int TotalCount
        {
            get
            {
                int count = 0;

                if (this.Documents == null)
                    return count;

                foreach (IDocumentSet doc in Documents)
                {
                    count += doc.TotalCount;
                }

                return count;
            }
        }

        public virtual int DocCount
        {
            get
            {
                int count = 0;

                if (this.Documents == null)
                    return count;

                foreach (IDocumentSet doc in Documents)
                {
                    count += doc.Documents.Count();
                }

                return count;
            }
        }

        public virtual ResultDocumentSet[] Documents
        {
            get { return _Documents; }
            set { _Documents = value; }
        }

        public virtual FacetGroup[] FacetGroups { get; set; }

        public virtual T[] GetKeyFieldValues<T>()
        {
            if (this.Documents == null)
                return null;

            return GetKeyFieldValues<T>(this.Documents[0].Name);
        }

        /// <summary>
        /// Gets the array of values from the key field defined in the ISearchCriteria.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documentSetName">Name of the document set.</param>
        /// <returns></returns>
        public virtual T[] GetKeyFieldValues<T>(string documentSetName)
        {
            if (this.Documents == null)
                return null;

            List<T> entries = new List<T>();

            foreach (IDocumentSet set in this.Documents)
            {
                if (!String.IsNullOrEmpty(set.Name) && !String.IsNullOrEmpty(documentSetName) && !documentSetName.Equals(set.Name, StringComparison.OrdinalIgnoreCase))
                    continue;

                foreach (IDocument doc in set.Documents)
                {
                    T id = (T)Convert.ChangeType(doc[this.SearchCriteria.KeyField].Value.ToString(), typeof(T));
                    entries.Add(id);
                }
            }

            return entries.ToArray();
        }

        public virtual Dictionary<T, Dictionary<string, object>> GetKeyAndOutlineFieldValueMap<T>()
		{
			return Documents == null ? null : GetKeyAndOutlineFieldValueMap<T>(Documents[0].Name);
		}

	    public virtual Dictionary<T,Dictionary<string,object>> GetKeyAndOutlineFieldValueMap<T>(string documentSetName)
		{
			if (Documents == null)
				return null;

			var entries = new Dictionary<T, Dictionary<string,object>>();

			foreach (var set in Documents)
			{
				if (!String.IsNullOrEmpty(set.Name) && 
					!String.IsNullOrEmpty(documentSetName) && 
					!documentSetName.Equals(set.Name, StringComparison.OrdinalIgnoreCase))
					continue;

				foreach (var doc in set.Documents)
				{
                    var id = (T)Convert.ChangeType(doc[SearchCriteria.KeyField].Value.ToString(), typeof(T));
				    var tags = new Dictionary<string, object>
				    {
				        {SearchCriteria.OutlineField , GetFieldValue(doc, SearchCriteria.OutlineField)},
                        {SearchCriteria.ReviewsAverageField , GetFieldValue(doc, SearchCriteria.ReviewsAverageField)},
                        {SearchCriteria.ReviewsTotalField , GetFieldValue(doc, SearchCriteria.ReviewsTotalField)},
				    };

                    if (!entries.ContainsKey(id))// THIS SHOULD NEVER HAPPEN!!!
                        entries.Add(id, tags);
				}
			}

			return entries;
		}

        public SearchResults(ISearchCriteria criteria, ResultDocumentSet[] documents)
        {
            _SearchCriteria = criteria;
            _Documents = documents;
        }

        private string GetFieldValue(IDocument doc, string fieldName)
        {
            var value = "";

            var field = doc[fieldName];
            if (field != null)
            {
                var array = doc[fieldName].Values;
                if (array != null)
                {
                    var values = new List<string>();

                    foreach (var val in array)
                    {
                        var enumerate = val as IEnumerable;
                        if (val is string || enumerate == null)
                        {
                            values.Add(val != null ? val.ToString() : "");
                        }
                        else
                        {
                            values.AddRange(from object val1 in enumerate select val1.ToString());
                        }
                    }

                    value = String.Join(";", values);
                }
                else
                {
                    value = doc[fieldName].Value.ToString();
                }
            }

            return value;
        }
    }
}

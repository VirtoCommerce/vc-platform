using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Search.Facets;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Search
{
    [DataContract/*,Serializable*/]
    public class SearchResults : ISearchResults
    {
        ResultDocumentSet[] _Documents = null;
        [DataMember]
        ISearchCriteria _SearchCriteria = null;

        public virtual ISearchCriteria SearchCriteria
        {
            get { return _SearchCriteria; }
        }

        /// <summary>
        /// Gets or sets the word suggestions.
        /// </summary>
        /// <value>The suggestions.</value>
        [DataMember]
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

        [DataMember]
        public virtual ResultDocumentSet[] Documents
        {
            get { return _Documents; }
            set { _Documents = value; }
        }

        [DataMember]
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

		public virtual Dictionary<T, string> GetKeyAndOutlineFieldValueMap<T>()
		{
			return Documents == null ? null : GetKeyAndOutlineFieldValueMap<T>(Documents[0].Name);
		}

	    public virtual Dictionary<T,string> GetKeyAndOutlineFieldValueMap<T>(string documentSetName)
		{
			if (Documents == null)
				return null;

			var entries = new Dictionary<T, string>();

			foreach (var set in Documents)
			{
				if (!String.IsNullOrEmpty(set.Name) && 
					!String.IsNullOrEmpty(documentSetName) && 
					!documentSetName.Equals(set.Name, StringComparison.OrdinalIgnoreCase))
					continue;

				foreach (var doc in set.Documents)
				{
					var id = (T)Convert.ChangeType(doc[SearchCriteria.KeyField].Value.ToString(), typeof(T));
					string outline;

					var array = doc[SearchCriteria.OutlineField].Value as IEnumerable;		
					if (array != null)
					{
						var outlines = new List<string>();
						var valEnum = array.GetEnumerator();
						while(valEnum.MoveNext())
						{
							outlines.Add(valEnum.Current.ToString());
						}
						outline = String.Join(";", outlines);
					}
					else
					{
						outline = doc[SearchCriteria.OutlineField].Value.ToString();
					}

					entries.Add(id, outline);
				}
			}

			return entries;
		}

        public SearchResults(ISearchCriteria criteria, ResultDocumentSet[] documents)
        {
            _SearchCriteria = criteria;
            _Documents = documents;
        }
    }
}

using System;
using System.Runtime.Serialization;
namespace VirtoCommerce.Domain.Search
{


    public class KeywordSearchCriteria : SearchCriteriaBase
    {
        private string _SearchPhrase = String.Empty;

        /// <summary>
        /// Gets or sets the search phrase.
        /// </summary>
        /// <value>The search phrase.</value>
        public virtual string SearchPhrase
        {
            get { return _SearchPhrase; }
            set { ChangeState(); _SearchPhrase = value; }
        }

		/// <summary>
        /// Initializes a new instance of the <see cref="KeywordSearchCriteria"/> class.
		/// </summary>
		/// <param name="documentType">Type of the document.</param>
        public KeywordSearchCriteria(string documentType)
			: base(documentType)
		{

		}
    }
}

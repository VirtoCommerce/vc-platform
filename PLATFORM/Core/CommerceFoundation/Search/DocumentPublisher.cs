
namespace VirtoCommerce.Foundation.Search
{
    public class DocumentPublisher
    {
        readonly ISearchProvider _search = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentPublisher"/> class.
        /// </summary>
        /// <param name="search">The search.</param>
        public DocumentPublisher(ISearchProvider search)
        {
            _search = search;
        }

        /// <summary>
        /// Submits the documents.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="documents">The documents.</param>
        public void SubmitDocuments(string scope, string documentType, IDocument[] documents)
        {
            foreach (var doc in documents)
            {
                _search.Index(scope, documentType, doc);
            }

            _search.Commit(scope);
            _search.Close(scope, documentType);
        }

        /// <summary>
        /// Removes the documents.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="documents">The documents.</param>
        public void RemoveDocuments(string scope, string documentType, string[] documents)
        {
            foreach (var doc in documents)
            {
                _search.Remove(scope, documentType, "__key", doc);
            }

            _search.Commit(scope);
        }
    }
}

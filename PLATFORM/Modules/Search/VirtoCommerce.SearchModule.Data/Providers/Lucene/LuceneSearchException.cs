using System;
using VirtoCommerce.Domain.Search;

namespace VirtoCommerce.SearchModule.Data.Provides.Lucene
{

    public class LuceneSearchException : SearchException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LuceneSearchException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public LuceneSearchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LuceneSearchException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public LuceneSearchException(string message)
            : base(message)
        {
        }
    }
}

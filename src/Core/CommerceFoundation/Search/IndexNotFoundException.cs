using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Search
{
    public class IndexNotFoundException : SearchException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public IndexNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public IndexNotFoundException(string message)
            : base(message)
        {
        }
    }

}

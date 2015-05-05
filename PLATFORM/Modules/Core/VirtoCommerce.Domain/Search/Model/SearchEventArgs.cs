using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Domain.Search
{
    public class SearchEventArgs : EventArgs
    {
        private string _Message = String.Empty;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchEventArgs"/> class.
        /// </summary>
        public SearchEventArgs()
            : base()
        {
        }
    }

}

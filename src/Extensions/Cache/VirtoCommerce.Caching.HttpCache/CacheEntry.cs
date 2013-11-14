using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Caching.HttpCache
{
    internal class CacheEntry
    {
        private readonly object _Lock;

        /// <summary>
        /// Gets the lock.
        /// </summary>
        /// <value>The lock.</value>
        public object Lock
        {
            get { return _Lock; }
        }

        internal CacheEntry()
        {
            _Lock = new object();
        }
    }

}

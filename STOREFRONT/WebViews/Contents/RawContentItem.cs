#region
using System.Collections.Generic;

#endregion

namespace VirtoCommerce.Web.Views.Contents
{
    internal class RawContentItem
    {
        #region Public Properties
        public string Content { get; set; }

        public string ContentType { get; set; }

        public IDictionary<string, dynamic> Settings { get; set; }
        #endregion
    }
}
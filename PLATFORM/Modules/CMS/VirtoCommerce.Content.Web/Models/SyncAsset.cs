using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
    public class SyncAsset
    {
        public string Id { get; set; }

        /// <summary>
        /// Asset element name, contains full path of theme asset or page element relative to the root
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Asset element text content (text files - html, js, css, txt, liquid &amp; etc), based on content type
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Asset element byte content (non-text files - images, fonts, zips, pdfs &amp; etc), based on content type
        /// </summary>
        public byte[] ByteContent { get; set; }

        public string AssetUrl { get; set; }

        public string ContentType { get; set; }

        /// <summary>
        /// Last updated date of the asset element
        /// </summary>
        public DateTime Updated { get; set; }
    }
}
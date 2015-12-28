using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
    /// <summary>
    /// Represent dictionary property values 
    /// </summary>
	public class PropertyDictionaryValue : Entity
	{
        /// <summary>
        /// Property identifier
        /// </summary>
		public string PropertyId { get; set; }
		public Property Property { get; set; }
        /// <summary>
        /// Alias for value used for group same dict values in different languages
        /// </summary>
		public string Alias { get; set; }
        /// <summary>
        /// Language
        /// </summary>
		public string LanguageCode { get; set; }
		public string Value { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	/// <summary>
	/// Promotion entry populate interface. This interface will be used to populate promotion entry object with attributes from the line item or other object.
	/// </summary>
	public interface IPromotionEntryPopulate
	{
		/// <summary>
		/// Populates the specified promotion entry with attribute values from the object.
		/// </summary>
		/// <param name="entry">The promo entry.</param>
		/// <param name="data">The item.</param>
		void Populate(ref PromotionEntry entry, object data);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	[Flags]
	public enum SearchResponseGroup
	{
		WithPromotions = 1,
		WithContentItems = 2,
		WithContentPlaces = 4,
		WithContentPublications = 8,
		WithFolders = 16,
		Full = WithPromotions | WithContentItems | WithContentPlaces | WithContentPublications | WithFolders
	}
}

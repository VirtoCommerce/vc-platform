using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.Domain.Marketing.Services
{
	public interface IDynamicContentService
	{
		DynamicContentItem GetContentItemById(string id);
		DynamicContentItem CreateContent(DynamicContentItem content);
		void UpdateContents(DynamicContentItem[] contents);
		void DeleteContents(string[] ids);

		DynamicContentPlace GetPlaceById(string id);
		DynamicContentPlace CreatePlace(DynamicContentPlace place);
		void UpdatePlace(DynamicContentPlace place);
		void DeletePlaces(string[] ids);

		DynamicContentPublication GetPublicationById(string id);
		DynamicContentPublication CreatePublication(DynamicContentPublication publications);
		void UpdatePublications(DynamicContentPublication[] publications);
		void DeletePublications(string[] ids);
	}
}

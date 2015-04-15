using Omu.ValueInjecter;
using System.Linq;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
	public static class DynamicContentPublicationConverter
	{
		public static webModel.DynamicContentPublication ToWebModel(this coreModel.DynamicContentPublication publication)
		{
			var retVal = new webModel.DynamicContentPublication();
			retVal.InjectFrom(publication);
			if(publication.ContentItems != null)
			{
				retVal.ContentItems = publication.ContentItems.Select(x => x.ToWebModel()).ToList();
			}
			if (publication.ContentPlaces != null)
			{
				retVal.ContentPlaces = publication.ContentPlaces.Select(x => x.ToWebModel()).ToList();
			}
			return retVal;
		}

		public static coreModel.DynamicContentPublication ToCoreModel(this webModel.DynamicContentPublication publication)
		{
			var retVal = new coreModel.DynamicContentPublication();
			retVal.InjectFrom(publication);
			if (publication.ContentItems != null)
			{
				retVal.ContentItems = publication.ContentItems.Select(x => x.ToCoreModel()).ToList();
			}
			if (publication.ContentPlaces != null)
			{
				retVal.ContentPlaces = publication.ContentPlaces.Select(x => x.ToCoreModel()).ToList();
			}
			return retVal;
		}
	
	}
}

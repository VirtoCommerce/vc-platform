using Omu.ValueInjecter;
using System.Linq;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
	public static class ContentItemConverter
	{
		public static webModel.DynamicContentItem ToWebModel(this coreModel.DynamicContentItem content)
		{
			var retVal = new webModel.DynamicContentItem();
			retVal.InjectFrom(content);
			if(content.Folder != null)
			{
				retVal.Outline = content.Folder.Outline;
				retVal.Path = content.Folder.Path;
			}
			if(content.Properties != null)
			{
				retVal.Properties = content.Properties.Select(x => x.ToWebModel()).ToList();
			}
			return retVal;
		}

		public static coreModel.DynamicContentItem ToCoreModel(this webModel.DynamicContentItem content)
		{
			var retVal = new coreModel.DynamicContentItem();
			retVal.InjectFrom(content);
			if (content.Properties != null)
			{
				retVal.Properties = content.Properties.Select(x => x.ToCoreModel()).ToList();
			}
			return retVal;
		}
	
	}
}

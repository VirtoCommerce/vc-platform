using Omu.ValueInjecter;
using System.Linq;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;
using VirtoCommerce.MarketingModule.Expressions.Content;
using System;
using Newtonsoft.Json;
using VirtoCommerce.MarketingModule.Expressions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.MarketingModule.Data.Common;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
	public static class DynamicContentPublicationConverter
	{
		public static webModel.DynamicContentPublication ToWebModel(this coreModel.DynamicContentPublication publication, DynamicContentExpressionTree etalonEpressionTree = null)
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

			retVal.DynamicExpression = etalonEpressionTree;
			if (!String.IsNullOrEmpty(publication.PredicateVisualTreeSerialized))
			{
				retVal.DynamicExpression = JsonConvert.DeserializeObject<DynamicContentExpressionTree>(publication.PredicateVisualTreeSerialized);
				if (etalonEpressionTree != null)
				{
					//Copy available elements from etalon because they not persisted
					var sourceBlocks = ((DynamicExpression)etalonEpressionTree).Traverse(x => x.AvailableChildren);
					var targetBlocks = ((DynamicExpression)retVal.DynamicExpression).Traverse(x => x.Children);
					foreach (var sourceBlock in sourceBlocks)
					{
						foreach (var targetBlock in targetBlocks.Where(x => x.Id == sourceBlock.Id))
						{
							targetBlock.AvailableChildren = sourceBlock.AvailableChildren;
						}
					}
					//copy available elements from etalon
					retVal.DynamicExpression.AvailableChildren = etalonEpressionTree.AvailableChildren;
				}
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

			var conditionExpression = publication.DynamicExpression.GetConditionExpression();
			retVal.PredicateSerialized = SerializationUtil.SerializeExpression(conditionExpression);
		
			//Clear availableElements in expression (for decrease size)
			publication.DynamicExpression.AvailableChildren = null;
			var allBlocks = ((DynamicExpression)publication.DynamicExpression).Traverse(x => x.Children);
			foreach (var block in allBlocks)
			{
				block.AvailableChildren = null;
			}
			retVal.PredicateVisualTreeSerialized = JsonConvert.SerializeObject(publication.DynamicExpression);
			return retVal;
		}
	
	}
}

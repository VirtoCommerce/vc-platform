using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using dataModel = VirtoCommerce.MarketingModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data.Promotions;
using ExpressionSerialization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
	public static class ContentPublicationConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.DynamicContentPublication ToCoreModel(this dataModel.DynamicContentPublishingGroup dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.DynamicContentPublication();
			retVal.InjectFrom(dbEntity);

			retVal.PredicateSerialized = dbEntity.ConditionExpression;
			retVal.PredicateVisualTreeSerialized = dbEntity.PredicateVisualTreeSerialized;

			if (dbEntity.ContentItems != null)
			{
				retVal.ContentItems = dbEntity.ContentItems.Select(x => x.ContentItem.ToCoreModel()).ToList();
			}
			if (dbEntity.ContentPlaces != null)
			{
				retVal.ContentPlaces = dbEntity.ContentPlaces.Select(x => x.ContentPlace.ToCoreModel()).ToList();
			}

			return retVal;
		}


		public static dataModel.DynamicContentPublishingGroup ToDataModel(this coreModel.DynamicContentPublication publication)
		{
			if (publication == null)
				throw new ArgumentNullException("publication");

			var retVal = new dataModel.DynamicContentPublishingGroup();
			retVal.InjectFrom(publication);

			retVal.ConditionExpression = publication.PredicateSerialized;
			retVal.PredicateVisualTreeSerialized = publication.PredicateVisualTreeSerialized;

			if (publication.ContentItems != null)
			{
				retVal.ContentItems = new ObservableCollection<dataModel.PublishingGroupContentItem>(publication.ContentItems.Select(x => new dataModel.PublishingGroupContentItem { DynamicContentPublishingGroupId = retVal.Id, DynamicContentItemId = x.Id }));
			}
			if (publication.ContentPlaces != null)
			{
				retVal.ContentPlaces = new ObservableCollection<dataModel.PublishingGroupContentPlace>(publication.ContentPlaces.Select(x => new dataModel.PublishingGroupContentPlace { DynamicContentPublishingGroupId = retVal.Id, DynamicContentPlaceId = x.Id }));
			}
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.DynamicContentPublishingGroup source, dataModel.DynamicContentPublishingGroup target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<dataModel.DynamicContentPublishingGroup>(x => x.Name, x => x.Description, x => x.IsActive,
																								  x => x.StartDate, x => x.EndDate, x=>x.PredicateVisualTreeSerialized, x=>x.ConditionExpression);

			target.InjectFrom(patchInjection, source);

			if (!source.ContentItems.IsNullCollection())
			{
				var itemComparer = AnonymousComparer.Create((dataModel.PublishingGroupContentItem x) => x.DynamicContentItemId);
				source.ContentItems.Patch(target.ContentItems, itemComparer, (sourceProperty, targetProperty) => { });
			}

			if (!source.ContentPlaces.IsNullCollection())
			{
				var itemComparer = AnonymousComparer.Create((dataModel.PublishingGroupContentPlace x) => x.DynamicContentPlaceId);
				source.ContentPlaces.Patch(target.ContentPlaces, itemComparer, (sourceProperty, targetProperty) => { });
			}

		}


	}
}

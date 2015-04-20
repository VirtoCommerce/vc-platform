using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.Foundation.Frameworks;
using System.Collections.ObjectModel;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using foundationModel = VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data.Promotions;
using ExpressionSerialization;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
	public static class ContentPublicationConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.DynamicContentPublication ToCoreModel(this foundationModel.DynamicContentPublishingGroup dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.DynamicContentPublication();
			retVal.InjectFrom(dbEntity);
			retVal.Id = dbEntity.DynamicContentPublishingGroupId;

			retVal.CreatedDate = dbEntity.Created.Value;
			retVal.ModifiedDate = dbEntity.LastModified;
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


		public static foundationModel.DynamicContentPublishingGroup ToFoundation(this coreModel.DynamicContentPublication publication)
		{
			if (publication == null)
				throw new ArgumentNullException("publication");

			var retVal = new foundationModel.DynamicContentPublishingGroup();
			retVal.InjectFrom(publication);

			if (publication.Id != null)
			{
				retVal.DynamicContentPublishingGroupId = publication.Id;
			}
			retVal.ContentItems = new NullCollection<foundationModel.PublishingGroupContentItem>();
			if (publication.ContentItems != null)
			{
				retVal.ContentItems = new ObservableCollection<foundationModel.PublishingGroupContentItem>(publication.ContentItems.Select(x => new foundationModel.PublishingGroupContentItem { DynamicContentPublishingGroupId = retVal.DynamicContentPublishingGroupId, DynamicContentItemId = x.Id }));
			}
			retVal.ContentPlaces = new NullCollection<foundationModel.PublishingGroupContentPlace>();
			if (publication.ContentPlaces != null)
			{
				retVal.ContentPlaces = new ObservableCollection<foundationModel.PublishingGroupContentPlace>(publication.ContentPlaces.Select(x => new foundationModel.PublishingGroupContentPlace { DynamicContentPublishingGroupId = retVal.DynamicContentPublishingGroupId, DynamicContentPlaceId = x.Id }));
			}
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.DynamicContentPublishingGroup source, foundationModel.DynamicContentPublishingGroup target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<foundationModel.DynamicContentPublishingGroup>(x => x.Name, x => x.Description, x => x.IsActive,
																								  x => x.StartDate, x => x.EndDate);

			target.InjectFrom(patchInjection, source);

			if (!source.ContentItems.IsNullCollection())
			{
				var itemComparer = AnonymousComparer.Create((foundationModel.PublishingGroupContentItem x) => x.DynamicContentItemId);
				source.ContentItems.Patch(target.ContentItems, itemComparer, (sourceProperty, targetProperty) => { });
			}

			if (!source.ContentPlaces.IsNullCollection())
			{
				var itemComparer = AnonymousComparer.Create((foundationModel.PublishingGroupContentPlace x) => x.DynamicContentPlaceId);
				source.ContentPlaces.Patch(target.ContentPlaces, itemComparer, (sourceProperty, targetProperty) => { });
			}

		}


	}
}

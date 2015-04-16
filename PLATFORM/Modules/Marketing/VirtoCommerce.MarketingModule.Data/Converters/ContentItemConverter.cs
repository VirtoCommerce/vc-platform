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
	public static class ContentItemConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.DynamicContentItem ToCoreModel(this foundationModel.DynamicContentItem dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.DynamicContentItem();
			retVal.InjectFrom(dbEntity);
			retVal.Id = dbEntity.DynamicContentItemId;

			retVal.CreatedDate = dbEntity.Created.Value;
			retVal.ModifiedDate = dbEntity.LastModified;
			retVal.ContentType = dbEntity.ContentTypeId;
			retVal.Properties = dbEntity.PropertyValues.Select(x => x.ToCoreModel()).ToList();

			if (dbEntity.Folder != null)
			{
				retVal.Folder = dbEntity.Folder.ToCoreModel();
			}

			return retVal;
		}


		public static foundationModel.DynamicContentItem ToFoundation(this coreModel.DynamicContentItem contentItem)
		{
			if (contentItem == null)
				throw new ArgumentNullException("contentItem");

			var retVal = new foundationModel.DynamicContentItem();
			retVal.InjectFrom(contentItem);
			retVal.ContentTypeId = contentItem.ContentType;
			if (contentItem.Id != null)
			{
				retVal.DynamicContentItemId = contentItem.Id;
			}
			retVal.PropertyValues = new NullCollection<foundationModel.DynamicContentItemProperty>();
			if(contentItem.Properties != null)
			{
				retVal.PropertyValues = new ObservableCollection<foundationModel.DynamicContentItemProperty>(contentItem.Properties.Select(x => x.ToFoundation()));
				foreach (var property in retVal.PropertyValues)
				{
					property.DynamicContentItemId = retVal.DynamicContentItemId;
				}
			}
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.DynamicContentItem source, foundationModel.DynamicContentItem target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<foundationModel.DynamicContentItem>(x => x.Name, x => x.Description, x=>x.FolderId, x=>x.ImageUrl, x=>x.ContentTypeId);
			if (!source.PropertyValues.IsNullCollection())
			{
				var propertyComparer = AnonymousComparer.Create((foundationModel.DynamicContentItemProperty x) => x.PropertyValueId);
				source.PropertyValues.Patch(target.PropertyValues, propertyComparer, (sourceProperty, targetProperty) => sourceProperty.Patch(targetProperty));
			}
			target.InjectFrom(patchInjection, source);
		}


	}
}

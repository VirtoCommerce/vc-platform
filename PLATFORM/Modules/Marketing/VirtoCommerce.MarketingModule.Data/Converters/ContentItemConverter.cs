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
	public static class ContentItemConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.DynamicContentItem ToCoreModel(this dataModel.DynamicContentItem dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.DynamicContentItem();
			retVal.InjectFrom(dbEntity);

			retVal.ContentType = dbEntity.ContentTypeId;
			retVal.Properties = dbEntity.PropertyValues.Select(x => x.ToCoreModel()).ToList();

			if (dbEntity.Folder != null)
			{
				retVal.Folder = dbEntity.Folder.ToCoreModel();
			}

			return retVal;
		}


		public static dataModel.DynamicContentItem ToDataModel(this coreModel.DynamicContentItem contentItem)
		{
			if (contentItem == null)
				throw new ArgumentNullException("contentItem");

			var retVal = new dataModel.DynamicContentItem();
			retVal.InjectFrom(contentItem);
			retVal.ContentTypeId = contentItem.ContentType;
		
			retVal.PropertyValues = new NullCollection<dataModel.DynamicContentItemProperty>();
			if(contentItem.Properties != null)
			{
				retVal.PropertyValues = new ObservableCollection<dataModel.DynamicContentItemProperty>(contentItem.Properties.Select(x => x.ToFoundation()));
				foreach (var property in retVal.PropertyValues)
				{
					property.DynamicContentItemId = retVal.Id;
				}
			}
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.DynamicContentItem source, dataModel.DynamicContentItem target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<dataModel.DynamicContentItem>(x => x.Name, x => x.Description, x=>x.FolderId, x=>x.ImageUrl, x=>x.ContentTypeId);
			if (!source.PropertyValues.IsNullCollection())
			{
				var propertyComparer = AnonymousComparer.Create((dataModel.DynamicContentItemProperty x) => x.Id);
				source.PropertyValues.Patch(target.PropertyValues, propertyComparer, (sourceProperty, targetProperty) => sourceProperty.Patch(targetProperty));
			}
			target.InjectFrom(patchInjection, source);
		}


	}
}

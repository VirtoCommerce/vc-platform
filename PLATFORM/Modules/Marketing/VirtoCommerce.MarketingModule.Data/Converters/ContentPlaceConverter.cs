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
	public static class ContentPlaceConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.DynamicContentPlace ToCoreModel(this foundationModel.DynamicContentPlace dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.DynamicContentPlace();
			retVal.InjectFrom(dbEntity);
			retVal.Id = dbEntity.DynamicContentPlaceId;

			retVal.CreatedDate = dbEntity.Created.Value;
			retVal.ModifiedDate = dbEntity.LastModified;

			if(dbEntity.Folder != null)
			{
				retVal.Folder = dbEntity.Folder.ToCoreModel();
			}

			return retVal;
		}


		public static foundationModel.DynamicContentPlace ToFoundation(this coreModel.DynamicContentPlace contentPlace)
		{
			if (contentPlace == null)
				throw new ArgumentNullException("contentPlace");

			var retVal = new foundationModel.DynamicContentPlace();
			retVal.InjectFrom(contentPlace);

			if (contentPlace.Id != null)
			{
				retVal.DynamicContentPlaceId = contentPlace.Id;
			}
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.DynamicContentPlace source, foundationModel.DynamicContentPlace target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<foundationModel.DynamicContentPlace>(x => x.Name, x => x.Description, x => x.FolderId, x => x.ImageUrl);
		
			target.InjectFrom(patchInjection, source);
		}


	}
}

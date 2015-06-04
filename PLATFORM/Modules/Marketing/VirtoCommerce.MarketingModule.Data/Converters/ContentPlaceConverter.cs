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
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
	public static class ContentPlaceConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.DynamicContentPlace ToCoreModel(this dataModel.DynamicContentPlace dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.DynamicContentPlace();
			retVal.InjectFrom(dbEntity);

			if(dbEntity.Folder != null)
			{
				retVal.Folder = dbEntity.Folder.ToCoreModel();
			}

			return retVal;
		}


		public static dataModel.DynamicContentPlace ToDataModel(this coreModel.DynamicContentPlace contentPlace)
		{
			if (contentPlace == null)
				throw new ArgumentNullException("contentPlace");

			var retVal = new dataModel.DynamicContentPlace();
			retVal.InjectFrom(contentPlace);

			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.DynamicContentPlace source, dataModel.DynamicContentPlace target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<dataModel.DynamicContentPlace>(x => x.Name, x => x.Description, x => x.FolderId, x => x.ImageUrl);
		
			target.InjectFrom(patchInjection, source);
		}


	}
}

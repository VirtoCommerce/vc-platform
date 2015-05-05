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
	public static class ContentFolderConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.DynamicContentFolder ToCoreModel(this dataModel.DynamicContentFolder dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.DynamicContentFolder();
			retVal.InjectFrom(dbEntity);
			if (dbEntity.ParentFolder != null)
			{
				retVal.ParentFolder = dbEntity.ParentFolder.ToCoreModel();
			}
			return retVal;
		}


		public static dataModel.DynamicContentFolder ToDataModel(this coreModel.DynamicContentFolder contentFolder)
		{
			if (contentFolder == null)
				throw new ArgumentNullException("contentFolder");

			var retVal = new dataModel.DynamicContentFolder();
			retVal.InjectFrom(contentFolder);
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.DynamicContentFolder source, dataModel.DynamicContentFolder target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<dataModel.DynamicContentFolder>(x => x.Name, x => x.Description, x => x.ImageUrl);

			target.InjectFrom(patchInjection, source);
		}


	}
}

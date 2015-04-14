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
	public static class ContentFolderConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.DynamicContentFolder ToCoreModel(this foundationModel.DynamicContentFolder dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.DynamicContentFolder();
			retVal.InjectFrom(dbEntity);
			retVal.Id = dbEntity.DynamicContentFolderId;
			if (dbEntity.ParentFolder != null)
			{
				retVal.ParentFolder = dbEntity.ParentFolder.ToCoreModel();
			}
			return retVal;
		}


		public static foundationModel.DynamicContentFolder ToFoundation(this coreModel.DynamicContentFolder contentFolder)
		{
			if (contentFolder == null)
				throw new ArgumentNullException("contentFolder");

			var retVal = new foundationModel.DynamicContentFolder();
			retVal.InjectFrom(contentFolder);

			if (contentFolder.Id != null)
			{
				retVal.DynamicContentFolderId = contentFolder.Id;
			}
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.DynamicContentFolder source, foundationModel.DynamicContentFolder target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<foundationModel.DynamicContentFolder>(x => x.Name, x => x.Description, x => x.ImageUrl);

			target.InjectFrom(patchInjection, source);
		}


	}
}

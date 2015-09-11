using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreModel = VirtoCommerce.Domain.Quote.Model;
using dataModel = VirtoCommerce.QuoteModule.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.QuoteModule.Data.Converters
{
	public static class AttachmentConverter
	{
		public static coreModel.QuoteAttachment ToCoreModel(this dataModel.AttachmentEntity dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.QuoteAttachment();
			retVal.InjectFrom(dbEntity);

			return retVal;
		}


		public static dataModel.AttachmentEntity ToDataModel(this coreModel.QuoteAttachment attachment)
		{
			if (attachment == null)
				throw new ArgumentNullException("attachment");

			var retVal = new dataModel.AttachmentEntity();
			retVal.InjectFrom(attachment);

			return retVal;
		}
	}
}

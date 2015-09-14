using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreModel = VirtoCommerce.Domain.Quote.Model;
using dataModel = VirtoCommerce.QuoteModule.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Data.Converters
{
	public static class QuoteItemConverter
	{
		public static coreModel.QuoteItem ToCoreModel(this dataModel.QuoteItemEntity dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.QuoteItem();
			retVal.InjectFrom(dbEntity);

			retVal.ProposalPrices = dbEntity.ProposalPrices.Select(x => x.ToCoreModel()).ToList();

			return retVal;
		}


		public static dataModel.QuoteItemEntity ToDataModel(this coreModel.QuoteItem quoteItem)
		{
			if (quoteItem == null)
				throw new ArgumentNullException("quoteItem");

			var retVal = new dataModel.QuoteItemEntity();
			retVal.InjectFrom(quoteItem);

			if (quoteItem.ProposalPrices != null)
			{
				retVal.ProposalPrices = new ObservableCollection<dataModel.TierPriceEntity>(quoteItem.ProposalPrices.Select(x => x.ToDataModel()));
			}

			return retVal;
		}

		public static void Patch(this dataModel.QuoteItemEntity source, dataModel.QuoteItemEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			if (!source.ProposalPrices.IsNullCollection())
			{
                var tierPriceComparer = AnonymousComparer.Create((dataModel.TierPriceEntity x) => x.Quantity + "-" + x.Quantity);
                source.ProposalPrices.Patch(target.ProposalPrices, tierPriceComparer, (sourceTierPrice, targetTierPrice) => { return; });
			}
		}

	}
}

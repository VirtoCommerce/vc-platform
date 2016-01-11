using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreModel = VirtoCommerce.Domain.Quote.Model;
using dataModel = VirtoCommerce.QuoteModule.Data.Model;
using cartCoreModel = VirtoCommerce.Domain.Cart.Model;
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
            retVal.Currency = dbEntity.Currency;
            retVal.ProposalPrices = dbEntity.ProposalPrices.Select(x => x.ToCoreModel()).ToList();

			return retVal;
		}

        public static cartCoreModel.LineItem ToCartModel(this coreModel.QuoteItem quoteItem)
        {
            var retVal = new cartCoreModel.LineItem();
            retVal.InjectFrom(quoteItem);
            retVal.Sku = quoteItem.Sku;
            if (quoteItem.SelectedTierPrice != null)
            {
                retVal.PlacedPrice = quoteItem.SelectedTierPrice.Price;
                retVal.Quantity = (int)quoteItem.SelectedTierPrice.Quantity;
             }
            return retVal;
        }

        public static dataModel.QuoteItemEntity ToDataModel(this coreModel.QuoteItem quoteItem, PrimaryKeyResolvingMap pkMap)
		{
			if (quoteItem == null)
				throw new ArgumentNullException("quoteItem");

			var retVal = new dataModel.QuoteItemEntity();
            pkMap.AddPair(quoteItem, retVal);
            retVal.InjectFrom(quoteItem);
            retVal.Currency = quoteItem.Currency.ToString();
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

            var patchInjection = new PatchInjection<dataModel.QuoteItemEntity>(x => x.Comment);
            target.InjectFrom(patchInjection, source);

            if (!source.ProposalPrices.IsNullCollection())
			{
                var tierPriceComparer = AnonymousComparer.Create((dataModel.TierPriceEntity x) => x.Quantity + "-" + x.Price);
                source.ProposalPrices.Patch(target.ProposalPrices, tierPriceComparer, (sourceTierPrice, targetTierPrice) => { return; });
			}
		}

	}
}

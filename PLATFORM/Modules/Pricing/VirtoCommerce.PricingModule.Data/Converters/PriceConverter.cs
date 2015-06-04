using System;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using dataModel = VirtoCommerce.PricingModule.Data.Model;

namespace VirtoCommerce.PricingModule.Data.Converters
{
    public static class PriceConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public static coreModel.Price ToCoreModel(this dataModel.Price dbEntity)
        {
            if (dbEntity == null)
                throw new ArgumentNullException("dbEntity");

            var retVal = new coreModel.Price();

            retVal.InjectFrom(dbEntity);

            retVal.ProductId = dbEntity.ProductId;
            retVal.MinQuantity = (int)dbEntity.MinQuantity;

            if (dbEntity.Pricelist != null)
            {
                retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), dbEntity.Pricelist.Currency);
            }

            return retVal;

        }


        public static dataModel.Price ToDataModel(this coreModel.Price price)
        {
            if (price == null)
                throw new ArgumentNullException("price");

            var retVal = new dataModel.Price();

            retVal.InjectFrom(price);
            retVal.ProductId = price.ProductId;
            retVal.MinQuantity = price.MinQuantity;
            return retVal;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.Price source, dataModel.Price target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            var patchInjection = new PatchInjection<dataModel.Price>(x => x.ProductId, x => x.List,
                                                                           x => x.MinQuantity, x => x.Sale);
            target.InjectFrom(patchInjection, source);
        }


    }
}

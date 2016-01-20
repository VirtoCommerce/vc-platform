using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreModel = VirtoCommerce.Domain.Quote.Model;
using cartCoreModel = VirtoCommerce.Domain.Cart.Model;
using taxCoreModel = VirtoCommerce.Domain.Tax.Model;
using dataModel = VirtoCommerce.QuoteModule.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Data.Converters
{
	public static class QuoteRequestConverter
	{
		public static coreModel.QuoteRequest ToCoreModel(this dataModel.QuoteRequestEntity dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.QuoteRequest();
			retVal.InjectFrom(dbEntity);
            if (dbEntity.ShipmentMethodCode != null)
            {
                retVal.ShipmentMethod = new Domain.Quote.Model.ShipmentMethod
                {
                    OptionName = dbEntity.ShipmentMethodOption,
                    ShipmentMethodCode = dbEntity.ShipmentMethodCode
                };
            }
            retVal.Currency = dbEntity.Currency;
            retVal.Addresses = dbEntity.Addresses.Select(x => x.ToCoreModel()).ToList();
			retVal.Attachments = dbEntity.Attachments.Select(x => x.ToCoreModel()).ToList();
			retVal.Items = dbEntity.Items.Select(x => x.ToCoreModel()).ToList();
			
			return retVal;
		}

        public static taxCoreModel.TaxEvaluationContext ToTaxEvalContext(this coreModel.QuoteRequest quoteRequest)
        {
            var retVal = new taxCoreModel.TaxEvaluationContext();
            retVal.Id = quoteRequest.Id;
            retVal.Code = quoteRequest.Number;
            retVal.Currency = quoteRequest.Currency;
            retVal.Address = quoteRequest.Addresses != null ? quoteRequest.Addresses.FirstOrDefault() : null;
            retVal.Type = quoteRequest.GetType().Name;
            foreach(var quoteItem in quoteRequest.Items)
            {
                var line = new taxCoreModel.TaxLine
                {
                    Id = quoteItem.Id,
                    Code = quoteItem.Sku,
                    Name = quoteItem.Name,
                    TaxType = quoteItem.TaxType,
                    Amount = quoteItem.SelectedTierPrice.Price * quoteItem.SelectedTierPrice.Quantity
                };
                retVal.Lines.Add(line);
            }
            return retVal;
        }

        public static cartCoreModel.ShoppingCart ToCartModel(this coreModel.QuoteRequest quoteRequest)
        {
            var retVal = new cartCoreModel.ShoppingCart();
            retVal.InjectFrom(quoteRequest);
            retVal.Currency = quoteRequest.Currency;
            if (quoteRequest.Items != null)
            {
                retVal.Items = quoteRequest.Items.Select(x => x.ToCartModel()).ToList();
            }

            if(quoteRequest.ShipmentMethod != null)
            {
                var shipment = new cartCoreModel.Shipment
                {
                    Currency = quoteRequest.Currency,
                    ShipmentMethodCode = quoteRequest.ShipmentMethod.ShipmentMethodCode,
                    ShipmentMethodOption = quoteRequest.ShipmentMethod.OptionName                     
                };
                retVal.Shipments = new List<cartCoreModel.Shipment>(new[] { shipment });
            }

            return retVal;
        }

        public static dataModel.QuoteRequestEntity ToDataModel(this coreModel.QuoteRequest quoteRequest, PrimaryKeyResolvingMap pkMap)
		{
			if (quoteRequest == null)
				throw new ArgumentNullException("quoteRequest");

			var retVal = new dataModel.QuoteRequestEntity();
            pkMap.AddPair(quoteRequest, retVal);
            retVal.InjectFrom(quoteRequest);

			if (quoteRequest.ShipmentMethod != null)
			{
				retVal.ShipmentMethodCode = quoteRequest.ShipmentMethod.ShipmentMethodCode;
				retVal.ShipmentMethodOption = quoteRequest.ShipmentMethod.OptionName;
			}
            retVal.Currency = quoteRequest.Currency.ToString();
            if (quoteRequest.Addresses != null)
			{
				retVal.Addresses = new ObservableCollection<dataModel.AddressEntity>(quoteRequest.Addresses.Select(x => x.ToDataModel()));
			}
			if (quoteRequest.Attachments != null)
			{
				retVal.Attachments = new ObservableCollection<dataModel.AttachmentEntity>(quoteRequest.Attachments.Select(x => x.ToDataModel()));
			}
			if (quoteRequest.Items != null)
			{
				retVal.Items = new ObservableCollection<dataModel.QuoteItemEntity>(quoteRequest.Items.Select(x => x.ToDataModel(pkMap)));
			}
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.QuoteRequestEntity source, dataModel.QuoteRequestEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

            target.Tag = source.Tag;
			var patchInjection = new PatchInjection<dataModel.QuoteRequestEntity>(x => x.CancelledDate, x => x.CancelReason, x => x.ChannelId, x => x.Comment, x => x.InnerComment,
																				  x => x.IsLocked, x => x.IsCancelled, x => x.LanguageCode, x => x.OrganizationId, x => x.OrganizationName, x => x.ReminderDate,
																				  x => x.Status, x => x.StoreId, x => x.StoreName, x => x.ShipmentMethodCode, x => x.ShipmentMethodOption, x => x.EmployeeId, x => x.EmployeeName, 
                                                                                  x => x.Currency, x=>x.ManualShippingTotal, x=>x.ManualRelDiscountAmount, x=>x.ManualSubTotal, x=>x.IsSubmitted, x=>x.CustomerId, x=>x.CustomerName);
			target.InjectFrom(patchInjection, source);

			if (!source.Addresses.IsNullCollection())
			{
				source.Addresses.Patch(target.Addresses, (sourceAddress, targetAddress) => { return; } );
			}
			if (!source.Attachments.IsNullCollection())
			{
				source.Attachments.Patch(target.Attachments, (sourceAttachment, targetAttachment) => { return; } );
			}
			if (!source.Items.IsNullCollection())
			{
				source.Items.Patch(target.Items, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}
		}


	}
}

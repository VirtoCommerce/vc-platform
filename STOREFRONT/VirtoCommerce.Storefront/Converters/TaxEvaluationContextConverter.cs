using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Converters
{
    public static class TaxEvaluationContextConverter
    {
        public static VirtoCommerceDomainTaxModelTaxLine[] ToListAndSaleTaxLines(this Product product)
        {
            var retVal = new List<VirtoCommerceDomainTaxModelTaxLine>();
            retVal.Add(new VirtoCommerceDomainTaxModelTaxLine
            {
                Id = product.Id,
                Code = "list",
                Name = product.Name,
                TaxType = product.TaxType,
                Amount = (double)product.Price.ListPrice.Amount
            });
            //Need generate two tax line for List and Sale price to have tax amount for list price also
            if (product.Price.SalePrice != product.Price.ListPrice)
            {
                retVal.Add(new VirtoCommerceDomainTaxModelTaxLine
                {
                    Id = product.Id,
                    Code = "sale",
                    Name = product.Name,
                    TaxType = product.TaxType,
                    Amount = (double)product.Price.SalePrice.Amount
                });
            }
            //Need generate tax line for each tier price
            foreach (var tierPrice in product.Price.TierPrices)
            {
                retVal.Add(new VirtoCommerceDomainTaxModelTaxLine
                {
                    Id = product.Id,
                    Code = tierPrice.Quantity.ToString(),
                    Name = product.Name,
                    TaxType = product.TaxType,
                    Amount = (double)tierPrice.Price.Amount
                });
            }
            return retVal.ToArray();
        }

        public static VirtoCommerceDomainTaxModelTaxEvaluationContext ToTaxEvaluationContext(this WorkContext workContext, IEnumerable<Product> products = null)
        {
            var retVal = new VirtoCommerceDomainTaxModelTaxEvaluationContext();
            retVal.Id = workContext.CurrentStore.Id;
            retVal.Currency = workContext.CurrentCurrency.Code;
            retVal.Type = "";
            retVal.Customer = new VirtoCommerceDomainCustomerModelContact
            {
                Id = workContext.CurrentCustomer.Id,
                Name = workContext.CurrentCustomer.UserName
            };
            if(products != null)
            {
                retVal.Lines = products.SelectMany(x => x.ToListAndSaleTaxLines()).ToList();
            }
            return retVal;
        }

        public static VirtoCommerceDomainTaxModelTaxEvaluationContext ToTaxEvalContext(this ShoppingCart cart)
        {
            var retVal = new VirtoCommerceDomainTaxModelTaxEvaluationContext();
            retVal.Id = cart.Id;
            retVal.Code = cart.Name;
            retVal.Currency = cart.Currency.Code;
            retVal.Type = "Cart";
            retVal.Lines = new System.Collections.Generic.List<VirtoCommerceDomainTaxModelTaxLine>();
            foreach (var lineItem in cart.Items)
            {
                var extendedTaxLine = new VirtoCommerceDomainTaxModelTaxLine
                {
                    Id = lineItem.Id,
                    Code = "extended",
                    Name = lineItem.Name,
                    TaxType = lineItem.TaxType,
                    Amount = (double)lineItem.ExtendedPrice.Amount
                };
                retVal.Lines.Add(extendedTaxLine);

                var listTaxLine = new VirtoCommerceDomainTaxModelTaxLine
                {
                    Id = lineItem.Id,
                    Code = "list",
                    Name = lineItem.Name,
                    TaxType = lineItem.TaxType,
                    Amount = (double)lineItem.ListPrice.Amount
                };
                retVal.Lines.Add(listTaxLine);

                if (lineItem.ListPrice != lineItem.SalePrice)
                {
                    var saleTaxLine = new VirtoCommerceDomainTaxModelTaxLine
                    {
                        Id = lineItem.Id,
                        Code = "sale",
                        Name = lineItem.Name,
                        TaxType = lineItem.TaxType,
                        Amount = (double)lineItem.SalePrice.Amount
                    };
                    retVal.Lines.Add(saleTaxLine);
                }
              
            }
            foreach (var shipment in cart.Shipments)
            {
                var totalTaxLine = new VirtoCommerceDomainTaxModelTaxLine
                {
                    Id = shipment.Id,
                    Code = "total",
                    Name = shipment.ShipmentMethodCode,
                    TaxType = shipment.TaxType,
                    Amount = (double)shipment.Total.Amount
                };
                retVal.Lines.Add(totalTaxLine);
                var priceTaxLine = new VirtoCommerceDomainTaxModelTaxLine
                {
                    Id = shipment.Id,
                    Code = "price",
                    Name = shipment.ShipmentMethodCode,
                    TaxType = shipment.TaxType,
                    Amount = (double)shipment.ShippingPrice.Amount
                };
                retVal.Lines.Add(priceTaxLine);
            }
            return retVal;
        }

    }
}
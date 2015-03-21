using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Model.Taxes;
using VirtoCommerce.Foundation.Orders.Repositories;

namespace VirtoCommerce.OrderWorkflow
{
	public class CalculateTaxActivity : OrderActivityBase
	{
		ICatalogRepository _catalogRepository;
		protected ICatalogRepository CatalogRepository
		{
			get { return _catalogRepository ?? (_catalogRepository = ServiceLocator.GetInstance<ICatalogRepository>()); }
			set
			{
				_catalogRepository = value;
			}
		}

		ITaxRepository _taxRepository;
		protected ITaxRepository TaxRepository
		{
			get { return _taxRepository ?? (_taxRepository = ServiceLocator.GetInstance<ITaxRepository>()); }
			set
			{
				_taxRepository = value;
			}
		}

		ICacheRepository _cacheRepository;
		protected ICacheRepository CacheRepository
		{
			get { return _cacheRepository ?? (_cacheRepository = ServiceLocator.GetInstance<ICacheRepository>()); }
			set
			{
				_cacheRepository = value;
			}
		}

		public CalculateTaxActivity()
		{
		}

		public CalculateTaxActivity(ICatalogRepository catalogRepository, ITaxRepository taxRepository, ICacheRepository cacheRepository)
		{
			_catalogRepository = catalogRepository;
			_taxRepository = taxRepository;
			_cacheRepository = cacheRepository;
		}

		protected override void Execute(System.Activities.CodeActivityContext context)
		{
			base.Execute(context);

			CalculateTaxes(CurrentOrderGroup);
		}

		private void CalculateTaxes(OrderGroup order)
		{
			var catalogHelper = new CatalogClient(CatalogRepository, null, null, CacheRepository, null);

			foreach (var form in order.OrderForms)
			{
				decimal totalTaxes = 0;

			    foreach (var shipment in form.Shipments)
				{
					decimal itemTax = 0;
					decimal shippingTax = 0;
					foreach (var shipItem in shipment.ShipmentItems)
					{
                        var lineItem = shipItem.LineItem;

                        if (lineItem == null)
						{
							continue;
						}

						decimal lineItemTaxTotal = 0m;

						// Try getting an address
                        var address = GetAddressByName(order, shipment.ShippingAddressId);
						if (address != null) // no taxes if there is no address
						{
							// Try getting an item
                            var item = catalogHelper.GetItem(lineItem.CatalogItemId);
							if (item != null) // no entry, no tax category, no tax
							{
								var taxCategory = item.TaxCategory;
								// calls the method that returns all the tax values
								var taxes = GetTaxes(taxCategory, Thread.CurrentThread.CurrentCulture.Name, address.CountryCode, address.StateProvince, address.PostalCode, address.RegionName, string.Empty, string.Empty, address.City);

								if (taxes != null && taxes.Any())
								{
									foreach (var tax in taxes)
									{
										if (tax != null)
										{
                                            var taxAmount = Math.Round(lineItem.ExtendedPrice * (tax.Percentage / 100), 2);
											if (tax.Tax.TaxType == (int) TaxTypes.SalesTax)
											{
												itemTax += taxAmount;
												lineItemTaxTotal += taxAmount;
												totalTaxes += taxAmount;
											}
											else if (tax.Tax.TaxType == (int) TaxTypes.ShippingTax)
											{
                                                shippingTax += Math.Round(shipment.ShippingCost * (tax.Percentage / 100), 2);
												totalTaxes += shippingTax;
											}
										}
									}
								}
							}
						}
                        lineItem.TaxTotal = lineItemTaxTotal;
					}

					//TODO Round taxes to money
					shipment.ItemTaxTotal = itemTax;
					shipment.ShippingTaxTotal = shippingTax;
				}
				form.TaxTotal = totalTaxes;
			}
			order.TaxTotal = order.OrderForms.Sum(x=>x.TaxTotal);
		}

		private OrderAddress GetAddressByName(OrderGroup orderGroup, string shipmentAddressId)
		{
            return orderGroup.OrderAddresses
				.SingleOrDefault(a => a.OrderAddressId == shipmentAddressId);
		}

		//aa: regionCode parameter stands for County in the Jurisdiction model. shipping address RegionName is passed in this parameter.
		private TaxValue[] GetTaxes(string taxCategory, string cultureName, string countryCode, string state,
		                            string postalCode, string regionCode, string district, string geoCode, string city)
		{
			TaxValue[] retVal = null;

			var temp = TaxRepository.Taxes
			                        .Expand("TaxValues/JurisdictionGroup/JurisdictionRelations/Jurisdiction")
			                        .SelectMany(tax => tax.TaxValues).Where(taxValue =>
			                                                                taxValue.TaxCategory == taxCategory &&
			                                                                (taxValue.AffectiveDate == null ||
			                                                                 taxValue.AffectiveDate < DateTime.UtcNow) &&
			                                                                (taxValue.JurisdictionGroup.JurisdictionType ==
			                                                                 (int) JurisdictionTypes.Taxes ||
			                                                                 taxValue.JurisdictionGroup.JurisdictionType ==
			                                                                 (int) JurisdictionTypes.All) &&
			                                                                taxValue.JurisdictionGroup.JurisdictionRelations.Any(
				                                                                j => j.Jurisdiction.CountryCode == countryCode));
			var t = temp.Expand("JurisdictionGroup/JurisdictionRelations/Jurisdiction").Expand("Tax").ToList();
			if (t.Any())
			{
				var list = new List<TaxValue>();
				t.GroupBy(val => val.TaxId)
				 .ToList()
				 .ForEach(group => list.Add(GetTaxValue(group, countryCode, state, postalCode, regionCode, district, geoCode, city)));
				if (list.Any())
					retVal = list.ToArray();
			}

			return retVal;
		}

		private TaxValue GetTaxValue(IEnumerable<TaxValue> values, string countryCode, string state, string postalCode, string regionCode, string district, string geoCode, string city)
		{
			var priority = 100;
			TaxValue retVal = null;
			values.ToList().ForEach(taxValue =>
				{
					//check if can find tax value by geo code
					if (!string.IsNullOrEmpty(geoCode))
					{
                        if (taxValue.JurisdictionGroup.JurisdictionRelations.Any(j => string.Equals(j.Jurisdiction.GeoCode, geoCode) && j.Jurisdiction.CheckAllFieldsMatch(countryCode, state, postalCode, regionCode, district, geoCode, city)))
						{
							retVal = taxValue;
							priority = 0;
						}
					}

					//if not found, check if can find tax value by zip code and country code
					if (!string.IsNullOrEmpty(postalCode) && !string.IsNullOrEmpty(countryCode) && priority > 1)
					{
						if (taxValue.JurisdictionGroup.JurisdictionRelations.Any(
								j => string.Equals(j.Jurisdiction.CountryCode, countryCode) &&
									(string.Compare(j.Jurisdiction.ZipPostalCodeStart, postalCode) <= 0 &&
                                        string.Compare(j.Jurisdiction.ZipPostalCodeEnd, postalCode) >= 0) && j.Jurisdiction.CheckAllFieldsMatch(countryCode, state, postalCode, regionCode, district, geoCode, city)
										))
						{
								retVal = taxValue;
								priority = 1;
						}
					}

					//if not found, check if can find by city
					if (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(countryCode) && priority > 2)
					{
						if (taxValue.JurisdictionGroup.JurisdictionRelations.Any(
								j => string.Equals(j.Jurisdiction.CountryCode, countryCode) &&
									//string.Equals(j.Jurisdiction.StateProvinceCode, state) &&
                                    string.Equals(j.Jurisdiction.City, city) && j.Jurisdiction.CheckAllFieldsMatch(countryCode, state, postalCode, regionCode, district, geoCode, city)))
						{
								retVal = taxValue;
								priority = 2;
						}
					}
					
					//if not found, check if can find by district
					if (!string.IsNullOrEmpty(district) && !string.IsNullOrEmpty(countryCode) && priority > 3)
					{
						if (taxValue.JurisdictionGroup.JurisdictionRelations.Any(
								j => string.Equals(j.Jurisdiction.CountryCode, countryCode) &&
                                    string.Equals(j.Jurisdiction.District, district) && j.Jurisdiction.CheckAllFieldsMatch(countryCode, state, postalCode, regionCode, district, geoCode, city)))
						{
								retVal = taxValue;
								priority = 3;
						}
					}

					//if not found, check if can find by county (regionCode)
					if (!string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(countryCode) && priority > 4)
					{
						if (taxValue.JurisdictionGroup.JurisdictionRelations.Any(
								j => string.Equals(j.Jurisdiction.CountryCode, countryCode) &&
                                    string.Equals(j.Jurisdiction.StateProvinceCode, state) && j.Jurisdiction.CheckAllFieldsMatch(countryCode, state, postalCode, regionCode, district, geoCode, city)))
						{
								retVal = taxValue;
								priority = 4;
						}
					}
					
					//if not found, check if can find by country
					if (!string.IsNullOrEmpty(countryCode) && priority > 5)
					{
						if (taxValue.JurisdictionGroup.JurisdictionRelations.Any(
                                j => string.Equals(j.Jurisdiction.CountryCode, countryCode) && j.Jurisdiction.CheckAllFieldsMatch(countryCode, state, postalCode, regionCode, district, geoCode, city)))
						{		
							retVal = taxValue;
							priority = 5;
						}
					}
				});
			return retVal;
		}
	}
}

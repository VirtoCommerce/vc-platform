using System;
using System.Collections.Generic;
using System.Globalization;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.Countries;
using VirtoCommerce.Foundation.Orders.Model.Gateways;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;

namespace VirtoCommerce.Foundation.Data.Orders
{
	public class SqlOrderDatabaseInitializer : SetupDatabaseInitializer<EFOrderRepository, Migrations.Configuration>
	{
		protected override void Seed(EFOrderRepository context)
		{
			CreateCountries(context);
			CreatePaymentMethods(context);
			CreateShippingMethods(context);
			base.Seed(context);
		}

		#region Country/Regions
		private void CreateCountries(EFOrderRepository repository)
		{
			var countries = new List<Country>
				{
					CreateCountry("Aruba", "ABW", 0),
					CreateCountry("Afghanistan", "AFG", 0),
					CreateCountry("Angola", "AGO", 0),
					CreateCountry("Anguilla", "AIA", 0),
					CreateCountry("Albania", "ALB", 0),
					CreateCountry("Andorra", "AND", 0),
					CreateCountry("Netherlands Antilles", "ANT", 0),
					CreateCountry("United Arab Emirates", "ARE", 0),
					CreateCountry("Argentina", "ARG", 0),
					CreateCountry("Armenia", "ARM", 0),
					CreateCountry("American Samoa", "ASM", 0),
					CreateCountry("Antarctica", "ATA", 0),
					CreateCountry("French Southern Territories", "ATF", 0),
					CreateCountry("Antigua and Barbuda", "ATG", 0),
					CreateCountry("Australia", "AUS", 0),
					CreateCountry("Austria", "AUT", 0),
					CreateCountry("Azerbaijan", "AZE", 0),
					CreateCountry("Burundi", "BDI", 0),
					CreateCountry("Belgium", "BEL", 0),
					CreateCountry("Benin", "BEN", 0),
					CreateCountry("Burkina Faso", "BFA", 0),
					CreateCountry("Bangladesh", "BGD", 0),
					CreateCountry("Bulgaria", "BGR", 0),
					CreateCountry("Bahrain", "BHR", 0),
					CreateCountry("Bahamas", "BHS", 0),
					CreateCountry("Bosnia and Herzegovina", "BIH", 0),
					CreateCountry("Belarus", "BLR", 0),
					CreateCountry("Belize", "BLZ", 0),
					CreateCountry("Bermuda", "BMU", 0),
					CreateCountry("Bolivia", "BOL", 0),
					CreateCountry("Brazil", "BRA", 0),
					CreateCountry("Barbados", "BRB", 0),
					CreateCountry("Brunei Darussalam", "BRN", 0),
					CreateCountry("Bhutan", "BTN", 0),
					CreateCountry("Bouvet Island", "BVT", 0),
					CreateCountry("Botswana", "BWA", 0),
					CreateCountry("Central African Republic", "CAF", 0),
					CreateCountry("Canada", "CAN", 0),
					CreateCountry("Cocos (Keeling) Islands", "CCK", 0),
					CreateCountry("Switzerland", "CHE", 0),
					CreateCountry("Chile", "CHL", 0),
					CreateCountry("China", "CHN", 0),
					CreateCountry("Cote D'Ivoire", "CIV", 0),
					CreateCountry("Cameroon", "CMR", 0),
					CreateCountry("Congo, the Democratic Republic of the", "COD", 0),
					CreateCountry("Congo", "COG", 0),
					CreateCountry("Cook Islands", "COK", 0),
					CreateCountry("Colombia", "COL", 0),
					CreateCountry("Comoros", "COM", 0),
					CreateCountry("Cape Verde", "CPV", 0),
					CreateCountry("Costa Rica", "CRI", 0),
					CreateCountry("Cuba", "CUB", 0),
					CreateCountry("Christmas Island", "CXR", 0),
					CreateCountry("Cayman Islands", "CYM", 0),
					CreateCountry("Cyprus", "CYP", 0),
					CreateCountry("Czech Republic", "CZE", 0),
					CreateCountry("Germany", "DEU", 0),
					CreateCountry("Djibouti", "DJI", 0),
					CreateCountry("Dominica", "DMA", 0),
					CreateCountry("Denmark", "DNK", 0),
					CreateCountry("Dominican Republic", "DOM", 0),
					CreateCountry("Algeria", "DZA", 0),
					CreateCountry("Ecuador", "ECU", 0),
					CreateCountry("Egypt", "EGY", 0),
					CreateCountry("Eritrea", "ERI", 0),
					CreateCountry("Western Sahara", "ESH", 0),
					CreateCountry("Spain", "ESP", 0),
					CreateCountry("Estonia", "EST", 0),
					CreateCountry("Ethiopia", "ETH", 0),
					CreateCountry("Finland", "FIN", 0),
					CreateCountry("Fiji", "FJI", 0),
					CreateCountry("Falkland Islands (Malvinas)", "FLK", 0),
					CreateCountry("France", "FRA", 0),
					CreateCountry("Faroe Islands", "FRO", 0),
					CreateCountry("Micronesia, Federated States of", "FSM", 0),
					CreateCountry("Gabon", "GAB", 0),
					CreateCountry("United Kingdom", "GBR", 0),
					CreateCountry("Georgia", "GEO", 0),
					CreateCountry("Ghana", "GHA", 0),
					CreateCountry("Gibraltar", "GIB", 0),
					CreateCountry("Guinea", "GIN", 0),
					CreateCountry("Guadeloupe", "GLP", 0),
					CreateCountry("Gambia", "GMB", 0),
					CreateCountry("Guinea-Bissau", "GNB", 0),
					CreateCountry("Equatorial Guinea", "GNQ", 0),
					CreateCountry("Greece", "GRC", 0),
					CreateCountry("Grenada", "GRD", 0),
					CreateCountry("Greenland", "GRL", 0),
					CreateCountry("Guatemala", "GTM", 0),
					CreateCountry("French Guiana", "GUF", 0),
					CreateCountry("Guam", "GUM", 0),
					CreateCountry("Guyana", "GUY", 0),
					CreateCountry("Hong Kong", "HKG", 0),
					CreateCountry("Heard Island and Mcdonald Islands", "HMD", 0),
					CreateCountry("Honduras", "HND", 0),
					CreateCountry("Croatia", "HRV", 0),
					CreateCountry("Haiti", "HTI", 0),
					CreateCountry("Hungary", "HUN", 0),
					CreateCountry("Indonesia", "IDN", 0),
					CreateCountry("India", "IND", 0),
					CreateCountry("British Indian Ocean Territory", "IOT", 0),
					CreateCountry("Ireland", "IRL", 0),
					CreateCountry("Iran, Islamic Republic of", "IRN", 0),
					CreateCountry("Iraq", "IRQ", 0),
					CreateCountry("Iceland", "ISL", 0),
					CreateCountry("Israel", "ISR", 0),
					CreateCountry("Italy", "ITA", 0),
					CreateCountry("Jamaica", "JAM", 0),
					CreateCountry("Jordan", "JOR", 0),
					CreateCountry("Japan", "JPN", 0),
					CreateCountry("Kazakhstan", "KAZ", 0),
					CreateCountry("Kenya", "KEN", 0),
					CreateCountry("Kyrgyzstan", "KGZ", 0),
					CreateCountry("Cambodia", "KHM", 0),
					CreateCountry("Kiribati", "KIR", 0),
					CreateCountry("Saint Kitts and Nevis", "KNA", 0),
					CreateCountry("Korea, Republic of", "KOR", 0),
					CreateCountry("Kuwait", "KWT", 0),
					CreateCountry("Lao People's Democratic Republic", "LAO", 0),
					CreateCountry("Lebanon", "LBN", 0),
					CreateCountry("Liberia", "LBR", 0),
					CreateCountry("Libyan Arab Jamahiriya", "LBY", 0),
					CreateCountry("Saint Lucia", "LCA", 0),
					CreateCountry("Liechtenstein", "LIE", 0),
					CreateCountry("Sri Lanka", "LKA", 0),
					CreateCountry("Lesotho", "LSO", 0),
					CreateCountry("Lithuania", "LTU", 0),
					CreateCountry("Luxembourg", "LUX", 0),
					CreateCountry("Latvia", "LVA", 0),
					CreateCountry("Macao", "MAC", 0),
					CreateCountry("Morocco", "MAR", 0),
					CreateCountry("Monaco", "MCO", 0),
					CreateCountry("Moldova, Republic of", "MDA", 0),
					CreateCountry("Madagascar", "MDG", 0),
					CreateCountry("Maldives", "MDV", 0),
					CreateCountry("Mexico", "MEX", 0),
					CreateCountry("Marshall Islands", "MHL", 0),
					CreateCountry("Macedonia, the Former Yugoslav Republic of", "MKD", 0),
					CreateCountry("Mali", "MLI", 0),
					CreateCountry("Malta", "MLT", 0),
					CreateCountry("Myanmar", "MMR", 0),
					CreateCountry("Mongolia", "MNG", 0),
					CreateCountry("Northern Mariana Islands", "MNP", 0),
					CreateCountry("Mozambique", "MOZ", 0),
					CreateCountry("Mauritania", "MRT", 0),
					CreateCountry("Montserrat", "MSR", 0),
					CreateCountry("Martinique", "MTQ", 0),
					CreateCountry("Mauritius", "MUS", 0),
					CreateCountry("Malawi", "MWI", 0),
					CreateCountry("Malaysia", "MYS", 0),
					CreateCountry("Mayotte", "MYT", 0),
					CreateCountry("Namibia", "NAM", 0),
					CreateCountry("New Caledonia", "NCL", 0),
					CreateCountry("Niger", "NER", 0),
					CreateCountry("Norfolk Island", "NFK", 0),
					CreateCountry("Nigeria", "NGA", 0),
					CreateCountry("Nicaragua", "NIC", 0),
					CreateCountry("Niue", "NIU", 0),
					CreateCountry("Netherlands", "NLD", 0),
					CreateCountry("Norway", "NOR", 0),
					CreateCountry("Nepal", "NPL", 0),
					CreateCountry("Nauru", "NRU", 0),
					CreateCountry("New Zealand", "NZL", 0),
					CreateCountry("Oman", "OMN", 0),
					CreateCountry("Pakistan", "PAK", 0),
					CreateCountry("Panama", "PAN", 0),
					CreateCountry("Pitcairn", "PCN", 0),
					CreateCountry("Peru", "PER", 0),
					CreateCountry("Philippines", "PHL", 0),
					CreateCountry("Palau", "PLW", 0),
					CreateCountry("Papua New Guinea", "PNG", 0),
					CreateCountry("Poland", "POL", 0),
					CreateCountry("Puerto Rico", "PRI", 0),
					CreateCountry("Korea, Democratic People's Republic of", "PRK", 0),
					CreateCountry("Portugal", "PRT", 0),
					CreateCountry("Paraguay", "PRY", 0),
					CreateCountry("Palestinian Territory, Occupied", "PSE", 0),
					CreateCountry("French Polynesia", "PYF", 0),
					CreateCountry("Qatar", "QAT", 0),
					CreateCountry("Reunion", "REU", 0),
					CreateCountry("Romania", "ROM", 0),
					CreateCountry("Russian Federation", "RUS", 0),
					CreateCountry("Rwanda", "RWA", 0),
					CreateCountry("Saudi Arabia", "SAU", 0),
					CreateCountry("Serbia and Montenegro", "SCG", 0),
					CreateCountry("Sudan", "SDN", 0),
					CreateCountry("Senegal", "SEN", 0),
					CreateCountry("Singapore", "SGP", 0),
					CreateCountry("South Georgia and the South Sandwich Islands", "SGS", 0),
					CreateCountry("Saint Helena", "SHN", 0),
					CreateCountry("Svalbard and Jan Mayen", "SJM", 0),
					CreateCountry("Solomon Islands", "SLB", 0),
					CreateCountry("Sierra Leone", "SLE", 0),
					CreateCountry("El Salvador", "SLV", 0),
					CreateCountry("San Marino", "SMR", 0),
					CreateCountry("Somalia", "SOM", 0),
					CreateCountry("Saint Pierre and Miquelon", "SPM", 0),
					CreateCountry("Sao Tome and Principe", "STP", 0),
					CreateCountry("Suriname", "SUR", 0),
					CreateCountry("Slovakia", "SVK", 0),
					CreateCountry("Slovenia", "SVN", 0),
					CreateCountry("Sweden", "SWE", 0),
					CreateCountry("Swaziland", "SWZ", 0),
					CreateCountry("Seychelles", "SYC", 0),
					CreateCountry("Syrian Arab Republic", "SYR", 0),
					CreateCountry("Turks and Caicos Islands", "TCA", 0),
					CreateCountry("Chad", "TCD", 0),
					CreateCountry("Togo", "TGO", 0),
					CreateCountry("Thailand", "THA", 0),
					CreateCountry("Tajikistan", "TJK", 0),
					CreateCountry("Tokelau", "TKL", 0),
					CreateCountry("Turkmenistan", "TKM", 0),
					CreateCountry("Timor-Leste", "TLS", 0),
					CreateCountry("Tonga", "TON", 0),
					CreateCountry("Trinidad and Tobago", "TTO", 0),
					CreateCountry("Tunisia", "TUN", 0),
					CreateCountry("Turkey", "TUR", 0),
					CreateCountry("Tuvalu", "TUV", 0),
					CreateCountry("Taiwan, Province of China", "TWN", 0),
					CreateCountry("Tanzania, United Republic of", "TZA", 0),
					CreateCountry("Uganda", "UGA", 0),
					CreateCountry("Ukraine", "UKR", 0),
					CreateCountry("United States Minor Outlying Islands", "UMI", 0),
					CreateCountry("Uruguay", "URY", 0),
					CreateCountry("United States", "USA", 100),
					CreateCountry("Uzbekistan", "UZB", 0),
					CreateCountry("Holy See (Vatican City State)", "VAT", 0),
					CreateCountry("Saint Vincent and the Grenadines", "VCT", 0),
					CreateCountry("Venezuela", "VEN", 0),
					CreateCountry("Virgin Islands, British", "VGB", 0),
					CreateCountry("Virgin Islands, U.s.", "VIR", 0),
					CreateCountry("Viet Nam", "VNM", 0),
					CreateCountry("Vanuatu", "VUT", 0),
					CreateCountry("Wallis and Futuna", "WLF", 0),
					CreateCountry("Samoa", "WSM", 0),
					CreateCountry("Yemen", "YEM", 0),
					CreateCountry("South Africa", "ZAF", 0),
					CreateCountry("Zambia", "ZMB", 0),
					CreateCountry("Zimbabwe", "ZWE", 0)
				};
			foreach (var country in countries)
			{
				repository.Add(country);
			}
			repository.UnitOfWork.Commit();
		}

		private Region CreateRegion(string name, string id, int priority, string countryId)
		{
			var region = new Region
			{
				RegionId = id,
				CountryId = countryId,
				DisplayName = name,
				Name = name,
				Priority = priority,
				IsVisible = true
			};

			return region;
		}

		private IEnumerable<Region> CreateRegions()
		{
			var list = new List<Region>
				{
					CreateRegion("ALABAMA", "AL", 0, "USA"),
					CreateRegion("ALASKA", "AK", 0, "USA"),
					CreateRegion("AMERICAN SAMOA", "AS", 0, "USA"),
					CreateRegion("ARIZONA", "AZ", 0, "USA"),
					CreateRegion("ARKANSAS", "AR", 0, "USA"),
					CreateRegion("CALIFORNIA", "CA", 0, "USA"),
					CreateRegion("COLORADO", "CO", 0, "USA"),
					CreateRegion("CONNECTICUT", "CT", 0, "USA"),
					CreateRegion("DELAWARE", "DE", 0, "USA"),
					CreateRegion("DISTRICT OF COLUMBIA", "DC", 0, "USA"),
					CreateRegion("FEDERATED STATES OF MICRONESIA", "FM", 0, "USA"),
					CreateRegion("FLORIDA", "FL", 0, "USA"),
					CreateRegion("GEORGIA", "GA", 0, "USA"),
					CreateRegion("GUAM", "GU", 0, "USA"),
					CreateRegion("HAWAII", "HI", 0, "USA"),
					CreateRegion("IDAHO", "ID", 0, "USA"),
					CreateRegion("ILLINOIS", "IL", 0, "USA"),
					CreateRegion("INDIANA", "IN", 0, "USA"),
					CreateRegion("IOWA", "IA", 0, "USA"),
					CreateRegion("KANSAS", "KS", 0, "USA"),
					CreateRegion("KENTUCKY", "KY", 0, "USA"),
					CreateRegion("LOUISIANA", "LA", 0, "USA"),
					CreateRegion("MAINE", "ME", 0, "USA"),
					CreateRegion("MARSHALL ISLANDS", "MH", 0, "USA"),
					CreateRegion("MARYLAND", "MD", 0, "USA"),
					CreateRegion("MASSACHUSETTS", "MA", 0, "USA"),
					CreateRegion("MICHIGAN", "MI", 0, "USA"),
					CreateRegion("MINNESOTA", "MN", 0, "USA"),
					CreateRegion("MISSISSIPPI", "MS", 0, "USA"),
					CreateRegion("MISSOURI", "MO", 0, "USA"),
					CreateRegion("MONTANA", "MT", 0, "USA"),
					CreateRegion("NEBRASKA", "NE", 0, "USA"),
					CreateRegion("NEVADA", "NV", 0, "USA"),
					CreateRegion("NEW HAMPSHIRE", "NH", 0, "USA"),
					CreateRegion("NEW JERSEY", "NJ", 0, "USA"),
					CreateRegion("NEW MEXICO", "NM", 0, "USA"),
					CreateRegion("NEW YORK", "NY", 0, "USA"),
					CreateRegion("NORTH CAROLINA", "NC", 0, "USA"),
					CreateRegion("NORTH DAKOTA", "ND", 0, "USA"),
					CreateRegion("NORTHERN MARIANA ISLANDS", "MP", 0, "USA"),
					CreateRegion("OHIO", "OH", 0, "USA"),
					CreateRegion("OKLAHOMA", "OK", 0, "USA"),
					CreateRegion("OREGON", "OR", 0, "USA"),
					CreateRegion("PALAU", "PW", 0, "USA"),
					CreateRegion("PENNSYLVANIA", "PA", 0, "USA"),
					CreateRegion("PUERTO RICO", "PR", 0, "USA"),
					CreateRegion("RHODE ISLAND", "RI", 0, "USA"),
					CreateRegion("SOUTH CAROLINA", "SC", 0, "USA"),
					CreateRegion("SOUTH DAKOTA", "SD", 0, "USA"),
					CreateRegion("TENNESSEE", "TN", 0, "USA"),
					CreateRegion("TEXAS", "TX", 0, "USA"),
					CreateRegion("UTAH", "UT", 0, "USA"),
					CreateRegion("VERMONT", "VT", 0, "USA"),
					CreateRegion("VIRGIN ISLANDS", "VI", 0, "USA"),
					CreateRegion("VIRGINIA", "VA", 0, "USA"),
					CreateRegion("WASHINGTON", "WA", 0, "USA"),
					CreateRegion("WEST VIRGINIA", "WV", 0, "USA"),
					CreateRegion("WISCONSIN", "WI", 0, "USA"),
					CreateRegion("WYOMING", "WY", 0, "USA"),
					CreateRegion("Armed Forces Africa", "AF", 0, "USA"),
					CreateRegion("Armed Forces Americas (except Canada)", "AA", 0, "USA"),
					CreateRegion("Armed Forces Canada", "AC", 0, "USA"),
					CreateRegion("Armed Forces Europe", "AE", 0, "USA"),
					CreateRegion("Armed Forces Middle East", "AM", 0, "USA"),
					CreateRegion("Armed Forces Pacific", "AP", 0, "USA")
				};
			return list.ToArray();
		}

		private Country CreateCountry(string name, string id, int priority)
		{
			var country = new Country { CountryId = id, DisplayName = name, Name = name, Priority = priority, IsVisible = true };

			if (id == "USA")
			{
				var regions = CreateRegions();
				foreach (var region in regions)
				{
					country.Regions.Add(region);
				}
			}

			return country;
		}

		#endregion

		#region Payments & Shippings
		private void CreateShippingMethods(EFOrderRepository repository)
		{
			var gateways = CreateShippingGateways();
			gateways.ForEach(repository.Add);
			var currency = new[] { "USD", "EUR" };
			var shippingMethods = new List<ShippingMethod>();
			foreach (var curr in currency)
			{
				shippingMethods.Add(new ShippingMethod
				{
					ShippingMethodId = "FreeShipping" + curr,
					Name = "FreeShipping",
					DisplayName = "Free Shipping",
					Description = "Free Shipping",
					Currency = curr,
					BasePrice = 0,
					IsActive = true
				});
				shippingMethods.Add(new ShippingMethod
				{
					ShippingMethodId = "FlatRate" + curr,
					Name = "FlatRate",
					DisplayName = "Flat Rate",
					Description = "Flat Rate",
					Currency = curr,
					BasePrice = 10,
					IsActive = true
				});
				shippingMethods.Add(new ShippingMethod
				{
					ShippingMethodId = "WeightedRate" + curr,
					Name = "WeightedRate",
					DisplayName = "Weighted Rate",
					Description = "Weighted Rate is based on base price and line items weight",
					Currency = curr,
					BasePrice = 8,
					IsActive = true
				});
			}

			var simpleOption = new ShippingOption
			{
				Name = "simple",
				Description = "Shipping option for simple shipping gateway",
				ShippingGateway = gateways[0]
			};

			repository.Add(simpleOption);

			var weightedOption = new ShippingOption
			{
				Name = "weightedOption",
				Description = "Weighted option for weighted shipping gateway",
				ShippingGateway = gateways[1]
			};

			weightedOption.ShippingGatewayPropertyValues.Add(new ShippingGatewayPropertyValue
			{
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
				Name = "UnitWeightPrice",
				ShortTextValue = 0.49M.ToString(CultureInfo.InvariantCulture)
			});

			repository.Add(weightedOption);

			foreach (var sm in shippingMethods)
			{
				if (sm.Name.Equals("FreeShipping") || sm.Name.Equals("FlatRate"))
				{
					simpleOption.ShippingMethods.Add(sm);
				}
				if (sm.Name.Equals("WeightedRate"))
				{
					weightedOption.ShippingMethods.Add(sm);
				}

				var methodLanguage = new ShippingMethodLanguage
				{
					DisplayName = sm.DisplayName,
					LanguageCode = "en-US",
					ShippingMethodId = sm.ShippingMethodId,
				};

				sm.ShippingMethodLanguages.Add(methodLanguage);

				foreach (var pm in repository.PaymentMethods)
				{
					//Add only credit cart payment for weighted shipping
					if (sm.Name.Equals("WeightedRate") && !pm.Name.Equals("CreditCard"))
					{
						continue;
					}
					pm.PaymentMethodShippingMethods.Add(new PaymentMethodShippingMethod
					{
						PaymentMethodId = pm.PaymentMethodId,
						ShippingMethodId = sm.ShippingMethodId
					});
				}
			}

			repository.UnitOfWork.Commit();
		}

		private void CreatePaymentMethods(EFOrderRepository repository)
		{
			var paymentGateways = CreatePaymentGateways();
			paymentGateways.ForEach(repository.Add);

			var paymentMethods = new List<PaymentMethod>
				{
					new PaymentMethod
						{
							Description = "Pay by phone",
							Name = "Phone",
							PaymentGateway = paymentGateways[0],
							IsActive = true
						},
                    new PaymentMethod
						{
							Description = "Paypal",
							Name = "Paypal",
							PaymentGateway = paymentGateways[1],
							IsActive = true
						},
					new PaymentMethod
						{
							Description = "Credit Card",
							Name = "CreditCard",
                            PaymentGateway = paymentGateways[2],
							IsActive = true
						}
				};

			foreach (var pm in paymentMethods)
			{
				repository.Add(pm);

				//Setup test config for Authorize.Net
				if (pm.Name.Equals("CreditCard", StringComparison.OrdinalIgnoreCase))
				{
					pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
					{
						ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
						Name = "MerchantLogin",
						ShortTextValue = "87WmkB7W"
					});
					pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
					{
						ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
						Name = "MerchantPassword",
						ShortTextValue = "8hAuD275892cBFcb"
					});
					pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
					{
						ValueType = GatewayProperty.ValueTypes.Boolean.GetHashCode(),
						Name = "TestMode",
						BooleanValue = true
					});
					//pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
					//{
					//    ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
					//    Name = "GatewayURL",
					//    ShortTextValue = "https://test.authorize.net/gateway/transact.dll"
					//});
				}
				else if (pm.Name.Equals("Paypal", StringComparison.OrdinalIgnoreCase))
				{
					pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
					{
						ValueType = GatewayProperty.ValueTypes.DictionaryKey.GetHashCode(),
						Name = "mode",
						ShortTextValue = "sandbox"
					});
					pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
					{
						ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
						Name = "account1.apiUsername",
						ShortTextValue = "jb-us-seller_api1.paypal.com"
					});
					pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
					{
						ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
						Name = "account1.apiPassword",
						ShortTextValue = "WX4WTU3S8MY44S7F"
					});
					pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
					{
						ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
						Name = "account1.apiSignature",
						ShortTextValue = "AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy"
					});
					pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
					{
						ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
						Name = "account1.applicationId",
						ShortTextValue = "APP-80W284485P519543T"
					});
					pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
					{
						ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
						Name = "URL",
						ShortTextValue = "https://www.sandbox.paypal.com/webscr&cmd={0}"
					});
				}

				var methodLanguage = new PaymentMethodLanguage
				{
					DisplayName = pm.Description,
					LanguageCode = "en-US",
					PaymentMethodId = pm.PaymentMethodId,
				};

				pm.PaymentMethodLanguages.Add(methodLanguage);
			}

			repository.UnitOfWork.Commit();
		}

		private List<ShippingGateway> CreateShippingGateways()
		{
			var gateways = new List<ShippingGateway>
				{
					new ShippingGateway
						{
							ClassType = "VirtoCommerce.Shipping.SimpleShippingGateway, VirtoCommerce.SimpleShippingGateway",
							Name = "SimpleShippingGateway"
						},
                    new ShippingGateway
						{
							ClassType = "VirtoCommerce.Shipping.WeightedShippingGateway, VirtoCommerce.SimpleShippingGateway",
							Name = "WeightedShippingGateway"
						}
				};

			gateways[0].GatewayProperties.Add(new GatewayProperty { DisplayName = "Currency", Name = "Currency", ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode() });
			gateways[0].GatewayProperties.Add(new GatewayProperty { DisplayName = "Include VAT", Name = "IncludeVAT", ValueType = GatewayProperty.ValueTypes.Boolean.GetHashCode() });
			var property = new GatewayProperty { DisplayName = "Dictionary Values", Name = "DictionaryParam", ValueType = GatewayProperty.ValueTypes.DictionaryKey.GetHashCode() };
			property.GatewayPropertyDictionaryValues.Add(new GatewayPropertyDictionaryValue { DisplayName = "parameter 1", Value = "p01" });
			property.GatewayPropertyDictionaryValues.Add(new GatewayPropertyDictionaryValue { DisplayName = "parameter 3", Value = "p03" });
			gateways[0].GatewayProperties.Add(property);

			//TODO need to add decimals
			gateways[1].GatewayProperties.Add(new GatewayProperty { DisplayName = "Unit weight price", Name = "UnitWeightPrice", ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode() });


			return gateways;
		}

		private List<PaymentGateway> CreatePaymentGateways()
		{
			var defaultGateway = new PaymentGateway
			{
				ClassType = "VirtoCommerce.PaymentGateways.DefaultPaymentGateway, VirtoCommerce.PaymentGateways",
				Name = "DefaultPaymentGateway",
				SupportsRecurring = false
			};

			var paymentGateways = new List<PaymentGateway> { defaultGateway };

			var property0 = new GatewayProperty
			{
				DisplayName = "Status",
				Name = "Status",
				ValueType = GatewayProperty.ValueTypes.DictionaryKey.GetHashCode(),
				IsRequired = true,
			};

			property0.GatewayPropertyDictionaryValues.Add(new GatewayPropertyDictionaryValue
			{
				DisplayName = "Idle",
				Value = "Idle"
			});
			property0.GatewayPropertyDictionaryValues.Add(new GatewayPropertyDictionaryValue
			{
				DisplayName = "Processing in progress",
				Value = "InProgress"
			});
			property0.GatewayPropertyDictionaryValues.Add(new GatewayPropertyDictionaryValue
			{
				DisplayName = "Paused",
				Value = "Paused"
			});

			defaultGateway.GatewayProperties.Add(property0);
			defaultGateway.GatewayProperties.Add(new GatewayProperty
			{
				DisplayName = "Allow unencrypted data",
				Name = "IsAllowUnencrypted",
				ValueType = GatewayProperty.ValueTypes.Boolean.GetHashCode()
			});

			defaultGateway.GatewayProperties.Add(new GatewayProperty
			{
				DisplayName = "Secure URL",
				Name = "URL",
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			});

			SetupPaypalGateway(paymentGateways);
			SetupAuthorizeNetGateway(paymentGateways);
			SetuptIchargeGateway(paymentGateways);

			return paymentGateways;
		}

		private class IchargeInfo
		{
			public int TransactionTypes { get; set; }
			public string Code { get; set; }
			public string Name { get; set; }
		}

		private void SetupAuthorizeNetGateway(List<PaymentGateway> gateways)
		{
			var icGateway = new PaymentGateway
			{
				ClassType = "VirtoCommerce.PaymentGateways.AuthorizeNetPaymentGateway, VirtoCommerce.PaymentGateways",
				Name = "Authorize.NET",
				SupportedTransactionTypes = 0x1F,
				SupportsRecurring = true
			};

			icGateway.GatewayProperties.Add(new GatewayProperty
			{
				DisplayName = "Merchant's Gateway login",
				Name = "MerchantLogin",
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
				IsRequired = true,
			});

			icGateway.GatewayProperties.Add(new GatewayProperty
			{
				DisplayName = "Merchant's Gateway password",
				Name = "MerchantPassword",
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
				IsRequired = true,
			});

			icGateway.GatewayProperties.Add(new GatewayProperty
			{
				DisplayName = "Identifies if transaction is in test mode",
				Name = "TestMode",
				ValueType = GatewayProperty.ValueTypes.Boolean.GetHashCode()
			});


			gateways.Add(icGateway);
		}

		private void SetupPaypalGateway(List<PaymentGateway> gateways)
		{
			var payPalGateway = new PaymentGateway
			{
				ClassType = "VirtoCommerce.PaymentGateways.PaypalPaymentGateway, VirtoCommerce.PaymentGateways",
				Name = "PayPal",
				SupportedTransactionTypes = (int)(TransactionType.Sale),
				SupportsRecurring = true
			};

			payPalGateway.GatewayProperties.Add(new GatewayProperty
			{
				DisplayName = "Secure URL",
				Name = "URL",
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			});

			var propertyMode = new GatewayProperty
			{
				DisplayName = "PayPal environment",
				Name = "mode",
				ValueType = GatewayProperty.ValueTypes.DictionaryKey.GetHashCode(),
				IsRequired = true,
			};

			propertyMode.GatewayPropertyDictionaryValues.Add(new GatewayPropertyDictionaryValue
			{
				DisplayName = "Live mode",
				Value = "live"
			});
			propertyMode.GatewayPropertyDictionaryValues.Add(new GatewayPropertyDictionaryValue
			{
				DisplayName = "Sandbox mode",
				Value = "sandbox"
			});

			payPalGateway.GatewayProperties.Add(propertyMode);

			#region Uncomment this if want to override default settings
			//paymentGateways[1].GatewayProperties.Add(new GatewayProperty
			//{
			//    DisplayName = "Connection timeout",
			//    Name = "connectionTimeout",
			//    ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			//});
			//paymentGateways[1].GatewayProperties.Add(new GatewayProperty
			//{
			//    DisplayName = "Request retries",
			//    Name = "requestRetries",
			//    ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			//});
			#endregion
			payPalGateway.GatewayProperties.Add(new GatewayProperty
			{
				DisplayName = "username",
				Name = "account1.apiUsername",
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			});
			payPalGateway.GatewayProperties.Add(new GatewayProperty
			{
				DisplayName = "password",
				Name = "account1.apiPassword",
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			});
			payPalGateway.GatewayProperties.Add(new GatewayProperty
			{
				DisplayName = "signature",
				Name = "account1.apiSignature",
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			});
			payPalGateway.GatewayProperties.Add(new GatewayProperty
			{
				DisplayName = "application id",
				Name = "account1.applicationId",
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			});
			#region Uncomment this if need to setup paypal with certificate
			//paymentGateways[1].GatewayProperties.Add(new GatewayProperty
			//{
			//    DisplayName = "username",
			//    Name = "account2.apiUsername",
			//    ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			//});
			//paymentGateways[1].GatewayProperties.Add(new GatewayProperty
			//{
			//    DisplayName = "password",
			//    Name = "account2.apiPassword",
			//    ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			//});
			//paymentGateways[1].GatewayProperties.Add(new GatewayProperty
			//{
			//    DisplayName = "certificate",
			//    Name = "account2.apiCertificate",
			//    ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			//});
			//paymentGateways[1].GatewayProperties.Add(new GatewayProperty
			//{
			//    DisplayName = "private key password",
			//    Name = "account2.privateKeyPassword",
			//    ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			//});
			//paymentGateways[1].GatewayProperties.Add(new GatewayProperty
			//{
			//    DisplayName = "application id",
			//    Name = "account2.applicationId",
			//    ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
			//});
			#endregion

			gateways.Add(payPalGateway);
		}

		private void SetuptIchargeGateway(List<PaymentGateway> gateways)
		{
			//Commented gateways are no longer in service.
			var ichargeGateways = new List<IchargeInfo>
				{
					//new IchargeInfo { Code="gwNoGateway", Name = "No Gateway"},
					new IchargeInfo { Code="gwAuthorizeNet", Name="Authorize.Net", TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					//new IchargeInfo { Code="gwDPI", Name="DPI Link", TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization)},
					new IchargeInfo { Code="gwEprocessing", Name="eProcessing Transparent Databse Engine", TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwGoRealTime", Name="GoRealTime (Full-pass)", TransactionTypes = (int)(TransactionType.Sale)},
					//new IchargeInfo { Code="gwIBill", Name="IBill Processing Plus", TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit)},
					new IchargeInfo { Code="gwIntellipay", Name="Intellipay ExpertLink", TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization) },
					//new IchargeInfo { Code="gwIOnGate", Name="Iongate",TransactionTypes = (int)(TransactionType.Sale)},
					new IchargeInfo { Code="gwITransact", Name="iTransact RediCharge",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization)},
					new IchargeInfo { Code="gwNetBilling", Name="NetBilling DirectMode",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwPayFlowPro", Name="Verisign PayFlow Pro",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					//new IchargeInfo { Code="gwPayready", Name="Payready Link",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization)},
					//new IchargeInfo { Code="gwViaKlix", Name="NOVA's Viaklix",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Credit)},
					new IchargeInfo { Code="gwUSAePay", Name="USA ePay CGI Transaction Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwPlugNPay", Name="Plug 'n Pay",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwPlanetPayment", Name="Planet Payment iPay",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwMPCS", Name="MPCS",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwRTWare", Name="RTWare",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwECX", Name="ECX",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwBankOfAmerica", Name="Bank of America eStores",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization)},
					new IchargeInfo { Code="gwInnovative", Name="Innovative Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwMerchantAnywhere", Name="Merchant Anywhere",TransactionTypes = (int)(TransactionType.Sale)},
					new IchargeInfo { Code="gwSkipjack", Name="SkipJack", TransactionTypes = (int)(TransactionType.Sale | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwECHOnline", Name="ECHOnline",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit)},
					new IchargeInfo { Code="gw3DSI", Name="3 Delta Systems (3DSI) EC-Linx",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture)},
					new IchargeInfo { Code="gwTrustCommerce", Name="TrustCommerce",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwPSIGate", Name="PSIGate",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture)},
					new IchargeInfo { Code="gwPayFuse", Name="PayFuse",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwPayFlowLink", Name="PayFlowLink",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture)},
					new IchargeInfo { Code="gwOrbital", Name="Paymentech Orbital Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwLinkPoint", Name="LinkPoint",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwMoneris", Name="Moneris eSelect Plus Canada",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwUSight", Name="uSight Gateway Post-Auth",TransactionTypes = (int)(TransactionType.Sale)},
					new IchargeInfo { Code="gwFastTransact", Name="Fast Transact VeloCT",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit)},
					new IchargeInfo { Code="gwNetworkMerchants", Name="NetworkMerchants",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwOgone", Name="Ogone DirectLink",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Credit)},
					//new IchargeInfo { Code="gwEFSNet", Name="Concord EFSNet",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)}, //(Depreciated, use LinkPoint) 
					new IchargeInfo { Code="gwPRIGate", Name="TransFirst Transaction Central Classic",TransactionTypes = (int)(TransactionType.Sale)},
					//new IchargeInfo { Code="gwProtx", Name="Protx",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)}, //(Depreciated, use SagePay (67) instead)
					//new IchargeInfo { Code="gwOptimal", Name="Optimal Payments / FirePay Direct Payment Protocol",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwMerchantPartners", Name="Merchant Partners",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwCyberCash", Name="CyberCash ",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture)},
					new IchargeInfo { Code="gwFirstData", Name="First Data Global Gateway (Linkpoint)",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwYourPay", Name="YourPay",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)}, //(Depreciated, use Linkpoint (42) instead) 
					new IchargeInfo { Code="gwACHPAyments", Name="ACH Payments AGI",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwPaymentsGateway", Name="Payments Gateway AGI",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwCyberSource", Name="Cyber Source SOAP API",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwEway", Name="eWay XML API (Australia)",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwGoEMerchant", Name="goEmerchant XML",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					//new IchargeInfo { Code="gwPayStream", Name="PayStream Web Services (SOAP) Interface (Australia)",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit)},
					new IchargeInfo { Code="gwTransFirst", Name="TransFirst eLink",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwChase", Name="Chase Merchant Services (Linkpoint)",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwPSIGateXML", Name="PSIGate XML Interface",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwNexCommerce", Name="Thompson Merchant Services NexCommerce (iTransact mode)",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization)},
					new IchargeInfo { Code="gwWorldPay", Name="WorldPay Select Junior Invisible",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization)},
					new IchargeInfo { Code="gwTransactionCentral", Name="TransFirst Transaction Central",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwPaygea", Name="Paygea",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit)},
					new IchargeInfo { Code="gwSterling", Name="Sterling SPOT API",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwPayJunction", Name="PayJunction Trinity Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwSECPay", Name="SECPay (United Kingdom) API Solution",TransactionTypes = (int)(TransactionType.Sale)},
					new IchargeInfo { Code="gwPaymentExpress", Name="Payment Express PXPost",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit)},
					new IchargeInfo { Code="gwMyVirtualMerchant", Name="Elavon/NOVA/My Virtual Merchant",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit)},
					new IchargeInfo { Code="gwSagePayments", Name="Sage Payment Solutions",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwSecurePay", Name="SecurePay",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwMonerisUSA", Name="Moneris eSelect Plus USA",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwBeanstream", Name="Beanstream Process Transaction API",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwVerifi", Name="Verifi Direct-Post API",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwSagePay", Name="SagePay Direct",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwMerchantESolutions", Name="Merchant E-Solutions Payment Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwPayLeap", Name="PayLeap Web Services API",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwPayPoint", Name="PayPoint.net",TransactionTypes = (int)(TransactionType.Sale)},
					new IchargeInfo { Code="gwWorldPayXML", Name="Worldpay XML",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwProPay", Name="ProPay Merchant Services API",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit)},
					new IchargeInfo { Code="gwQBMS", Name="Intuit QuickBooks Merchant Services (QBMS)",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwHeartland", Name="Heartland POS Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwLitle", Name="Litle Online Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwBrainTree", Name="BrainTree DirectPost (Server-to-Server) Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwJetPay", Name="JetPay Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwSterlingXML", Name="Sterling XML Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwLandmark", Name="Landmark Flat File HTTPS Post",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwHSBC", Name="HSBC XML API",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwBluePay", Name="BluePay 2.0 Post",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwAdyen", Name="Adyen API Payments",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwBarclay", Name="Barclay XML API",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwPayTrace", Name="PayTrace Payment Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwYKC", Name="YKC Gateway",TransactionTypes = (int)(TransactionType.Sale)},
					new IchargeInfo { Code="gwCyberbit", Name="Cyberbit Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit)},
					new IchargeInfo { Code="gwGoToBilling", Name="GoToBilling Gateway",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwTransNationalBankcard", Name="TransNational Bankcard",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwNetbanx", Name="Netbanx",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit)},
					new IchargeInfo { Code="gwMIT", Name="MIT",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Credit)},
					new IchargeInfo { Code="gwDataCash", Name="DataCash",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwACHFederal", Name="ACH Federal",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
					new IchargeInfo { Code="gwGlobalIris", Name="Global Iris (HSBC)",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
				};

			foreach (var ichargeInfo in ichargeGateways)
			{
				var icGateway = new PaymentGateway
				{
					GatewayId = ichargeInfo.Code,
					ClassType = "VirtoCommerce.PaymentGateways.IchargePaymentGateway, VirtoCommerce.PaymentGateways",
					Name = ichargeInfo.Name,
					SupportsRecurring = false,
					SupportedTransactionTypes = ichargeInfo.TransactionTypes
				};

				icGateway.GatewayProperties.Add(new GatewayProperty
				{
					DisplayName = "Merchant's Gateway login",
					Name = "MerchantLogin",
					ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
					IsRequired = true,
				});

				icGateway.GatewayProperties.Add(new GatewayProperty
				{
					DisplayName = "Merchant's Gateway password",
					Name = "MerchantPassword",
					ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
					IsRequired = true,
				});

				icGateway.GatewayProperties.Add(new GatewayProperty
				{
					DisplayName = "Default URL for a specific Gateway.",
					Name = "GatewayURL",
					ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
					IsRequired = false
				});
				var testMode = new GatewayProperty
				{
					DisplayName = "Identifies if transaction is in test mode",
					Name = "TestMode",
					ValueType = GatewayProperty.ValueTypes.Boolean.GetHashCode()
				};

				switch (ichargeInfo.Code)
				{
					case "gwAuthorizeNet":
					case "gwPlanetPayment":
					case "gwMPCS":
					case "gwRTWare":
					case "gwECX":
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "Extra security key for Authorize.Net's AIM (3.1) protocol.",
							Name = "AIMHashSecret",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						icGateway.GatewayProperties.Add(testMode);
						break;
					//case "gwViaKlix":
					//    icGateway.GatewayProperties.Add(new GatewayProperty
					//    {
					//        DisplayName = "SSL user id",
					//        Name = "ssl_user_id",
					//        ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
					//        IsRequired = true
					//    });
					//    break;
					case "gwBankOfAmerica":
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "HTTP Referer header allowed in your Bank of America store security settings",
							Name = "referer",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						break;
					case "gwInnovative":
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "Test override errors",
							Name = "test_override_errors",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = false
						});
						icGateway.GatewayProperties.Add(testMode);
						break;
					case "gwPayFuse":
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "PayFuse requires this additional merchant property.",
							Name = "MerchantAlias",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = false
						});
						icGateway.GatewayProperties.Add(testMode);
						break;
					case "gwYourPay":
					case "gwFirstData":
					case "gwLinkPoint":
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "SSL Certificate store",
							Name = "SSLCertStore",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "SSL Certificate store",
							Name = "SSLCertStore",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "SSL Certificate subject",
							Name = "SSLCertSubject",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "SSL Certificate encoded",
							Name = "SSLCertEncoded",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						if (ichargeInfo.Code.Equals("gwLinkPoint"))
						{
							icGateway.GatewayProperties.Add(testMode);
						}
						break;
					//case "gwProtx":
					//    icGateway.GatewayProperties.Add(new GatewayProperty
					//    {
					//        DisplayName = "RelatedSecurityKey special fields required for Credit",
					//        Name = "RelatedSecurityKey",
					//        ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
					//        IsRequired = true
					//    });
					//    icGateway.GatewayProperties.Add(new GatewayProperty
					//    {
					//        DisplayName = "RelatedVendorTXCode special fields required for Credit",
					//        Name = "RelatedVendorTXCode",
					//        ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
					//        IsRequired = true
					//    });
					//    icGateway.GatewayProperties.Add(new GatewayProperty
					//    {
					//        DisplayName = "TXAuthNo special fields required for Captures",
					//        Name = "RelatedTXAuthNo",
					//        ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
					//        IsRequired = true
					//    });
					//    break;
					//case "gwOptimal":
					//    icGateway.GatewayProperties.Add(new GatewayProperty
					//    {
					//        DisplayName = "Optimal Gateway also requires an additional account field",
					//        Name = "account",
					//        ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
					//        IsRequired = true
					//    });
					//    break;
					//case "gwPayStream":
					//    icGateway.GatewayProperties.Add(new GatewayProperty
					//    {
					//        DisplayName = "CustomerID",
					//        Name = "CustomerID",
					//        ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
					//        IsRequired = true
					//    });
					//    icGateway.GatewayProperties.Add(new GatewayProperty
					//    {
					//        DisplayName = "ZoneID",
					//        Name = "ZoneID",
					//        ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
					//        IsRequired = true
					//    });
					//    icGateway.GatewayProperties.Add(new GatewayProperty
					//    {
					//        DisplayName = "Username",
					//        Name = "Username",
					//        ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
					//        IsRequired = true
					//    });
					//    break;
					case "gwUSAePay":
					case "gwECHOnline":
					case "gwTrustCommerce":
					case "gwPSIGate":
					case "gwEway":
					case "gwTransFirst":
					case "gwPSIGateXML":
					case "gwWorldPay":
					case "gwPaymentExpress":
					case "gwPayLeap":
					case "gwSterlingXML":
					case "gwHSBC":
					case "gwBluePay":
					case "gwBarclay":
					case "gwPayTrace":
					case "gwGoToBilling":
						icGateway.GatewayProperties.Add(testMode);
						break;
				}

				gateways.Add(icGateway);
			}
		}
		#endregion
	}
}

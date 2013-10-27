using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.Gateways;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;

namespace VirtoCommerce.PowerShell.Orders
{
	public class SqlOrderSampleDatabaseInitializer : SqlOrderDatabaseInitializer
	{
		private readonly string[] _files = new string[] { 
					"Jurisdiction.sql",
					"JurisdictionGroup.sql",
					"JurisdictionRelation.sql",
					"Tax.sql",
					"TaxValue.sql"
		};

		private readonly string[] _customers = new[] 
        {
            "Bauermeister, Denise",
            "Blackwell, Cynthia",
            "Bressler, Linda",
            "Caroompas, John",
            "Clark, Patti",
            "Dangl, Sherrilynne",
            "Desguin, Joel",
            "Dickinson, Kate",
            "Dugan, Kathy",
            "Galloway, Linda",
            "Granger, Deborah",
            "Heeschen, Kristin",
            "Hiber, Miss",
            "Hickson, Dorothy",
            "Hoevatanakul, Narisa",
            "Kwan, Shirley",
            "Lamb, Tricia",
            "LeCours, Kathie",
            "Molstad, Amy",
            "Murdock, Monica",
            "Ntzouras, Andrew"           
        };

		protected override void Seed(EFOrderRepository context)
		{
			base.Seed(context);
			CreatePaymentMethods(context);
			CreateShippingMethods(context);
			CreateOrders(context);
			FillOrdersScripts(context);
		}

		private void CreateShippingMethods(EFOrderRepository repository)
		{
			var gateways = CreateShippingGateways();
			gateways.ForEach(repository.Add);

			var shippingMethods = new List<ShippingMethod>
				{
					new ShippingMethod
						{
							ShippingMethodId = "FreeShipping",
							Name = "FreeShipping",
							DisplayName = "Free Shipping",
							Description = "Free Shipping",
							Currency = "USD",
							BasePrice = 0,
							IsActive = true
						},
					new ShippingMethod
						{
							ShippingMethodId = "FlatRate",
							Name = "FlatRate",
							DisplayName = "Flat Rate",
							Description = "Flat Rate",
							Currency = "USD",
							BasePrice = 10,
							IsActive = true
						}
				};

			var option = new ShippingOption { Name = "default", Description = "Default", ShippingGateway = gateways[0] };
			option.ShippingMethods.Add(shippingMethods[0]);

			var option2 = new ShippingOption { Name = "default2", Description = "Default2", ShippingGateway = gateways[0] };
			option2.ShippingMethods.Add(shippingMethods[1]);

			repository.Add(option);
			repository.Add(option2);

			foreach (var sm in shippingMethods)
			{
				var methodLanguage = new ShippingMethodLanguage
				{
					DisplayName = sm.Description,
					LanguageCode = "en-US",
					ShippingMethodId = sm.ShippingMethodId,
				};

				sm.ShippingMethodLanguages.Add(methodLanguage);

				foreach (var pm in repository.PaymentMethods)
				{
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
							Description = "Paypal",
							Name = "Paypal",
							PaymentGateway = paymentGateways[0],
							IsActive = true
						},
					new PaymentMethod
						{
							Description = "Pay by phone",
							Name = "Phone",
							PaymentGateway = paymentGateways[1],
							IsActive = true
						},
					new PaymentMethod
						{
							Description = "Credit Card",
							Name = "CreditCard",
							IsActive = true
						},
					new PaymentMethod
						{
							Description = "Use contract negotiated credit available for the organization",
							Name = "Credit",
							IsActive = true
							//PaymentGateway = paymentGateways[0]
						}
				};

			foreach (var pm in paymentMethods)
			{
				repository.Add(pm);

				//Setup test config for Authorize.Net
				if (pm.Name == "CreditCard")
				{
					pm.PaymentGateway = paymentGateways.First(pg => pg.GatewayId == "gwAuthorizeNet");
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
					pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
					{
						ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
						Name = "GatewayURL",
						ShortTextValue = "https://test.authorize.net/gateway/transact.dll"
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
						}
				};

			gateways[0].GatewayProperties.Add(new GatewayProperty { DisplayName = "Currency", Name = "Currency", ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode() });
			gateways[0].GatewayProperties.Add(new GatewayProperty { DisplayName = "Include VAT", Name = "IncludeVAT", ValueType = GatewayProperty.ValueTypes.Boolean.GetHashCode() });
			var property = new GatewayProperty { DisplayName = "Dictionary Values", Name = "DictionaryParam", ValueType = GatewayProperty.ValueTypes.DictionaryKey.GetHashCode() };
			property.GatewayPropertyDictionaryValues.Add(new GatewayPropertyDictionaryValue { DisplayName = "parameter 1", Value = "p01" });
			property.GatewayPropertyDictionaryValues.Add(new GatewayPropertyDictionaryValue { DisplayName = "parameter 3", Value = "p03" });
			gateways[0].GatewayProperties.Add(property);
			return gateways;
		}

		private List<PaymentGateway> CreatePaymentGateways()
		{
			var paymentGateways = new List<PaymentGateway>
				{
					new PaymentGateway
						{
							ClassType = "VirtoCommerce.PaymentGateways.DefaultPaymentGateway, VirtoCommerce.PaymentGateways",
							Name = "DefaultPaymentGateway",
							SupportsRecurring = false
						},
					new PaymentGateway
						{
							ClassType = "VirtoCommerce.PaymentGateways.DefaultPaymentGateway, VirtoCommerce.PaymentGateways",
							Name = "Gateway 2",
							SupportsRecurring = true
						}
				};

			paymentGateways.ForEach(x =>
			{
				x.GatewayProperties.Add(new GatewayProperty
				{
					DisplayName = "Secure URL",
					Name = "URL",
					ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode()
				});
				x.GatewayProperties.Add(new GatewayProperty
				{
					DisplayName = "Allow unencrypted data",
					Name = "IsAllowUnencrypted",
					ValueType = GatewayProperty.ValueTypes.Boolean.GetHashCode()
				});
			});

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

			paymentGateways[0].GatewayProperties.Add(property0);

			SetuptIchargeGateway(paymentGateways);

			return paymentGateways;
		}

		private class IchargeInfo
		{
			public int TransactionTypes { get; set; }
			public string Code { get; set; }
			public string Name { get; set; }
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
					new IchargeInfo { Code="gwEFSNet", Name="Concord EFSNet",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)}, //(Depreciated, use LinkPoint) 
					new IchargeInfo { Code="gwPRIGate", Name="TransFirst Transaction Central Classic",TransactionTypes = (int)(TransactionType.Sale)},
					new IchargeInfo { Code="gwProtx", Name="Protx",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)}, //(Depreciated, use SagePay (67) instead)
					new IchargeInfo { Code="gwOptimal", Name="Optimal Payments / FirePay Direct Payment Protocol",TransactionTypes = (int)(TransactionType.Sale | TransactionType.Authorization | TransactionType.Capture | TransactionType.Credit | TransactionType.Void)},
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
					ClassType = "VirtoCommerce.PaymentGateways.ICharge.IchargePaymentGateway, VirtoCommerce.PaymentGateways",
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
					case "gwViaKlix":
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "SSL user id",
							Name = "ssl_user_id",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						break;
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
					case "gwProtx":
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "RelatedSecurityKey special fields required for Credit",
							Name = "RelatedSecurityKey",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "RelatedVendorTXCode special fields required for Credit",
							Name = "RelatedVendorTXCode",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "TXAuthNo special fields required for Captures",
							Name = "RelatedTXAuthNo",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						break;
					case "gwOptimal":
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "Optimal Gateway also requires an additional account field",
							Name = "account",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						break;
					case "gwPayStream":
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "CustomerID",
							Name = "CustomerID",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "ZoneID",
							Name = "ZoneID",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						icGateway.GatewayProperties.Add(new GatewayProperty
						{
							DisplayName = "Username",
							Name = "Username",
							ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
							IsRequired = true
						});
						break;
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

		private void CreateOrders(EFOrderRepository repository)
		{
			var paymentMethods = repository.PaymentMethods.ToList();

			var customerId = 1;
			var rnd = new Random();
			for (var i = 0; i < _customers.Length - 1; i++) // 20 customers, with 10 orders each
			{
				for (var index = 0; index < 10; index++)
				{
					var order = MockOrderBuilder.BuildOrder()
												.WithAddresses()
												.WithPayments(paymentMethods)
												.WithShipment()
												.WithLineItemsCount(2 + rnd.Next(5))
												.WithReturns()
												.WithStatus("InProgress")
												.WithCustomer(customerId.ToString(CultureInfo.InvariantCulture), _customers[i])
												.GetOrder();
					order.StoreId = "SampleStore";
					order.OrderForms[0].Shipments[0].ShippingAddressId = order.OrderAddresses[1].OrderAddressId;

					repository.Add(order);
				}

				repository.UnitOfWork.Commit();

				customerId++;
			}
		}

		private void FillOrdersScripts(EFOrderRepository repository)
		{
			foreach (string file in _files)
			{
				RunCommand(repository, file, "Orders");
			}
		}

	}

	public class MockOrderBuilder
	{
		private readonly Order _order;
		private readonly IOrderEntityFactory _entityFactory;

		private MockOrderBuilder(Order order, IOrderEntityFactory entityFactory)
		{

			_order = order;
			_entityFactory = entityFactory;
			var orderForm = entityFactory.CreateEntityForType(typeof(OrderForm)) as OrderForm;
			orderForm.Name = "default";
			orderForm.OrderGroupId = _order.OrderGroupId;
			_order.OrderForms.Add(orderForm);
		}


		public static MockOrderBuilder BuildOrder()
		{
			return BuildOrder(new OrderEntityFactory());
		}

		public static MockOrderBuilder BuildOrder(IOrderEntityFactory entityFactory)
		{

			var order = (Order)entityFactory.CreateEntityForType(typeof(Order));
			order.Name = "default";
			order.BillingCurrency = "USD";
			order.StoreId = "UK Store";
			order.TrackingNumber = "PO32" + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture).Substring(10, 5);
			order.Total = 123.43m;
			order.HandlingTotal = 1.2m;
			order.TaxTotal = 4.44m;
			order.Subtotal = 124.63m;
			order.ShippingTotal = 10.12m;

			return new MockOrderBuilder(order, entityFactory);
		}


		public MockOrderBuilder WithStatus(string status)
		{
			_order.Status = status;
			return this;
		}

		public MockOrderBuilder WithReturns()
		{
			var rnd = new Random();

			// generate returns for 30% of orders
			if (rnd.Next(10) < 3)
			{
				var rmaRequest = _entityFactory.CreateEntityForType(typeof(RmaRequest)) as RmaRequest;
				rmaRequest.ReturnTotal = 323.21m;
				rmaRequest.RefundAmount = 301.89m;
				rmaRequest.OrderId = _order.OrderGroupId;
				rmaRequest.AuthorizationCode = rmaRequest.RmaRequestId;

				var returnLineItems = _order.OrderForms[0].LineItems.Select(x => x.LineItemId).ToArray();
				var itemStates = ((RmaLineItemState[])Enum.GetValues(typeof(RmaLineItemState))).Select(x => x.ToString()).ToArray();
				//var rmaStatuses = ((RmaRequestStatus[])Enum.GetValues(typeof(RmaRequestStatus))).Select(x => x.ToString()).ToArray();

				var rmaReturnItem = CreateRmaReturnItem(returnLineItems[0], itemStates[rnd.Next(2)], rnd);
				rmaRequest.RmaReturnItems.Add(rmaReturnItem);
				rmaRequest.Status = rmaReturnItem.ItemState == RmaLineItemState.AwaitingReturn.ToString() ? RmaRequestStatus.AwaitingStockReturn.ToString() : RmaRequestStatus.AwaitingCompletion.ToString();

				// 50 %
				if (rnd.Next(2) > 0)
				{
					rmaReturnItem = CreateRmaReturnItem(returnLineItems[1], rmaReturnItem.ItemState, rnd);
					rmaRequest.RmaReturnItems.Add(rmaReturnItem);
				}

				_order.RmaRequests.Add(rmaRequest);
			}

			return this;
		}

		private RmaReturnItem CreateRmaReturnItem(string lineItemId, string itemState, Random rnd)
		{
			var item = _entityFactory.CreateEntityForType(typeof(RmaReturnItem)) as RmaReturnItem;
			item.ItemState = itemState;
			item.ReturnAmount = 10.05m + (decimal)(300 * rnd.NextDouble());
			item.ReturnReason = "Corrupt";

			var rmaLineItem = _entityFactory.CreateEntityForType(typeof(RmaLineItem)) as RmaLineItem;
			rmaLineItem.RmaReturnItemId = item.RmaReturnItemId;
			rmaLineItem.LineItemId = lineItemId;
			rmaLineItem.ReturnQuantity = 1 + rnd.Next(2);
			rmaLineItem.Quantity = 0;
			item.RmaLineItems.Add(rmaLineItem);
			return item;
		}

		public MockOrderBuilder WithAddresses()
		{
			var orderAddresses = new[] { 
							new OrderAddress { FirstName ="New", LastName ="Yourk",  Name="Billing", City = "New Yourk", CountryName="USA", CountryCode="USA", DaytimePhoneNumber="+7 (906) 2121-321", Email="user@mail.com", Line1="str. 113", Line2="bld. 21", PostalCode="323232", StateProvince="WC" },
							new OrderAddress { FirstName ="Los", LastName ="Angeles", Name="Billing", City = "Los Angeles", CountryName="USA", CountryCode="USA", DaytimePhoneNumber="+7 (906) 4444-444", Email="user2@mail.com", Line1="av. 32", Line2="bld. 1", PostalCode="432142", StateProvince="LA" },
							new OrderAddress { FirstName ="Yourk", LastName ="Yourk", Name="Shipping", City = "Yourk", CountryName="USA", CountryCode="USA", DaytimePhoneNumber="+7 (906) 2121-321", Email="user@mail.com", Line1="str. 113", Line2="Pas Juozapa", PostalCode="12100" },
							new OrderAddress { FirstName ="Vilnius", LastName ="Lithuania", Name="Shipping", City = "Vilnius", CountryName="Lithuania", CountryCode="LTU", DaytimePhoneNumber="+370 5 2744-444", Email="user@mail.com", Line1="Laisves pr. 125", PostalCode="54821" }
							};
			foreach (var address in orderAddresses)
			{
				address.OrderGroupId = _order.OrderGroupId;
				_order.OrderAddresses.Add(address);
			}
			return this;
		}

		public MockOrderBuilder WithLineItemsCount(int lineItemsCount)
		{
			foreach (var lineItem in GenerateLineItems(lineItemsCount))
			{
				_order.OrderForms[0].LineItems.Add(lineItem);
				var shipmentsCount = _order.OrderForms[0].Shipments.Count();
				var shipment = _order.OrderForms[0].Shipments[lineItemsCount % shipmentsCount];
				shipment.ShipmentItems.Add(new ShipmentItem { LineItem = lineItem, LineItemId = lineItem.LineItemId, Quantity = lineItem.Quantity, ShipmentId = shipment.ShipmentId });
			}
			return this;
		}

		public MockOrderBuilder WithShipment()
		{
			return WithShipment(_order.OrderForms[0].LineItems.Select(x => x.LineItemId).ToArray());
		}

		public MockOrderBuilder WithShipmentCount(int count)
		{
			var lineItems = _order.OrderForms[0].LineItems.ToArray();
			var lineItemsPerShipmentCount = lineItems.Count() / count;

			for (var i = 0; i < Math.Min(count, lineItems.Length); i++)
			{
				WithShipment(lineItems.Skip(i * lineItemsPerShipmentCount).Take(lineItemsPerShipmentCount).Select(x => x.LineItemId).ToArray());
			}
			return this;
		}

		public MockOrderBuilder WithShipment(string[] lineItemIds)
		{
			var shipment = _entityFactory.CreateEntityForType(typeof(Shipment)) as Shipment;
			shipment.ShippingMethodId = "FreeShipping";
			shipment.ShippingMethodName = "FreeShipping";
			shipment.ShippingCost = 0m;
			shipment.ShippingAddressId = "1";
			shipment.ShipmentTotal = 213.12m;
			shipment.Subtotal = 119;
			shipment.ShippingDiscountAmount = 5.99m;

			foreach (var lineItemId in lineItemIds)
			{
				var lineItem = _order.OrderForms[0].LineItems.FirstOrDefault(x => x.LineItemId == lineItemId);
				var shipmentItem = _entityFactory.CreateEntityForType(typeof(ShipmentItem)) as ShipmentItem;
				shipmentItem.LineItemId = lineItem.LineItemId;
				shipmentItem.Quantity = lineItem.Quantity;

				shipment.ShipmentItems.Add(shipmentItem);
			}
			shipment.ItemSubtotal = 200.12m;
			shipment.ItemTaxTotal = 5.01m;
			shipment.TotalBeforeTax = shipment.ItemSubtotal - 5m;
			shipment.ShippingTaxTotal = 0.35m;

			shipment.OrderFormId = _order.OrderForms[0].OrderFormId;
			_order.OrderForms[0].Shipments.Add(shipment);

			return this;
		}

		public MockOrderBuilder WithPayments(List<PaymentMethod> paymentMethods)
		{
			var pmCreditCard = paymentMethods.First(x => x.Name == "CreditCard");

			var payments = new Payment[] {
											 new CreditCardPayment
												 { 
												 PaymentType = PaymentType.CreditCard.GetHashCode(),
												 CreditCardCustomerName="John Doe", 
												 CreditCardExpirationMonth = 1, 
												 CreditCardExpirationYear = 2016, 
												 CreditCardNumber = "4007000000027",
												 CreditCardType = "VISA",
												 CreditCardSecurityCode = "123",
												 AuthorizationCode = "0",
												 PaymentMethodId  = pmCreditCard.PaymentMethodId, 
												 PaymentMethodName = pmCreditCard.Description, 
												 ValidationCode="000000", 
												 Amount=32.53m,
												 TransactionType = TransactionType.Sale.ToString(),
												 Status = PaymentStatus.Completed.ToString()
											 },
											new CashCardPayment
												{
													PaymentType = PaymentType.CashCard.GetHashCode(), 
													PaymentMethodName="Visa", 
													ValidationCode="RE6211-44", 
													Amount=55.73m,
													TransactionType = TransactionType.Sale.ToString(),
													Status = PaymentStatus.Failed.ToString()
												},
											 new InvoicePayment
												 {
													 PaymentType = PaymentType.Invoice.GetHashCode(), 
													 PaymentMethodName="Credit", 
													 ValidationCode="BE3-21", 
													 Amount=4.53m,
													 TransactionType = TransactionType.Credit.ToString(),
													 Status = PaymentStatus.Completed.ToString()
												 }
										   };
			foreach (var payment in payments)
			{
				payment.OrderFormId = _order.OrderForms[0].OrderFormId;
				_order.OrderForms[0].Payments.Add(payment);
			}

			return this;
		}


		public MockOrderBuilder WithCustomer(string customerId, string name)
		{
			_order.CustomerId = customerId;
			_order.CustomerName = name;
			return this;
		}
		public Order GetOrder()
		{
			return _order;
		}


		public LineItem[] GenerateLineItems(int count)
		{
			var retVal = new List<LineItem>();

			var names = new[] { "Apple 30 GB iPod with Video Playback Black (5th Generation)",
									   "Sony MDR-IF240RK Wireless Headphone System",
									   "Samsung DVD-HD841 Up-Converting DVD Player", 
									   "Apple QuickTake 200 - Digital camera - compact - 0.35 Mpix - supported memory: SM", 
									   "Samsung YP-T9JAB 4 GB Digital Multimedia Player (Black)", 
									   "EFC-1B1NBECXAR Carrying Case for 10.1", 
										"Sony SGPFLS1 Tablet S LCD Protection Sheet", 
										"Galaxy Tab 8.9 3G Android Honeycomb Tablet (16GB, 850/1900 3G)",
										"Samsung Galaxy Tab Gt-p7500 16gb, Wi-fi + 3g Unlocked"};

			var codes = new[] { "v-0239432c",
									   "MDR-IF240RK",
									   "DVD-HD841", 
									   "v-b12223cc2", 
									   "YP-T9JAB0012", 
									   "EFC-1B1NBECXAR", 
										"v-bc22234088d", 
										"v-2112393vd0",
										"v-b85699233c"};

			var rnd = new Random();
			for (var i = 0; i < count; i++)
			{
				var lineItem = _entityFactory.CreateEntityForType(typeof(LineItem)) as LineItem;
				var index = rnd.Next(names.Length);

				lineItem.DisplayName = names[index];
				lineItem.Quantity = 1 + rnd.Next(19);
				lineItem.ListPrice = rnd.Next(200);
				lineItem.PlacedPrice = lineItem.ListPrice;
				lineItem.CatalogItemId = codes[index];
				lineItem.CatalogItemCode = codes[index];
				lineItem.OrderFormId = _order.OrderForms[0].OrderFormId;

				retVal.Add(lineItem);
			}
			return retVal.ToArray();
		}

	}
}

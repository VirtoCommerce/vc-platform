using AvaTax.TaxModule.Web.Converters;
using AvaTax.TaxModule.Web.Logging;
using AvaTaxCalcREST;
using Common.Logging;
using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Tax.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using domainModel = VirtoCommerce.Domain.Commerce.Model;

namespace AvaTax.TaxModule.Web
{
    public class AvaTaxRateProvider : TaxProvider
    {
        private readonly AvalaraLogger _logger;
        private readonly IContactService _customerSearchService;

        public AvaTaxRateProvider()
            : base("AvaTaxRate")
        {
        }

        public AvaTaxRateProvider(IContactService customerService, ILog log, params SettingEntry[] settings)
            : this()
        {
            Settings = settings;
            _logger = new AvalaraLogger(log);
            _customerSearchService = customerService;
        }

        private string AccountNumber
        {
            get
            {
                string retVal = string.Empty;
                var settingAccountNumber = Settings.Where(x => x.Name == "Avalara.Tax.Credentials.AccountNumber").FirstOrDefault();
                if (settingAccountNumber != null)
                {
                    retVal = settingAccountNumber.Value;
                }
                return retVal;
            }
        }

        private string LicenseKey
        {
            get
            {
                string retVal = string.Empty;
                var settingLicenseKey = Settings.Where(x => x.Name == "Avalara.Tax.Credentials.LicenseKey").FirstOrDefault();
                if (settingLicenseKey != null)
                {
                    retVal = settingLicenseKey.Value;
                }
                return retVal;
            }
        }

        private string CompanyCode
        {
            get
            {
                string retVal = string.Empty;
                var settingCompanyCode = Settings.Where(x => x.Name == "Avalara.Tax.Credentials.CompanyCode").FirstOrDefault();
                if (settingCompanyCode != null)
                {
                    retVal = settingCompanyCode.Value;
                }
                return retVal;
            }
        }

        private string ServiceUrl
        {
            get
            {
                string retVal = string.Empty;
                var settingServiceUrl = Settings.Where(x => x.Name == "Avalara.Tax.Credentials.ServiceUrl").FirstOrDefault();
                if (settingServiceUrl != null)
                {
                    retVal = settingServiceUrl.Value;
                }
                return retVal;
            }
        }

        private bool IsEnabled
        {
            get
            {
                bool retVal = false;
                var settingIsEnabled = Settings.Where(x => x.Name == "Avalara.Tax.IsEnabled").FirstOrDefault();
                if (settingIsEnabled != null)
                {
                    retVal = bool.Parse(settingIsEnabled.Value);
                }
                return retVal;
            }
        }

        public override IEnumerable<TaxRate> CalculateRates(IEvaluationContext context)
        {
            var taxEvalContext = context as TaxEvaluationContext;
            if (taxEvalContext == null)
            {
                throw new NullReferenceException("taxEvalContext");
            }

            var retVal = GetTaxRates(taxEvalContext.TaxRequest);
            return retVal;
        }

        public virtual void CalculateCartTax(ShoppingCart cart)
        {
            LogInvoker<AvalaraLogger.TaxRequestContext>.Execute(log =>
            {
                if (IsEnabled && !string.IsNullOrEmpty(AccountNumber) && !string.IsNullOrEmpty(LicenseKey)
                    && !string.IsNullOrEmpty(ServiceUrl)
                    && !string.IsNullOrEmpty(CompanyCode))
                {
                    Contact contact = null;
                    if (cart.CustomerId != null)
                        contact = _customerSearchService.GetById(cart.CustomerId);

                    var request = cart.ToAvaTaxRequest(CompanyCode, contact);
                    if (request != null)
                    {
                        log.docCode = request.DocCode;
                        log.customerCode = request.CustomerCode;
                        log.docType = request.DocType.ToString();
                        log.amount = (double)cart.Total;

                        var taxSvc = new JsonTaxSvc(AccountNumber, LicenseKey, ServiceUrl);
                        var getTaxResult = taxSvc.GetTax(request);

                        if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                        {
                            //if tax calculation failed create exception with provided error info
                            var error = string.Join(Environment.NewLine,
                                getTaxResult.Messages.Select(m => m.Summary));
                            throw new Exception(error);
                        }

                        //reset all cart items taxes
                        if (cart.Items.Any())
                        {
                            cart.Items.ForEach(x =>
                            {
                                x.TaxTotal = 0;
                                x.TaxDetails = null;
                            });
                        }

                        //reset all cart shipments taxes
                        if (cart.Shipments.Any())
                        {
                            cart.Shipments.ForEach(x =>
                            {
                                x.TaxTotal = 0;
                                x.TaxDetails = null;
                            });
                        }

                        foreach (var taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<AvaTaxCalcREST.TaxLine>())
                        {
                            var lineItem = cart.Items.FirstOrDefault(x => x.Id == taxLine.LineNo);
                            if (lineItem != null)
                            {
                                lineItem.TaxTotal = taxLine.Tax;
                                if (taxLine.TaxDetails != null && taxLine.TaxDetails.Any(td => !string.IsNullOrEmpty(td.TaxName)))
                                {
                                    lineItem.TaxDetails =
                                        taxLine.TaxDetails.Where(td => !string.IsNullOrEmpty(td.TaxName)).Select(taxDetail => new domainModel.TaxDetail
                                        {
                                            Amount = taxDetail.Tax,
                                            Name = taxDetail.TaxName,
                                            Rate = taxDetail.Rate
                                        }).ToList();
                                }
                            }
                            else
                            {
                                var shipment = cart.Shipments.FirstOrDefault(s => s.Id != null ? s.Id.Equals(taxLine.LineNo) : s.ShipmentMethodCode.Equals(taxLine.LineNo));
                                if (shipment != null)
                                {
                                    shipment.TaxTotal = taxLine.Tax;
                                    if (taxLine.TaxDetails != null && taxLine.TaxDetails.Any(td => !string.IsNullOrEmpty(td.TaxName)))
                                    {
                                        shipment.TaxDetails =
                                            taxLine.TaxDetails.Where(td => !string.IsNullOrEmpty(td.TaxName)).Select(taxDetail => new domainModel.TaxDetail
                                            {
                                                Amount = taxDetail.Tax,
                                                Name = taxDetail.TaxName,
                                                Rate = taxDetail.Rate
                                            }).ToList();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to create get tax request");
                    }
                }
                else
                {
                    throw new Exception("Tax calculation disabled or credentials not provided");
                }
            })
            .OnError(_logger, AvalaraLogger.EventCodes.TaxCalculationError)
            .OnSuccess(_logger, AvalaraLogger.EventCodes.GetTaxRequestTime);
        }

        public virtual void CalculateOrderTax(CustomerOrder order)
        {
            LogInvoker<AvalaraLogger.TaxRequestContext>.Execute(log =>
            {
                if (IsEnabled && !string.IsNullOrEmpty(AccountNumber)
                    && !string.IsNullOrEmpty(LicenseKey)
                    && !string.IsNullOrEmpty(ServiceUrl)
                    && !string.IsNullOrEmpty(CompanyCode))
                {
                    //if all payments completed commit tax document in avalara
                    var isCommit = order.InPayments != null && order.InPayments.Any()
                        && order.InPayments.All(pi => pi.IsApproved);

                    Contact contact = null;
                    if (order.CustomerId != null)
                        contact = _customerSearchService.GetById(order.CustomerId);

                    var request = order.ToAvaTaxRequest(CompanyCode, contact, isCommit);
                    if (request != null)
                    {
                        log.docCode = request.DocCode;
                        log.docType = request.DocType.ToString();
                        log.customerCode = request.CustomerCode;
                        log.amount = (double)order.Sum;
                        log.isCommit = isCommit;

                        var taxSvc = new JsonTaxSvc(AccountNumber, LicenseKey, ServiceUrl);
                        var getTaxResult = taxSvc.GetTax(request);

                        if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                        {
                            //if tax calculation failed create exception with provided error info
                            var error = string.Join(Environment.NewLine, getTaxResult.Messages.Select(m => m.Summary));
                            throw new Exception(error);
                        }

                        //reset items taxes
                        if (order.Items.Any())
                            order.Items.ForEach(x =>
                            {
                                x.Tax = 0;
                                x.TaxDetails = null;
                            });

                        //reset order shipments taxes
                        if (order.Shipments.Any())
                            order.Shipments.ForEach(x =>
                            {
                                x.Tax = 0;
                                x.TaxDetails = null;
                            });

                        order.Tax = 0;

                        foreach (var taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<AvaTaxCalcREST.TaxLine>())
                        {
                            var lineItem = order.Items.FirstOrDefault(x => x.Id == taxLine.LineNo);
                            if (lineItem != null)
                            {
                                lineItem.Tax = taxLine.Tax;
                                if (taxLine.TaxDetails != null && taxLine.TaxDetails.Any(td => !string.IsNullOrEmpty(td.TaxName)))
                                {
                                    lineItem.TaxDetails =
                                        taxLine.TaxDetails.Where(td => !string.IsNullOrEmpty(td.TaxName)).Select(taxDetail => new domainModel.TaxDetail
                                        {
                                            Amount = taxDetail.Tax,
                                            Name = taxDetail.TaxName,
                                            Rate = taxDetail.Rate
                                        }).ToList();
                                }
                            }
                            else
                            {
                                var shipment = order.Shipments.FirstOrDefault(s => s.Id != null ? s.Id.Equals(taxLine.LineNo) : s.ShipmentMethodCode.Equals(taxLine.LineNo));
                                if (shipment != null)
                                {
                                    shipment.Tax = taxLine.Tax;
                                    if (taxLine.TaxDetails != null && taxLine.TaxDetails.Any(td => !string.IsNullOrEmpty(td.TaxName)))
                                    {
                                        shipment.TaxDetails =
                                            taxLine.TaxDetails.Where(td => !string.IsNullOrEmpty(td.TaxName)).Select(taxDetail => new domainModel.TaxDetail
                                            {
                                                Amount = taxDetail.Tax,
                                                Name = taxDetail.TaxName,
                                                Rate = taxDetail.Rate
                                            }).ToList();
                                    }
                                }
                            }

                            order.Tax = getTaxResult.TotalTax;
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to create get tax request");
                    }
                }
                else
                {
                    throw new Exception("Failed to create get tax request");
                }
            })
                .OnError(_logger, AvalaraLogger.EventCodes.TaxCalculationError)
                .OnSuccess(_logger, AvalaraLogger.EventCodes.GetSalesInvoiceRequestTime);
        }

        public virtual void AdjustOrderTax(CustomerOrder originalOrder, CustomerOrder modifiedOrder)
        {
            LogInvoker<AvalaraLogger.TaxRequestContext>.Execute(log =>
            {
                if (IsEnabled && !string.IsNullOrEmpty(AccountNumber)
                    && !string.IsNullOrEmpty(LicenseKey)
                    && !string.IsNullOrEmpty(ServiceUrl)
                    && !string.IsNullOrEmpty(CompanyCode))
                {
                    //if all payments completed commit tax document in avalara
                    var isCommit = modifiedOrder.InPayments != null && modifiedOrder.InPayments.Any()
                        && modifiedOrder.InPayments.All(pi => pi.IsApproved);

                    Contact contact = null;
                    if (modifiedOrder.CustomerId != null)
                        contact = _customerSearchService.GetById(modifiedOrder.CustomerId);

                    var request = modifiedOrder.ToAvaTaxAdjustmentRequest(CompanyCode, contact, originalOrder, isCommit);
                    if (request != null)
                    {
                        log.docCode = request.ReferenceCode;
                        log.docType = request.DocType.ToString();
                        log.customerCode = request.CustomerCode;
                        log.amount = (double)originalOrder.Sum;

                        var taxSvc = new JsonTaxSvc(AccountNumber, LicenseKey, ServiceUrl);
                        var getTaxResult = taxSvc.GetTax(request);

                        if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                        {
                            var error = string.Join(Environment.NewLine,
                                getTaxResult.Messages.Select(m => m.Summary));
                            throw new Exception(error);
                        }

                        foreach (var taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<AvaTaxCalcREST.TaxLine>())
                        {
                            var lineItem = modifiedOrder.Items.FirstOrDefault(x => x.Id == taxLine.LineNo);
                            if (lineItem != null)
                            {
                                lineItem.Tax += taxLine.Tax;
                                if (taxLine.TaxDetails != null && taxLine.TaxDetails.Any(td => !string.IsNullOrEmpty(td.TaxName)))
                                {

                                    var taxLines =
                                        taxLine.TaxDetails.Where(td => !string.IsNullOrEmpty(td.TaxName)).Select(taxDetail => new domainModel.TaxDetail
                                        {
                                            Amount = taxDetail.Tax,
                                            Name = taxDetail.TaxName,
                                            Rate = taxDetail.Rate
                                        }).ToList();

                                    lineItem.TaxDetails = lineItem.TaxDetails == null ? taxLines : lineItem.TaxDetails.AddRange(taxLines);
                                }
                            }
                        }

                        modifiedOrder.Tax = 0;
                    }
                }
                else
                {
                    throw new Exception("AvaTax credentials not provided or tax calculation disabled");
                }
            })
            .OnError(_logger, AvalaraLogger.EventCodes.TaxCalculationError)
            .OnSuccess(_logger, AvalaraLogger.EventCodes.GetTaxRequestTime);
        }

        public virtual void CancelTaxDocument(CustomerOrder order)
        {
            LogInvoker<AvalaraLogger.TaxRequestContext>.Execute(log =>
            {
                if (IsEnabled && !string.IsNullOrEmpty(AccountNumber) && !string.IsNullOrEmpty(LicenseKey)
                    && !string.IsNullOrEmpty(ServiceUrl)
                    && !string.IsNullOrEmpty(CompanyCode))
                {                    
                    var request = order.ToAvaTaxCancelRequest(CompanyCode, CancelCode.DocDeleted);
                    if (request != null)
                    {
                        log.docCode = request.DocCode;
                        log.docType = request.DocType.ToString();

                        var taxSvc = new JsonTaxSvc(AccountNumber, LicenseKey, ServiceUrl);
                        var getTaxResult = taxSvc.CancelTax(request);

                        if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                        {
                            var error = string.Join(Environment.NewLine, getTaxResult.Messages.Select(m => m.Summary));
                            throw new Exception(error);
                        }
                    }
                }
                else
                {
                    throw new Exception("AvaTax credentials not provided or tax calculation disabled");
                }
            })
            .OnError(_logger, AvalaraLogger.EventCodes.TaxCalculationError)
            .OnSuccess(_logger, AvalaraLogger.EventCodes.GetTaxRequestTime);
        }

        private List<TaxRate> GetTaxRates(TaxRequest taxRequest)
        {
            List<TaxRate> retVal = new List<TaxRate>();
            LogInvoker<AvalaraLogger.TaxRequestContext>.Execute(log =>
            {
                if (IsEnabled && !string.IsNullOrEmpty(AccountNumber)
                    && !string.IsNullOrEmpty(LicenseKey)
                    && !string.IsNullOrEmpty(ServiceUrl)
                    && !string.IsNullOrEmpty(CompanyCode))
                {                    
                    var request = taxRequest.ToAvaTaxRequest(CompanyCode, false);
                    if (request != null)
                    {
                        log.docCode = request.DocCode;
                        log.docType = request.DocType.ToString();
                        log.customerCode = request.CustomerCode;                        

                        var taxSvc = new JsonTaxSvc(AccountNumber, LicenseKey, ServiceUrl);
                        var getTaxResult = taxSvc.GetTax(request);

                        if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                        {
                            //if tax calculation failed create exception with provided error info
                            var error = string.Join(Environment.NewLine, getTaxResult.Messages.Select(m => m.Summary));
                            throw new Exception(error);
                        }
                        
                        foreach (var taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<AvaTaxCalcREST.TaxLine>())
                        {                            
                                var rate = new TaxRate
                                {
                                    Rate = taxLine.Tax,
                                    Currency = taxRequest.Currency,
                                    TaxProvider = this,
                                    Line = taxRequest.Lines.First(l => l.Id == taxLine.LineNo)
                                };
                                retVal.Add(rate);                            
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to create get tax request");
                    }
                }
                else
                {
                    throw new Exception("Failed to create get tax request");
                }
            })
            .OnError(_logger, AvalaraLogger.EventCodes.TaxCalculationError)
            .OnSuccess(_logger, AvalaraLogger.EventCodes.GetSalesInvoiceRequestTime);

            return retVal;
        }
    }
}
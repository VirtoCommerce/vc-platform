using AvaTax.TaxModule.Web.Converters;
using AvaTax.TaxModule.Web.Logging;
using AvaTaxCalcREST;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Tax.Model;
using VirtoCommerce.Platform.Core.Settings;

namespace AvaTax.TaxModule.Web
{
    public class AvaTaxRateProvider : TaxProvider
    {
        private readonly AvalaraLogger _logger;

        public AvaTaxRateProvider()
            : base("AvaTaxRate")
        {
        }

        public AvaTaxRateProvider(ILog log, params SettingEntry[] settings)
            : this()
        {
            Settings = settings;
            _logger = new AvalaraLogger(log);
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
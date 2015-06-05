using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaTax.TaxModule.Web.Services
{
    public class InMemoryTaxManagerImpl: ITaxManager
    {
        private readonly List<ITax> _taxes = new List<ITax>();

        #region ITaxManager Members

        public void RegisterTax(ITax tax)
        {
            if (tax == null)
            {
                throw new ArgumentNullException("tax");
            }
            if (_taxes.Any(x => x.Code == tax.Code))
            {
                throw new OperationCanceledException(tax.Code + " already registered");
            }
            _taxes.Add(tax);
        }

        public IEnumerable<ITax> Taxes
        {
            get { return _taxes.AsReadOnly(); }
        }

        #endregion
    }
}
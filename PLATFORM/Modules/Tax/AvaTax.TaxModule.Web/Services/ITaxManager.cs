using System.Collections.Generic;

namespace AvaTax.TaxModule.Web.Services
{
    public interface ITaxManager
    {
        void RegisterTax(ITax taxModule);
        IEnumerable<ITax> Taxes { get; }
    }
}

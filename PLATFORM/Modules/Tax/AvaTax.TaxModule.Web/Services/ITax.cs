
namespace AvaTax.TaxModule.Web.Services
{
    public interface ITax
    {
        string Username { get; }
        string Password { get; }
        bool IsEnabled { get; }
        string ServiceUrl { get; }
        string CompanyCode { get; }
    }
}

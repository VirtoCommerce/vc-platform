
namespace AvaTax.TaxModule.Web.Services
{
    public interface ITaxSettings
    {
        string Username { get; }
        string Password { get; }
        bool IsEnabled { get; }
        bool IsValidateAddress { get; }
        string ServiceUrl { get; }
        string CompanyCode { get; }
    }
}


namespace AvaTax.TaxModule.Web.Services
{
    public interface ITax
    {
        string Username { get; }
        string Password { get; }
        string Code { get; }
        bool IsEnabled { get; }
        string Description { get; }
        string LogoUrl { get; }
        string ServiceUrl { get; }
        string CompanyCode { get; }
    }
}

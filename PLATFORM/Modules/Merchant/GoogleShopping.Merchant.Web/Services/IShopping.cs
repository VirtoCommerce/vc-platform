
namespace GoogleShopping.MerchantModule.Web.Services
{
    public interface IShopping
    {
        ulong MerchantId { get; }
        string Code { get; }
        string Description { get; }
        string LogoUrl { get; }
    }
}
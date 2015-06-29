
namespace GoogleShopping.MerchantModule.Web.Services
{
    public interface IShoppingSettings
    {
        ulong MerchantId { get; }
        string Code { get; }
        string Description { get; }
        string LogoUrl { get; }
    }
}
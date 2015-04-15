using Google.Apis.ShoppingContent.v2;

namespace GoogleShopping.MerchantModule.Web.Providers
{
    public interface IGoogleContentServiceProvider
    {
        ShoppingContentService GetShoppingContentService();
    }
}

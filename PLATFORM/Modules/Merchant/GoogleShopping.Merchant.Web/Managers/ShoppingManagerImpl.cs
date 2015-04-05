using GoogleShopping.MerchantModule.Web.Services;

namespace GoogleShopping.MerchantModule.Web.Managers
{
    public class ShoppingManagerImpl : IShopping
    {
        private readonly ulong _merchantId;
        private readonly string _code;
        private readonly string _description;
        private readonly string _logoUrl;

        public ShoppingManagerImpl(ulong merchantId, string code, string description, string logoUrl)
        {
            _merchantId = merchantId;
            _code = code;
            _description = description;
            _logoUrl = logoUrl;
        }

        public ulong MerchantId
        {
            get { return _merchantId; }
        }

        public string Code
        {
            get { return _code; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string LogoUrl
        {
            get { return _logoUrl; }
        }
    }
}
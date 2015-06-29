using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace GoogleShopping.MerchantModule.Web.Services
{
    public class ShoppingManagerSettings : IShoppingSettings
    {
        private readonly string _merchantIdPropertyName;
        private readonly string _code;
        private readonly string _description;
        private readonly string _logoUrl;

        private readonly ISettingsManager _settingsManager; 

        public ShoppingManagerSettings(ISettingsManager settingsManager, string merchantIdPropertyName, string code, string description, string logoUrl)
        {
            _merchantIdPropertyName = merchantIdPropertyName;
            _code = code;
            _description = description;
            _logoUrl = logoUrl;

            _settingsManager = settingsManager;
        }

        public ulong MerchantId
        {
            get
            {
                var retVal = _settingsManager.GetValue<ulong>(_merchantIdPropertyName, 0);
                return retVal;
            }
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
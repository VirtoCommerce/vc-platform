using System.Web.Http;
using VirtoCommerce.Platform.Core.Settings;

namespace AvaTax.TaxModule.Web.Controller
{
    [RoutePrefix("api/tax/auth")]
    public class AvalaraAuthorizationController : ApiController
    {
        private readonly ISettingsManager _settingsManager;

        public AvalaraAuthorizationController(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }
    }
}

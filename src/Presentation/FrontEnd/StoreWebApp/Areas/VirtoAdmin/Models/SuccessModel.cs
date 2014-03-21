using VirtoCommerce.Foundation.AppConfig;

namespace VirtoCommerce.Web.Areas.VirtoAdmin.Models
{
    public class SuccessModel
    {
        public string Website { get; set; }

        public string AdminUrl
        {
            get
            {
                return string.Format(AppConfigConfiguration.Instance.Setup.AdminUrl, Website);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Foundation.AppConfig;

namespace VirtoCommerce.Web.Areas.VirtoAdmin.Models
{
    public class SuccessModel
    {
        public string Website { get; set; }

        public string AdminUrl
        {
            get { return AppConfigConfiguration.Instance.Setup.AdminUrl; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Model.Notifications
{
    public class RenderNotificationContentResult
    {
        /// <summary>
        /// Subject 
        /// </summary>
        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Model.Notifications
{
    public class TestNotificationRequest
    {
        public string Type { get; set; }
        public string ObjectId { get; set; }
        public string ObjectTypeId { get; set; }
        public string Language { get; set; }
        public NotificationParameter[] NotificationParameters { get; set; }
    }
}
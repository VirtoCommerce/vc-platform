using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Web.Cors
{
    public class CorsOptions
    {
        public bool? AllowAnyOrigin { get; set; }
        public string[] AllowedOrigins { get; set; }
    }
}

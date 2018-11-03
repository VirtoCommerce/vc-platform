using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using Owin;

namespace VirtoCommerce.Platform.Web
{
    public static class AppBuilderExtension
    {
        /// <summary>
        /// https://www.zpqrtbnk.net/posts/appdomains-threads-cultureinfos-and-paracetamol
        /// </summary>
        /// <param name="app"></param>
        public static void SanitizeThreadCulture(this IAppBuilder app)
        {
            var currentCulture = CultureInfo.CurrentCulture;

            // at the top of any culture should be the invariant culture,
            // find it doing an .Equals comparison ensure that we will
            // find it and not loop endlessly
            var invariantCulture = currentCulture;
            while (invariantCulture.Equals(CultureInfo.InvariantCulture) == false)
                invariantCulture = invariantCulture.Parent;

            if (ReferenceEquals(invariantCulture, CultureInfo.InvariantCulture))
                return;

            var thread = Thread.CurrentThread;
            thread.CurrentCulture = CultureInfo.GetCultureInfo(thread.CurrentCulture.Name);
            thread.CurrentUICulture = CultureInfo.GetCultureInfo(thread.CurrentUICulture.Name);
        }
    }
}
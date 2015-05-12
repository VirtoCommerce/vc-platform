#region
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VirtoCommerce.Web.Models.Services;
using VirtoCommerce.Web.Services;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class PageCollection : ItemCollection<Page>
    {
        #region Constructors and Destructors
        public PageCollection()
            : base(new List<Page>())
        {
        }
        #endregion

        #region Public Methods and Operators
        public override object BeforeMethod(string method)
        {
            var result = this.Root.SingleOrDefault(x => x.Handle == method);

            if (result == null)
            {
                var context = SiteContext.Current;
                var response = new PagesService().GetPage(context, method);
                return response;
            }

            return result;
        }
        #endregion
    }
}
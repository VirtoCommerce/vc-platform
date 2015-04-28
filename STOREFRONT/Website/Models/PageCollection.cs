#region
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using VirtoCommerce.Web.Models.Helpers;
using VirtoCommerce.Web.Models.Services;

#endregion

namespace VirtoCommerce.Web.Models
{

    #region
    #endregion

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
                //var response = Task.Run(() => new PagesService().GetPageAsync(context, method)).Result;
                var response = new PagesService().GetPage(context, method);
                return response;
            }

            return result;
        }
        #endregion
    }
}
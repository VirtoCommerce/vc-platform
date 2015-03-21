#region
using System.Collections.Generic;
using System.Linq;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{

    #region
    #endregion

    public class SubmitForm : Drop
    {
        #region Constructors and Destructors
        public SubmitForm()
        {
            this.Properties = new Dictionary<string, object>();
        }
        #endregion

        #region Public Properties
        public string ActionLink { get; set; }

        public string[] Errors { get; set; }

        public string Id { get; set; }

        public bool PasswordNeeded { get; set; }

        public bool PostedSuccessfully { get; set; }

        public Dictionary<string, object> Properties { get; set; }
        #endregion

        #region Public Methods and Operators
        public override object BeforeMethod(string method)
        {
            return this.Properties.Where(x => x.Key == method).Select(x => x.Value).SingleOrDefault();
        }
        #endregion
    }
}
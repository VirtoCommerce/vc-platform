using DotLiquid;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Web.Models
{
    public class SubmitForm : Drop
    {
        public SubmitForm()
        {
            this.Properties = new Dictionary<string, object>();
        }

        public string ActionLink { get; set; }

        public SubmitFormErrors Errors { get; set; }

        public string Id { get; set; }

        public string FormType { get; set; }

        public bool PasswordNeeded { get; set; }

        public bool? PostedSuccessfully { get; set; }

        public Dictionary<string, object> Properties { get; set; }

        public override object BeforeMethod(string method)
        {
            return this.Properties.Where(x => x.Key == method).Select(x => x.Value).SingleOrDefault();
        }
    }
}
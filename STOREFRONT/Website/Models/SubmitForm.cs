using System;
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

        public virtual string ActionLink { get; set; }

        public virtual object FormContext { get; set; }

        public virtual SubmitFormErrors Errors { get; set; }

        public virtual string Id
        {
            get { return FormType; }
            set { }
        }

        public virtual string FormType { get; set; }

        public virtual bool PasswordNeeded { get; set; }

        public virtual bool? PostedSuccessfully { get; set; }

        public virtual Dictionary<string, object> Properties { get; set; }

        public override object BeforeMethod(string method)
        {
            var val = this.Properties.Where(x => x.Key == method).Select(x => x.Value).SingleOrDefault();
            if (val == null && this.FormContext is Drop)
            {
                var ctx = (FormContext as Drop);
                return ctx[method];
            }
            return val;
        }
    }

    public class AddressForm : SubmitForm
    {
        #region Overrides of SubmitForm
        public override string Id
        {
            get
            {
                if(this.FormContext is CustomerAddress) // address
                    return String.Format("{0}", (this.FormContext as CustomerAddress).Id);

                return base.Id;
            }
            set { }
        }

        public override string ActionLink
        {
            get
            {
                if (FormContext is CustomerAddress) // address
                    return String.Format("/Account/EditAddress?id={0}", (FormContext as CustomerAddress).Id);

                return base.ActionLink;
            }
            set { }
        }
        #endregion
    }
}
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

        public virtual string GetActionLink(object contextObject)
        {
            return this.ActionLink;
        }

        public virtual object FormContext { get; set; }

        public virtual SubmitFormErrors Errors { get; set; }

        public virtual string Id
        {
            get { return FormType; }
            set { }
        }

        public virtual string GetFormId(object contextObject)
        {
            return this.Id;
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
                    return String.Format("address_form_{0}", (this.FormContext as CustomerAddress).Id);

                return base.Id;
            }
            set { }
        }

        #region Overrides of SubmitForm

        public override object FormContext
        {
            get { return base.FormContext; }
            set
            {
                if (value is CustomerAddress)
                {
                    var address = value as CustomerAddress;

                    this.Properties["address1"] = address.Address1;
                    this.Properties["address2"] = address.Address2;
                    this.Properties["city"] = address.City;
                    this.Properties["company"] = address.Company;
                    this.Properties["country"] = address.Country;
                    this.Properties["country_code"] = address.CountryCode;
                    this.Properties["first_name"] = address.FirstName;
                    this.Properties["id"] = address.Id;
                    this.Properties["last_name"] = address.LastName;
                    this.Properties["phone"] = address.Phone;
                    this.Properties["province"] = address.Province;
                    this.Properties["province_code"] = address.ProvinceCode;
                    this.Properties["zip"] = address.Zip;
                }

                base.FormContext = value;
            }
        }

        #endregion

        public override string GetActionLink(object contextObject)
        {
            if (contextObject is CustomerAddress) // address
                return String.Format("/Account/EditAddress?id={0}", (contextObject as CustomerAddress).Id);

            return base.GetActionLink(contextObject);
        }
        #endregion
    }
}
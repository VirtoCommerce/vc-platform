using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Web.Models.FormModels
{
    public class CheckoutFirstStepFormModel
    {
        public CheckoutFirstStepFormModel()
        {
            this.Checkout = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public Dictionary<string, string> Checkout { get; set; }

        public string form_type { get; set; }

        [Required]
        [EmailAddress]
        public string Email
        {
            get { return GetValue("email"); }
            set { SetValue("email", value); }
        }

        [Required]
        public string Address1
        {
            get { return GetValue("address1"); }
            set { SetValue("address1", value); }
        }

        public string Address2
        {
            get { return GetValue("address2"); }
            set { SetValue("address2", value); }
        }

        [Required]
        public string City
        {
            get { return GetValue("city"); }
            set { SetValue("city", value); }
        }

        public string Company
        {
            get { return GetValue("company"); }
            set { SetValue("company", value); }
        }

        [Required]
        public string Country
        {
            get { return GetValue("country"); }
            set { SetValue("country", value); }
        }

        [Required]
        public string FirstName
        {
            get { return GetValue("first_name"); }
            set { SetValue("first_name", value); }
        }

        [Required]
        public string LastName
        {
            get { return GetValue("last_name"); }
            set { SetValue("last_name", value); }
        }

        public string Phone
        {
            get { return GetValue("phone"); }
            set { SetValue("phone", value); }
        }

        public string Province
        {
            get { return GetValue("province"); }
            set { SetValue("province", value); }
        }

        [Required]
        public string Zip
        {
            get { return GetValue("zip"); }
            set { SetValue("zip", value); }
        }

        public string GetValue(string key)
        {
            return this.Checkout.ContainsKey(key) ? this.Checkout[key] : null;
        }

        public void SetValue(string key, string value)
        {
            if (this.Checkout.ContainsKey(key))
            {
                this.Checkout[key] = value;
            }
            else
            {
                this.Checkout.Add(key, value);
            }
        }
    }
}
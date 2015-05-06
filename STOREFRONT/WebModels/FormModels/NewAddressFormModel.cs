using System;
using System.Collections.Generic;

namespace VirtoCommerce.Web.Models.FormModels
{
    public class NewAddressFormModel
    {
        public NewAddressFormModel()
        {
            Address = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public IDictionary<string, string> Address { get; set; }

        public string form_type { get; set; }

        public string Id
        {
            get
            {
                return GetValue("id");
            }
            set
            {
                SetValue("id", value);
            }
        }

        public string Address1
        {
            get
            {
                return GetValue("address1");
            }
            set
            {
                SetValue("address1", value);
            }
        }

        public string Address2
        {
            get
            {
                return GetValue("address2");
            }
            set
            {
                SetValue("address2", value);
            }
        }

        public string City
        {
            get
            {
                return GetValue("city");
            }
            set
            {
                SetValue("city", value);
            }
        }

        public string Company
        {
            get
            {
                return GetValue("company");
            }
            set
            {
                SetValue("company", value);
            }
        }

        public string Country
        {
            get
            {
                return GetValue("country");
            }
            set
            {
                SetValue("country", value);
            }
        }

        public string FirstName
        {
            get
            {
                return GetValue("first_name");
            }
            set
            {
                SetValue("first_name", value);
            }
        }

        public string LastName
        {
            get
            {
                return GetValue("last_name");
            }
            set
            {
                SetValue("last_name", value);
            }
        }

        public string Phone
        {
            get
            {
                return GetValue("phone");
            }
            set
            {
                SetValue("phone", value);
            }
        }

        public string Province
        {
            get
            {
                return GetValue("province");
            }
            set
            {
                SetValue("province", value);
            }
        }

        public string Zip
        {
            get
            {
                return GetValue("zip");
            }
            set
            {
                SetValue("zip", value);
            }
        }

        public string GetValue(string key)
        {
            return Address.ContainsKey(key) ? Address[key] : null;
        }

        public void SetValue(string key, string value)
        {
            if (Address.ContainsKey(key))
            {
                Address[key] = value;
            }
            else
            {
                Address.Add(key, value);
            }
        }
    }
}
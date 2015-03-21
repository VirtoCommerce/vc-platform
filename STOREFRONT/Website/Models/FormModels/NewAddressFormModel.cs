namespace VirtoCommerce.Web.Models.FormModels
{
    public class NewAddressFormModel : BaseAddressFormModel
    {
        #region Public Properties
        public string Address1
        {
            get
            {
                return this.GetValue("address1");
            }
            set
            {
                this.SetValue("address1", value);
            }
        }

        public string Address2
        {
            get
            {
                return this.GetValue("address2");
            }
            set
            {
                this.SetValue("address2", value);
            }
        }

        public string City
        {
            get
            {
                return this.GetValue("city");
            }
            set
            {
                this.SetValue("city", value);
            }
        }

        public string Company
        {
            get
            {
                return this.GetValue("company");
            }
            set
            {
                this.SetValue("company", value);
            }
        }

        public string Country
        {
            get
            {
                return this.GetValue("country");
            }
            set
            {
                this.SetValue("country", value);
            }
        }

        public string FirstName
        {
            get
            {
                return this.GetValue("first_name");
            }
            set
            {
                this.SetValue("first_name", value);
            }
        }

        public string Id
        {
            get
            {
                return this.GetValue("id");
            }
            set
            {
                this.SetValue("id", value);
            }
        }

        public string LastName
        {
            get
            {
                return this.GetValue("last_name");
            }
            set
            {
                this.SetValue("last_name", value);
            }
        }

        public string Phone
        {
            get
            {
                return this.GetValue("phone");
            }
            set
            {
                this.SetValue("phone", value);
            }
        }

        public string Province
        {
            get
            {
                return this.GetValue("province");
            }
            set
            {
                this.SetValue("province", value);
            }
        }

        public string Zip
        {
            get
            {
                return this.GetValue("zip");
            }
            set
            {
                this.SetValue("zip", value);
            }
        }
        #endregion
    }
}
namespace VirtoCommerce.Web.Models.FormModels
{
    public class CustomerAddressFormModel : BaseAddressFormModel
    {
        public string FirstName
        {
            get { return GetValue("first_name"); }
            set { SetValue("first_name", value); }
        }

        public string LastName
        {
            get { return GetValue("last_name"); }
            set { SetValue("last_name", value); }
        }

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

        public string Company
        {
            get { return GetValue("company"); }
            set { SetValue("company", value); }
        }

        public string City
        {
            get { return GetValue("city"); }
            set { SetValue("city", value); }
        }

        public string Province
        {
            get { return GetValue("province"); }
            set { SetValue("province", value); }
        }

        public string ProvinceCode
        {
            get { return GetValue("province_code"); }
            set { SetValue("province_code", value); }
        }

        public string Zip
        {
            get { return GetValue("zip"); }
            set { SetValue("zip", value); }
        }

        public string Country
        {
            get { return GetValue("country"); }
            set { SetValue("country", value); }
        }

        public string CountryCode
        {
            get { return GetValue("country_code"); }
            set { SetValue("country_code", value); }
        }

        public string Phone
        {
            get { return GetValue("phone"); }
            set { SetValue("phone", value); }
        }

        public string Id
        {
            get { return GetValue("id"); }
            set { SetValue("id", value); }
        }
    }
}
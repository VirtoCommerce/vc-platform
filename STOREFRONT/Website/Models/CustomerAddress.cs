#region
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{
    public class CustomerAddress : Drop
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string Company { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string FirstName { get; set; }

        public string Id { get; set; }

        public string LastName { get; set; }

        public string Name
        {
            get
            {
                string name = null;

                if (!string.IsNullOrEmpty(this.FirstName) && !string.IsNullOrEmpty(this.LastName))
                {
                    name = string.Format("{0} {1}", this.FirstName.Trim(), this.LastName.Trim());
                }

                return name;
            }
        }

        public string Phone { get; set; }

        public string Province { get; set; }

        public string ProvinceCode { get; set; }

        public string Street
        {
            get { return !string.IsNullOrEmpty(this.Address1) ? string.Format("{0} {1}", this.Address1, this.Address2 ?? string.Empty).Trim() : null; }
        }

        public string Zip { get; set; }

        public bool IsFilledCorrectly
        {
            get
            {
                return
                    !string.IsNullOrEmpty(this.Address1) &&
                    !string.IsNullOrEmpty(this.City) &&
                    !string.IsNullOrEmpty(this.Country) &&
                    !string.IsNullOrEmpty(this.FirstName) &&
                    !string.IsNullOrEmpty(this.LastName) &&
                    !string.IsNullOrEmpty(this.Zip);
            }
        }
    }
}
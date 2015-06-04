#region
using DotLiquid;
using System.Runtime.Serialization;
using VirtoCommerce.Web.Models.Tags;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class CustomerAddress : Drop
    {
        public CustomerAddress()
        {
        }

        [DataMember]
        public string Address1 { get; set; }

        [DataMember]
        public string Address2 { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Company { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
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

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Province { get; set; }

        [DataMember]
        public string ProvinceCode { get; set; }

        [DataMember]
        public string Street
        {
            get { return !string.IsNullOrEmpty(this.Address1) ? string.Format("{0} {1}", this.Address1, this.Address2 ?? string.Empty).Trim() : null; }
        }

        [DataMember]
        public string Zip { get; set; }

        [DataMember]
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
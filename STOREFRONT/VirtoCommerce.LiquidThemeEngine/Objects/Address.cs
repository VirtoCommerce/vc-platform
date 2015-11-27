using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Address : Drop
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Street { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string ProvinceCode { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Phone { get; set; }

        public string Id { get; set; }
        public string Method { get; set; }
    }
}

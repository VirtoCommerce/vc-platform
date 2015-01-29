using System.Collections.Generic;
using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.CoreModule.Web.Security.Models
{
	public class AuthInfo
	{	   
		public string Login { get; set; }
		public string FullName { get; set; }
		public string[] Permissions { get; set; }
        public RegisterType UserType { get; set; }	   
	}

    public class AuthInfoExtended : AuthInfo
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public int AccountState { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public Address[] Addresses { get; set; }
        public string Email { get; set; }
    }

    public class Address
    {
        public string AddressId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string StateProvince { get; set; }
        public string CountryName { get; set; }
        public string PostalCode { get; set; }
        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string Type { get; set; }
        public string DaytimePhoneNumber { get; set; }
        public string EveningPhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public string Organization { get; set; }
    }
}
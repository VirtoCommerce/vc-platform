using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.SecurityModule.Web.Models
{
	public class AuthInfo
	{
		public string Login { get; set; }
		public string FullName { get; set; }
		public string[] Permissions { get; set; }

        public RegisterType UserType { get; set; }
	}
}
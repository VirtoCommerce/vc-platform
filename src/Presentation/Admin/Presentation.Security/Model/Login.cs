using Omu.ValueInjecter;
using PropertyChanged;
using VirtoCommerce.Foundation.Frameworks;
using ObjectModel = VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.Security.Model
{
	[ImplementPropertyChanged]
	public class Login : StorageEntity
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string BaseUrl { get; set; }
		public string CurrentUserName { get; set; }

		public bool IsValid
		{
			get
			{
				return Validate();				
			}
		}

		public override bool Validate()
		{
			var _usernameIsValid = !string.IsNullOrEmpty(Username);

			if (_usernameIsValid)
			{
				ClearError("Username");
			}
			else
				SetError("Username", "Username is required", true);

			var _passwordIsValid = !string.IsNullOrEmpty(Password);

			if (_passwordIsValid)
			{
				ClearError("Password");
			}
			else
				SetError("Password", "Password is required", true);

			var _baseUrlIsValid = !string.IsNullOrEmpty(BaseUrl);

			if (_baseUrlIsValid)
			{
				_baseUrlIsValid = BaseUrl.StartsWith("http://") || BaseUrl.StartsWith("https://");
				if (_baseUrlIsValid)
					ClearError("BaseUrl");
				else
					SetError("BaseUrl", "Base url should begin with \"http://\" or \"https://\" prefix", true);
			}
			else
				SetError("BaseUrl", "Base url is required", true);

			return _usernameIsValid && _passwordIsValid && _baseUrlIsValid;
		}
	}
}

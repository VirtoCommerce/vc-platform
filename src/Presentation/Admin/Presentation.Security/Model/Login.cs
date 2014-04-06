using Omu.ValueInjecter;
using PropertyChanged;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Security.Properties;
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
				SetError("Username", Resources.Username_is_required, true);

			var _passwordIsValid = !string.IsNullOrEmpty(Password);

			if (_passwordIsValid)
			{
				ClearError("Password");
			}
			else
				SetError("Password", Resources.Password_is_required, true);

			var _baseUrlIsValid = !string.IsNullOrEmpty(BaseUrl);

			if (_baseUrlIsValid)
			{
				_baseUrlIsValid = BaseUrl.StartsWith("http://") || BaseUrl.StartsWith("https://");
				if (_baseUrlIsValid)
					ClearError("BaseUrl");
				else
					SetError("BaseUrl", Resources.Base_url_should_begin_with__http_____or__https_____prefix, true);
			}
			else
				SetError("BaseUrl", Resources.Base_url_is_required, true);

			return _usernameIsValid && _passwordIsValid && _baseUrlIsValid;
		}
	}
}

using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Security.Properties;


namespace VirtoCommerce.ManagementClient.Security.Model
{
	public class AuthenticationContext : IAuthenticationContext
	{

		#region Private Properties

		private IAuthenticationService _authenticationService;

		private static Object lockUpdateProcess = new Object();

		private static Object lockIsProcessing = new Object();
		private bool _isProcwssing;
		private bool IsProcessing
		{
			get
			{
				lock (lockIsProcessing)
				{
					return _isProcwssing;
				}
			}
			set
			{
				lock (lockIsProcessing)
				{
					_isProcwssing = value;
				}

			}
		}

		private object _RegisterTypeLock = new object();
		private RegisterType _RegisterType = RegisterType.SiteAdministrator;
		private RegisterType RegistrationType
		{
			set
			{
				lock (_RegisterTypeLock)
				{
					_RegisterType = value;
				}
			}
			get
			{
				lock (_RegisterTypeLock)
				{
					return _RegisterType;
				}
			}
		}

		private string[] _permissions = new string[] { };
		private string[] Permissions
		{
			set
			{
				lock (_permissions)
				{
					_permissions = value;
				}
			}
			get
			{
				lock (_permissions)
				{
					return _permissions;
				}
			}
		}

		private string _baseUrl = "";
		private string BaseUrl
		{
			set
			{
				lock (_baseUrl)
				{
					_baseUrl = value;
				}
			}
			get
			{
				lock (_baseUrl)
				{
					return _baseUrl;
				}
			}
		}

		private string _userName = "";
		private string UserName
		{
			set
			{
				lock (_userName)
				{
					_userName = value;
				}
			}
			get
			{
				lock (_userName)
				{
					return _userName;
				}
			}
		}


		private string _password = "";
		private string Password
		{
			set
			{
				lock (_password)
				{
					_password = value;
				}
			}
			get
			{
				lock (_password)
				{
					return _password;
				}
			}
		}

		private static Object lockExpiresOn = new Object();
		private DateTime _expiresOn = DateTime.UtcNow;
		private DateTime ExpiresOn
		{
			set
			{
				lock (lockExpiresOn)
				{
					_expiresOn = value;
				}
			}
			get
			{
				lock (lockExpiresOn)
				{
					return _expiresOn;
				}
			}
		}

		#endregion

		public AuthenticationContext(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}

		#region IAuthenticationContext Members

		public string CurrentUserName { get; private set; }

		public string CurrentUserId { get; private set; }

		private string _token = "";

		public string Token
		{
			set { _token = value; }
			get { return _token; }
		}

		public bool IsUserAuthenticated
		{
			get { return !String.IsNullOrEmpty(Token); }
		}

		public bool IsAdminUser
		{
			get
			{
				return this.RegistrationType == RegisterType.Administrator;
			}
		}


		public bool Login(string userName, string password, string baseUrl)
		{
			Token = null;
			UserName = userName;
			Password = password;
			BaseUrl = baseUrl;
			UpdateToken();
			return IsUserAuthenticated;
		}

		/// <summary>
		/// Update token if token has been expired.
		/// </summary>
		public void UpdateToken()
		{
			if (ExpiresOn > DateTime.UtcNow)
			{
				return;
			}
			Debug.WriteLine("Need new token");
			lock (lockUpdateProcess)
			{
				Debug.WriteLine("Enter to critical section of getting token");
				if (ExpiresOn <= DateTime.UtcNow && !IsProcessing)
				{
					Debug.WriteLine("Start getting token");
					string newToken = null;
					Exception exception = null;

					try
					{
						if (_authenticationService != null)
						{
							IsProcessing = true;
#if DEBUG
							//Thread.Sleep(3000);
#endif
							//get url for scope authenticate
							var dataServicesBaseUri = new Uri(BaseUrl);

							// get new token
                            newToken = _authenticationService.AuthenticateUserAsync(UserName, Password, dataServicesBaseUri).Result;
							Debug.WriteLine("Token updated");
							if (!String.IsNullOrEmpty(newToken))
							{
								// Parse token
								var swToken =
									Foundation.Security.Swt.SimpleWebToken.Parse(newToken);
								Token = newToken;
								ExpiresOn = swToken.ExpiresOn;
								ExpiresOn = ExpiresOn.AddMinutes(-(ExpiresOn - DateTime.UtcNow).TotalMinutes / 2);
								CurrentUserId =
									swToken.Claims.Where(x => x.Key == ClaimTypes.NameIdentifier)
										   .Select(x => x.Value)
										   .FirstOrDefault();
								CurrentUserName =
									swToken.Claims.Where(x => x.Key == ClaimTypes.Name)
										   .Select(x => x.Value)
										   .FirstOrDefault();

								var registrationType = swToken.Claims.Where(x => x.Key == SecurityClaims.AccountRegistrationType).Select(x => x.Value).FirstOrDefault();

								if (!String.IsNullOrEmpty(registrationType))
								{
									RegistrationType = (RegisterType)Enum.Parse(typeof(RegisterType), registrationType);
								}

								var permissions = swToken.Claims.Where(x => x.Key == SecurityClaims.AccountPermission)
												   .Select(x => x.Value)
												   .ToArray();

								Permissions = permissions;
							}
						}
					}
					catch (Exception e)
					{
						exception = new GetTokenException(e.Message);
					}
					finally
					{
						IsProcessing = false;
					}
					if (exception == null && String.IsNullOrEmpty(newToken))
					{
						exception = new GetTokenException(Resources.Login_or_password_is_incorrect);
					}
					if (exception != null)
					{
						throw exception;
					}

				}
				Debug.WriteLine("Exit from critical section of getting token");
			}
		}

		public bool CheckPermission(string permission)
		{
			var retVal = IsAdminUser;
			//var retVal = false;
			if (!retVal)
			{
				var permissions = Permissions;
				if (permissions != null)
				{
					retVal = permissions.Any(x => x.StartsWith(permission));
				}
			}
			return retVal;
		}

		#endregion

	}

	public class GetTokenException : Exception
	{
		public GetTokenException()
		{
		}

		public GetTokenException(string message)
			: base(message)
		{
		}

		public GetTokenException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}

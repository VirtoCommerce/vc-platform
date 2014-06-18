using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using PropertyChanged;
using System;
using System.Configuration;
using System.Deployment.Application;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;
using VirtoCommerce.ManagementClient.Security.Model;
using VirtoCommerce.ManagementClient.Security.Properties;
using VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Implementations
{
    [ImplementPropertyChanged]
    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        #region Private fields

        private readonly IUnityContainer _container;
        private bool _isUserAuthenticated;

        #endregion

        public override bool IsBackTrackingDisabled
        {
            get
            {
                return true;
            }
        }

        public string ApplicationLoadingStatus { get; private set; }

        #region Constructor

        public LoginViewModel(IUnityContainer container)
        {
            CurrentUser = new Login();
            CurrentUser.PropertyChanged += login_PropertyChanged;
            LoginCommand = new DelegateCommand(ProcessLogin, () => CurrentUser.IsValid);
            _container = container;
            LoadValues();

            // subscribe to application loading status messages
            EventSystem.Subscribe<GenericEvent<string>>(xx => OnUIThread(() =>
            {
                ApplicationLoadingStatus = xx.Message;
            }));

#if DEBUG
            OnUIThread(() =>
                {
                    if (string.IsNullOrEmpty(CurrentUser.Username))
                    {
                        CurrentUser.Username = SecurityModule.UserNameAdmin;
                    }
                    if (string.IsNullOrEmpty(CurrentUser.BaseUrl))
                    {
                        CurrentUser.BaseUrl =
                            GetConnectionStringBaseUrl(SecurityConfiguration.Instance.Connection.DataServiceBaseUriName);
                    }
                    CurrentUser.Password = "store";
                });
#endif
        }

        void login_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            LoginCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region ILoginViewModel Members

        public DelegateCommand LoginCommand { get; private set; }

        [AlsoNotifyFor("IsAnimation")]
        public bool AuthProgress { get; set; }

        public Login CurrentUser { get; private set; }

        public bool IsAnimation
        {
            get { return _isUserAuthenticated || AuthProgress; }
        }

        public string Error { get; set; }

        #endregion

        #region Private methods

        private void RegisterSecurityServices(string serviceBaseUrl)
        {
            var factory = new ServiceConnectionFactory(serviceBaseUrl);
            _container.RegisterInstance<IServiceConnectionFactory>(factory, new ContainerControlledLifetimeManager());

            _container.RegisterService<ISecurityService>(
                factory.GetConnectionString(SecurityConfiguration.Instance.Connection.ServiceUri),
                SecurityConfiguration.Instance.Connection.WSEndPointName);

            _container.RegisterService<IAuthenticationService>(
                factory.GetConnectionString(SecurityConfiguration.Instance.Authentication.ServiceUri),
                SecurityConfiguration.Instance.Authentication.WSEndPointName);

            var service = _container.Resolve<IAuthenticationService>();
            _container.RegisterInstance<IAuthenticationContext>(new AuthenticationContext(service), new ContainerControlledLifetimeManager());
        }

        public event EventHandler LogonViewRequestedEvent;

        private async void ProcessLogin()
        {
            if (AuthProgress)
            {
                return;
            }
            AuthProgress = true;
            Error = null;
            NavigationNames.PublishStatusUpdate("Connecting");

            try
            {
                var serviceBaseUrl = CurrentUser.BaseUrl.ToLower();
                if (!serviceBaseUrl.EndsWith("/"))
                {
                    serviceBaseUrl += "/";
                }

                if (!serviceBaseUrl.EndsWith("Virto/"))
                {
                    serviceBaseUrl += "Virto/";
                }

                RegisterSecurityServices(serviceBaseUrl);

                var authenticationContext = _container.Resolve<IAuthenticationContext>();
                _isUserAuthenticated = await Task.Run(() => authenticationContext.Login(CurrentUser.Username, CurrentUser.Password, serviceBaseUrl));

                if (_isUserAuthenticated)
                {
                    OnPropertyChanged("IsAnimation");
                    CurrentUser.CurrentUserName = authenticationContext.CurrentUserName;
                    var logonEvent = LogonViewRequestedEvent;
                    if (logonEvent != null)
                    {
                        logonEvent(this, null);
                    }
                    SaveValues();
                }
            }
            catch (GetTokenException e)
            {
                Error = e.Message;
                AuthProgress = false;
            }
            catch (Exception e)
            {
                ErrorDialog.ShowErrorDialog(e.Message, e.StackTrace, e.ToString(), false, Resources.Error_at_login__);
                AuthProgress = false;
            }
        }

        #endregion

        #region Save/Load authentitication values

        private void SaveValues()
        {
            try
            {
                if (!string.IsNullOrEmpty(CurrentUser.Username) && !string.IsNullOrEmpty(CurrentUser.Password) && !string.IsNullOrEmpty(CurrentUser.BaseUrl))
                {
                    var globalConfigService = _container.Resolve<IGlobalConfigService>();
                    if (globalConfigService != null)
                    {
                        globalConfigService.Update("UserName", CurrentUser.Username);
                        globalConfigService.Update("BaseUrl", CurrentUser.BaseUrl);
                    }
                }
            }
            catch
            {
            }
        }

        private void LoadValues()
        {
            try
            {
                string parameterUserName = null;
                string parameterBaseUrl = null;

                if (ApplicationDeployment.IsNetworkDeployed && AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null)
                {
                    var parameter = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0];
                    string queryString;
                    if (Uri.IsWellFormedUriString(parameter, UriKind.Absolute))
                    {
                        // activating from the web
                        queryString = new Uri(parameter).Query;
                    }
                    else
                    {
                        // in case activating from SDK Configuration Manager
                        queryString = parameter;
                    }

                    var nameValueTable = HttpUtility.ParseQueryString(queryString);
                    parameterBaseUrl = nameValueTable.Get("storeurl");
                }

                var globalConfigService = _container.Resolve<IGlobalConfigService>();
                if (globalConfigService != null)
                {
                    parameterUserName = (string)globalConfigService.Get("UserName");
                    parameterBaseUrl = parameterBaseUrl ?? (string)globalConfigService.Get("BaseUrl");
                }

                if (!string.IsNullOrEmpty(parameterUserName) || !string.IsNullOrEmpty(parameterBaseUrl))
                {
                    if (!string.IsNullOrEmpty(parameterBaseUrl))
                    {
                        parameterBaseUrl = parameterBaseUrl.Replace('\\', '/');
                    }

                    OnUIThread(() =>
                    {
                        CurrentUser.Username = parameterUserName;
                        CurrentUser.BaseUrl = parameterBaseUrl;
                    });
                }
            }
            catch
            {
            }
        }

        private static string GetConnectionStringBaseUrl(string baseUriName)
        {
            return ConfigurationManager.ConnectionStrings[baseUriName] != null ? ConfigurationManager.ConnectionStrings[baseUriName].ConnectionString : "http://";
        }

        #endregion
    }
}

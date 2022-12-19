# How to add a new SSO provider
This tutorial shows you how to how to implement a new single sign-on (SSO) provider in an extension module using Google as an example.

## Create the Google OAuth 2.0 Client

Any application that uses OAuth 2.0 to access Google APIs must have authorization credentials that identify the application to Google's OAuth 2.0 server. The following steps explain how to create credentials for your project. Your applications can then use the credentials to access APIs that you have enabled for that project.

* Go to [Google API & Services](https://console.cloud.google.com/apis)
* Create the new Project and open the Dashboard
* In the Oauth consent screen of the Dashboard:
    * Select User Type - External and CREATE.
    * In the App information dialog, Provide an app name for the app, user support email, and developer contact information.
    * Step through the Scopes step.
    * Step through the Test users step.
    * Review the OAuth consent screen and go back to the app Dashboard.
    * In the Credentials tab of the application Dashboard, select CREATE CREDENTIALS > OAuth client ID.

* Select Application type > Web application, choose a name.
* In the Authorized redirect URIs section, select ADD URI to set the redirect URI, for example for the platform on a local machine add https://localhost:10645/signin-google. Run the platform using **https** scheme, otherwise SSO won't work.
* Press the CREATE button.
* Save the **Client ID** and **Client Secret** for use in the module.

## Google sign-in configuration

Store sensitive settings such as the Google client ID and secret values inside KeyVault Storage. For this sample, we'll use appsettings.json configuration file. Add the following section to the configuration.  

```JSON
"Google": {
    "Enabled": true,
    "AuthenticationType": "Google",
    "AuthenticationCaption": "Google",
    "ClientId": "<your Client ID>",
    "ClientSecret": "<your Client Secret>",
    "DefaultUserType": "Manager"
}
```

## Module extensions

Add the Microsoft.AspNetCore.Authentication.Google v6.0 package to .web project of the custom extension module then add the following code:

* Add the basic **GoogleOptions.cs** class
    ```cs
    public class GoogleOptions
    {
        public bool Enabled { get; set; }
        public string AuthenticationType { get; set; }
        public string AuthenticationCaption { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DefaultUserType { get; set; }
    }
    ```

* Add the **GoogleExternalSignInProvider.cs** class. **IExternalSignInProvider** interface describes the external provider custom behaviour. 
    ```cs
    public class GoogleExternalSignInProvider : IExternalSignInProvider
    {
        private readonly GoogleOptions _options;

        public bool AllowCreateNewUser => true;
        public int Priority => 200;
        public bool HasLoginForm => false;

        public GoogleExternalSignInProvider(IOptions<GoogleOptions> options)
        {
            _options = options.Value;
        }

        // Use this method to retrieve the username claim
        public string GetUserName(ExternalLoginInfo externalLoginInfo)
        {
            return externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        }

        // Use this method to get the default user type that'll be assigned to a new user
        public string GetUserType()
        {
            return _options.DefaultUserType;
        }
    }
    ```

* Configure Google authentication. Add the following code to the **module.cs** Initialize method
    ```cs
    var googleAuthEnabled = Configuration.GetValue<bool>("Google:Enabled");
    if (googleAuthEnabled)
    {
        // add options
        var optionsSection = Configuration.GetSection("Google");
        var options = new GoogleOptions();
        optionsSection.Bind(options);
        serviceCollection.Configure<GoogleOptions>(optionsSection);

        // add app builder google sso
        var authBuilder = new AuthenticationBuilder(serviceCollection);

        authBuilder.AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = options.ClientId;
            googleOptions.ClientSecret = options.ClientSecret;
        });

        // register Google external provider implementation
        serviceCollection.AddSingleton<GoogleExternalSignInProvider>();
        serviceCollection.AddSingleton(provider => new ExternalSignInProviderConfiguration
        {
            AuthenticationType = options.AuthenticationType,
            Provider = provider.GetService<GoogleExternalSignInProvider>(),
        });
    }
    ```

## Sign in with Google

Run the platform and open the Log in screen. An option to sign in with Google appears. Select the Google button, which redirects to Google for authentication. After entering your Google credentials, you will be redirected back to the platform.

![Platform login](../media/google-sso-login.png)
# How to sharing bearer tokens across platform instances 

In some deployment scenarios when are running multiple platform instances one of them usually play **authentication server** role and has access to user accounts storages.
Other platform instances are play role as **resource servers** that simply needs to limit access to those users who have valid security tokens that were provided by an **authentication server**.

![image](https://user-images.githubusercontent.com/7566324/71414550-26545b80-2660-11ea-93ed-eda241f5c76b.png)

Once the token is issued and signed by the authentication server, no database communication is required to verify the token.
Any service that accepts the token will just validate the digital signature of the token.

For that scenario, authentication middleware that handles JWT tokens is available in the `Microsoft.AspNetCore.Authentication.JwtBearer` package.
JWT stands for "JSON Web Token" and is a common security token format (defined by RFC 7519) for communicating security claims

The Virto platform has some  settings that can used to configure resource server to consume to consume such tokens

appsettings.json
```json5
...
 "Auth": {
        //Is the address of the token-issuing authentication server.
        //The JWT bearer authentication middleware uses this URI to get the public key that can be used to validate the token's signature.
        //The middleware also confirms that the iss parameter in the token matches this URI.
        "Authority": "https://authentication-server-url",
        
        //represents the receiver of the incoming token or the resource that the token grants access to.
        //If the value specified in this parameter does not match the parameter in the token,
        //the token will be rejected.
        //the 'resource_server' value can be left unchanged
        "Audience": "resource_server",

        "PublicCertPath": "./Certificates/virtocommerce.crt",
        "PrivateKeyPath": "./Certificates/virtocommerce.pfx",
        "PrivateKeyPassword": "virto"
    }
 ...
```

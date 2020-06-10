# SignalR scalability configuration

SignalR scalability can be configured with *SignalR* section of platform configuration.
To disable scalability of SignalR set *ScalabilityProvider* parameter to ```null``` value.
To enable scalability set *ScalabilityProvider* parameter to one of two available provider value.
You can set one of two providers, ```AzureSignalRService``` or ```RedisBackplane```.
The first way also requires to configure connection string in *AzureSignalRService* section.
If RedisBackplane mode is set, the common Redis connection string will be used. You also can set the Redis channel name via *RedisBackplane* child section.

```
  "SignalR": {        
        "ScalabilityProvider": null,
        "AzureSignalRService": {              
            "ConnectionString": "AzureSignalRServiceConnectionString"                               
        },
        "RedisBackplane": {
            "ChannelName": "VirtoCommerceChannel"            
        }
  }
```

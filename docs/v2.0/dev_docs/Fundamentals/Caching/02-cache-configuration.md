# Configuring Cache
Cache configuration is commonly provided by the _Caching_ section of the _appsettings.json_ files:

`appsettings.json`

```json
1 "Caching": {
2       "CacheEnabled": true, 
3       "CacheSlidingExpiration": "0:15:00", 
4       //"CacheAbsoluteExpiration": "0:15:00"
5    }
```
Notes: 

+ `CacheEnabled`: Set to `false` to disable caching of application data for the entire application.

+ `CacheSlidingExpiration` or `CacheAbsoluteExpiration`: Sets a sliding or absolute expiration time for all cached application data that does not have a manually configured expiration value.

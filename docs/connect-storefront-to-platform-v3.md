# Connect Storefront to Platform

* Deploy the latest storefront version as [described here](https://virtocommerce.com/docs/vc2devguide/deployment/storefront-deployment)

Note, if Platform and Storefront are deployed in the same on-premises environment, Storefront should be deployed on different port then Platform. You can do it by `dotnet run CLI`

```console
dotnet VirtoCommerce.Storefront.dll --urls=http://localhost:5002/
```

or host Storefront in IIS.

* Make changes  in  `appsettings.json`

```json
...
 "Endpoint": {
     //Specify platform url
     "Url": "https://localhost:5001",
     //Comment the following settings
     //"AppId": "...",
     //"SecretKey":"...",

     //Uncomment the following settings
      "UserName": "admin",
      "Password": "store",
      "RequestTimeout": "0:0:30"
    },
```

* Trust the .Net Core Development Self-Signed Certificate. More details on trusting the self-signed certificate can be found [here](https://blogs.msdn.microsoft.com/robert_mcmurray/2013/11/15/how-to-trust-the-iis-express-self-signed-certificate/)

## License

Copyright (c) Virto Solutions LTD.  All rights reserved.

Licensed under the Virto Commerce Open Software License (the "License"); you
may not use this file except in compliance with the License. You may
obtain a copy of the License at

http://virtocommerce.com/opensourcelicense

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
implied.

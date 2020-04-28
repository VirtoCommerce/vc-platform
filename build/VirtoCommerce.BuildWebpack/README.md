# VirtoCommerce BuildWebpack 
## Overview
The BuildWebpack project share a build tool among several .NET Framework or .NET Core projects through NuGet Package.    
## Installing
Install this package to VirtoCommerce.*.Web project in solutuion

## Functional possibilities
When NuGet package imported and build started the command “npm run webpack:dev” will be executed if bundler folder is absent and file “webpack.config.js” exist.
## Modification
After mofification build target you should create NuGet Package 
To build and package the target, you can use

``nuget pack VirtoCommerce.BuildWebpack.nuspec`` 

or

``dotnet pack VirtoCommerce.BuildWebpack.csproj --output ./ --configuration Release``.

In case of ``nuget pack VirtoCommerce.BuildWebpack.nuspec`` you can create Nuget package without *.sln, *.csprj, *.cs files in project.

Additional information at https://natemcmaster.com/blog/2017/11/11/build-tools-in-nuget/
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

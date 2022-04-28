# Module.manifest File
The _module.manifest_ file is used both to build the module package and to provide information to the platform runtime when a module is loading . The manifest is always included into a module package.

_Module.manifest_ is an XML file containing a top-level <module> node, which in its turn contains section elements described in the section below.

# Manifest File Example

Below, you can find an example of a _module.manifest_ file that illustrates a number of settings including optional ones:

`module.manifest`

```xml
<module xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
xmlns:xsd="http://www.w3.org/2001/XMLSchema">
<!-- 
(Required)
The case-insensitive module identifier, which must be unique across the set of modules module 6 resides in. 
IDs may not contain spaces or characters that are not valid for a URL,and generally follow .NET namespace rules.
--> 
 <id>VirtoCommerce.Cart</id>
 <!-- 
 (Required)
 The version of the package following the major.minor.patch pattern.
 -->
  <version>3.27.0</version>
<!--
(Optional)
Version numbers may include a pre-release suffix.
-->
  <version-tag>beta001</version-tag>
<!--
(Required)
The minimum platform version the current module is compatible with. 
-->
  <platformVersion>3.62.0</platformVersion>
<!--
(Optional)
A human-friendly title and description of the module, which may be used in the Platform Manager UI.
-->
  <title>Shopping cart module</title>
  <description>Shopping cart / checkout functionality</description>
  <authors>
    <author>Virto Commerce</author>
  </authors>
  <owners>
    <owner>Virto Commerce</owner>
  </owners>
<!--
(Optional)
A URL for the package home page displayed by the Platform Manager UI.
-->
  <projectUrl>https://virtocommerce.com/apps/extensions/virto-shoppingcart-module</projectUrl>
<!--
(Optional)
A path to an image file shown in the Platform Manager UI as a module icon.
This can be either a path to an image file within the module that is located in the Content folder or a URL to an external image.
-->  
<iconUrl>Modules/$(VirtoCommerce.Cart)/Content/logo.png</iconUrl>
<!--
(Required)
This value is used to specify the name of the assembly, which the module type is loaded from.
-->
  <assemblyFile>VirtoCommerce.CartModule.Web.dll</assemblyFile>
<!--
(Required)
A fully qualified name of the type, including its namespace with a class that implements the IModule interface.
The module loader creates an instance of the module class, and then it calls the Initialize method. 
  <moduleType>VirtoCommerce.CartModule.Web.Module, VirtoCommerce.CartModule.Web</moduleType>
<!--
(Optional)
Contains any number of <dependency> elements that identify other modules this module depends upon. 
-->  
<dependencies>
    <dependency id="VirtoCommerce.Core" version="3.22.0" />  
  </dependencies>
</module>
```

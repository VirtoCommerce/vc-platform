# Create a new module
The section explains how to create a new Virto Commerce module.

## Prerequisites

Virto Commerce uses `dotnet new` command to create a Virto Commerce module project based on a template.

* Open Windows PowerShell CMD .
* Run command to install Virto Commerce template.
    ```console
    dotnet new --install VirtoCommerce.Module.Template
    ```

![](../media/dotnet-new-install-template.png)


## Create a new module project

To create a new Virto Commerce module project from template:

* Open Windows PowerShell CMD.
* Navigate to your sources folder.
* Run command.
    ```console
    dotnet new vc-module --ModuleName CustomerReviews --Author "Jon Doe" --CompanyName VirtoCommerce 
    ```
* `vc-module-CustomerReviews` folder with solution should be created. 

The command calls the template engine to create the artifacts on disk based on the Virto Commerce template and options.


![](../media/dotnet-new-create-module-from-template.png)

### Parameters
| Options | Description | Type | Required | Default value |
|--------|-------------|------|----------|---------------|
| --Author (or -A) | Your name | string | Optional| John Doe |
| --CompanyName (or -C) | Your company name| string | Optional | VirtoCommerce |
| --ModuleName (or -M) | Your module name | string | Optional | newModule |
| --PlatformVersion (or -P) | Required Platform Version | string | Optional | 3.84.0 |
| --PlatformNuGetPackageVersion (or -Pl) | Required Platform NuGet Package Version | string | Optional | 3.84.0 |

## Build 
Virto Commerce CLI should be used to build, test and create a module package.

!!! note
    Also you can open solution in Visual Studio and use Visual Studio tools to build the module.

* Open Windows PowerShell CMD.
* Navigate to your sources folder. Ex: vc-module-{module-name}.
* Run command.
    ```console
    vc-build Compress
    ```

![](../media/vc-build-compress.png)

As a result you can find the module package in artifacts folder.

## Installation
The module package can be uploaded and installed to the Virto Commerce Platform.

* Open and sign-in into Virto Commerce Admin Portal.
* Select `Modules` menu and select `Advanced` section.
* Install/update module from file.
* Restart the platform.

If the module is installed properly, you should see the new module in the list of installed modules, Admin UI and Swagger API.

## See also
* [Essential modularity](../fundamentals/essential-modularity.md)
* [Virto Commerce CLI](https://docs.virtocommerce.org/CLI-tools/introduction/)
* [Virto Commerce Module Template on GitHub](https://github.com/VirtoCommerce/vc-cli-module-template)
* [VC-Build problem on .NET 5.0 and .NET 6.0](https://www.virtocommerce.org/t/vc-build-problem-on-net-5-and-net6/276)

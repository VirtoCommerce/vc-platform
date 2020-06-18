---
aliases:
  - docs/vc2devguide/deployment/platform-deployment/source-code-getting-started
date: '2018-08-31'
layout: docs
title: 'Deploy Platform from source code'

---
## Summary

Use this guide to deploy and configure Virto Commerce Platform from source code and setup development environment.

## Prerequisites

* Microsoft .NET Framework 4.6.1
* Internet Information Services 7 or later
* Microsoft SQL Server 2008 or laterВ (*Express or Full*)
* Visual Studio 2015 or later (*optional*)
* Git

## Downloading source code

Fork your own copy of VirtoCommerce Platform to your account on GitHub:
1. Open <a href="https://github.com/VirtoCommerce/vc-platform" rel="nofollow">Virto Commerce Platform in GitHub</a>В and clickВ **Fork**В in the upper right corner.
2. If you are a member of an organization on GitHub, select the target for the fork.
3. Clone the forked repository to local machine:
  ```
  git cloneВ https://github.com/vc-platform/vc-platform.git C:\vc-platform
  ```
4. Switch to the cloned directory:
  ```
  cd C:\vc-platform
  ```
5. Add a reference to the original repository:
  ```
  git remote add upstreamВ https://github.com/VirtoCommerce/vc-platform.git
  ```

In result you should get the **C:\vc-platform** folder which contains full platform source code.
To retrieve changes from original Virto Commerce Platform repository, merge **upstream/master** branch.

Restore NuGet packages in one of the following ways:
* Open **VirtoCommerce.Platform.sln**В solution in Visual Studio. In Solution Explorer window right-click on solution and select **Manage NuGet Packages for Solution**. In the opened window click the **Restore** button.
![Restore NuGet packages for solution](../../../assets/images/docs/image2016-7-26_11-23-32.png "Restore NuGet packages for solution")
* Run this command:
   ```
   nuget restore C:\vc-platform\VirtoCommerce.Platform.Web\VirtoCommerce.Platform.sln
   ```
Build the solution.

## Configure SQL Server

To be able to use default sql connection string with **(local)** hostname and **server authentication** credentials
  ```
 <add name="VirtoCommerce" connectionString="Data Source=(local);Initial Catalog=VirtoCommerce2;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=420" providerName="System.Data.SqlClient" />
  ```
You need to check the follow options in the SQL/SqlExpress setup
![Restore NuGet packages for solution](../../../assets/images/docs/sqlexpress-setup-2.JPG "Set default instance")
SQL Server Authentication mode must be enabled.  
![Restore NuGet packages for solution](../../../assets/images/docs/sqlexpress-setup-1.JPG "Mixed server and Windows authentication")

Create the new login named **virto** with password **virto**. The password policy enforcement should be switched off for a simple password like this.

```
USE master; CREATE LOGIN virto WITH PASSWORD = 'virto', CHECK_POLICY = OFF
```

Give theВ **CREATE ANY DATABASE** permission to user virto. This will allow you to create a database automaticallyВ when Commerce ManagerВ starts. For new databases created with this permission the user will have the **db_owner** role while access will be deniedВ for any other database.

```
USE master; GRANT CREATE ANY DATABASE TO virto
```

Open the **C:\vc-platform\VirtoCommerce.Platform.Web\web.config** file and make sure theВ **VirtoCommerce**В connection string has correct parameters for your configuration, particularly **Data Source**В (SQL Server address), **Initial Catalog** (database name), **User ID** (user name) and **Password**.

## Configure IIS

Open **IIS Manager** and add new application to **Default Web Site** with alias **admin**В and physical path toВ **C:\vc-platform\VirtoCommerce.Platform.Web**. Select application pool which uses **.NET CLR Version 4.0** and **Integrated pipeline mode**.
![Create new web application for Virto Commerce Platform](../../../assets/images/docs/add-admin-application-source-code.png "Create new web application for Virto Commerce Platform")

Inside the **admin** application add a new virtual directory with alias **assets**В and physical path toВ **C:\vc-platform\VirtoCommerce.Platform.Web\App_Data\Assets**. If there is no Assets directory inside App_Data, create it.
![Create a virtual directory for Virto Commerce Platform assets](../../../assets/images/docs/create-platform-assets-virtual-folder-source-code.png "Create a virtual directory for Virto Commerce Platform assets")

Your web site structure should be similar to the one shown below:
![Virto Commerce Platform website structure](../../../assets/images/docs/image2016-7-26_12-8-20.png "Virto Commerce Platform website structure")

## Configure file system

Open properties for theВ folder where you have extracted precompiled version or source code **(C:\vc-platform)**В and give permissionВ **Read & execute**В toВ **Users**В group if this permission is not inherited from the parent folder.  
![Set permissions for Virto Commerce Platform application folder](../../../assets/images/docs/image2015-8-21_15-46-0.png "Set permissions for Virto Commerce Platform application folder")

Open properties forВ **C:\vc-platform\VirtoCommerce.Platform.Web\App_Data**В folder and give permissionВ **Modify**В toВ **IIS_IUSRS**В user group.
![Set permissions for Virto Commerce Platform App_Data folder](../../../assets/images/docs/image2015-9-23_11-29-42.png "Set permissions for Virto Commerce Platform App_Data folder")

Open properties forВ **C:\vc-platform\VirtoCommerce.Platform.Web\Modules** (create this folder if not exsist)В folder and give permissionВ **Modify**В toВ **IIS_IUSRS**В user group as shown above.

## Configure CMS content (optionally)

[VirtoCommerce CMS Content module](apps/extensions/virto-cms-module) provide you ability to view & update CMS content from platform UI.
You can optionally change default path of CMS content:
* In the **C:\vc-platform\VirtoCommerce.Platform.Web\web.config** file in the **connectionStrings** section add the **add** node named **CmsContentConnectionString**
* Change its **connectionString** attribute value to needed folder path (**provider=LocalStorage;rootPath=~/App_Data/cms-content** connection string will be used by default).
  ```
  <connectionStrings>
    ...
    <add name="CmsContentConnectionString" connectionString="provider={provider name};rootPath={URL to CMS content location}" />
    ...
  </connectionStrings>
  ```

## Start VirtoCommerce Platform

Open the http://localhost/adminВ URL in browser.В A sign in page should open:
![Virto Commerce Platform login screen](../../../assets/images/docs/image2015-2-26_12-5-39.png "Virto Commerce Platform login screen")

The default administrator account login isВ **admin**В and the password isВ **store**.

## Next stepsВ (optional)

The VirtoCommerce Platform is up and running. What's next? Here are some ideas to try.

### Modules

#### Install modules in VirtoCommerce Platform UI

Install some modules as described inВ [Modules management](docs/vc2userguide/configuration/modules-management)В tutorial.

#### Manual module installationВ from source code

Do you prefer the installation inВ developer way? Great, follow theseВ steps for each module:
* Download the latest module source code from module GitHub repository in the same way as you did that for VirtoCommerce Platform in first step.
* Extract it to platform'sВ **Modules** directory. You also can extract it to any directory (**C:\vc-module** for example) and link it to the Modules directory using the following command:
  ```
  mklink /d C:\vc-platform\VirtoCommerce.Platform\VirtoCommerce.Platform.Web\Modules C:\vc-module
  ```
* Open and compile module solution.
* Restart IIS.
* Open Platform app.
* The new module will be loaded automatically.
* Check its status in **Configuration в†’ Modules в†’ Installed**.

#### Create your own module

Why not to **create your own** module? Follow the steps described inВ [Developing a custom solution](docs/vc2devguide/development-scenarios/developing-a-custom-solution)В tutorial.

#### How platform loads modules

![Virto Commerce Platform loading modules scheme](../../../assets/images/docs/VC_modules_copy_process.png "Virto Commerce Platform loading modules scheme")

### Sample data

If you have no data for your VirtoCommerce Platform, but want to easy develop VirtoCommerce Platform or its modules, you can install our sample data.

At first login you will see the following dialog after optional module choice & installation:

![Virto Commerce Platform sample data installation](../../../assets/images/docs/sample-data-installation.png "Virto Commerce Platform sample data installation")

Click to **Clothing and Electronics**. Sample data installation will start.

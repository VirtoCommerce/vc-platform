---
title: Extending Functionality
description: The list of articles about extending Virto Commerce functionality
layout: docs
date: 2016-09-07T09:51:26.077Z
priority: 2
---
Virto Commerce Platform is an ASP.NET MVC andВ [AngularJS](http://angularjs.org/)В Single Page Application withВ VirtoCommerce's modularity feature.

A module is a subdirectory ofВ **~/Modules**В directoryВ which contains aВ **module.manifest**В file.В Additionally, a module can contain any other content such asВ JavaScript, CSS, image files, .NET assemblies, etc. Some content is specialized and specifics should be outlined in the module manifest.В If a module contains .NET assemblies it is called aВ **managed module**.

Modules can extend Virto Commerce Platform either with JavaScript or with managed code.

JavaSript allows the user to:
* add new items to main menu.В [Registration in application's menu](docs/vc2devguide/working-with-platform-manager/extending-functionality/creating-new-module)
* add new widgets to widget containers (on dashboard or in blades). [Working with widgets](docs/vc2devguide/working-with-platform-manager/basic-functions/widgets)
* add new blades
* add custom content inside the blade orВ totally redefine the content using [metaform](docs/vc2devguide/working-with-platform-manager/basic-functions/metaform)В control
* add new buttons and other content to existing blade toolbars.В [Working with blade toolbar](docs/vc2devguide/working-with-platform-manager/basic-functions/blade-toolbar)
* define newВ types ofВ notifications andВ addВ new notificationВ templates. [Working with notifications](docs/vc2devguide/working-with-platform-manager/basic-functions/push-notifications)
* define new UI for settings management

Managed code allows the user to:
* add new Web API controllers
* add new services
* override existing services
* modify database

The UI can be extended with Javascript and the backend can be extended with managed code.

In addition, new security permissions as well as new application settings are added with the module manifest, but they are still used either in JavaScript or in managed code.

In this tutorial you will learn how to create custom modules with and without managed code. Each module will be loaded to the main application and will have its entry in the main menu.

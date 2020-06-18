---
title: Configuring error logging and notifications
description: The developer guide to configuring error logging and notifications in Virto Commerce
layout: docs
date: 2016-03-11T15:33:47.713Z
priority: 8
---
## Configuring NLog

For logging errors both of VirtoCommerce Manager and VirtoCommerce Storefront are using <a href="http://nlog-project.org/" rel="nofollow">NLog</a> - a free logging platform for .NET.

NLog configuration can be performed in special **&lt;nlog&gt;** sections of web.config files of VirtoCommerce.Storefront (Storefront solution) and VirtoCommerce.Platform.Web (Manager solution) projects.

Here is a simple configuration example for logging errors in a columned CSV-file named with a short formated date which will be stored in a projectвЂ™s root folder:

```
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" throwExceptions="true" autoReload="true">
  <targets>
    <target name="csv" xsi:type="File" fileName="${basedir}/${shortdate}.csv">
      <layout xsi:type="CSVLayout">
        <column name="time" layout="${longdate}" />
        <column name="logger" layout="${logger}"/>
        <column name="message" layout="${message}" />
        <column name="stack" layout="${stacktrace}"/>
      </layout>
    </target>
  </targets>
  <rules>
    <logger name="*" writeTo="csv" />
  </rules>
</nlog>
```

<a href="https://github.com/nlog/nlog/wiki" rel="nofollow">Full NLog documentation with tutorials and API references</a>

## Displaying error notifications on Storefront

To notify users about errors while AJAX calls VirtoCommerce Storefront is using a special alert popup.

![](../../assets/images/docs/capture-0.png)

All MVC methods that receive requests and send responses for AJAX calls are separated to special controllers in Controllers.Api namespace of VirtoCommerce.Storefront project and these controllers are marked with an attribute HandleJsonError like this:

```
[HandleJsonError]
public class ApiCartController : StorefrontControllerBase
{  }
```

So when an exception is occurred it will be serialized to JSON and sent to client.

On client side app.js contains a httpErrorInterceptor which handles HTTP requests and responses errors and propagate a global Angular application event storefrontError with serialized exception details from API controller.

In main.js there is a $scope method which is listening for event storefrontError and populate $scope.storefrontNotification object with error details. This object is using on a notification snippet notification.liquid which is placed on layouts used in storefront theme (theme.liquid and checkout_layout.liquid) inside body section.

```
<body ...>
  {% raw %}{% include 'notification' %}{% endraw %}
</body>
```

When object $scope.storefrontNotification is not null the notification alert will appear.

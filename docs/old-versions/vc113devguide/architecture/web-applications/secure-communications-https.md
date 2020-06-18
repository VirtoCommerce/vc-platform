---
title: Secure communications (https)
description: Secure communications (https)
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 5
---
VirtoCommerce is designed to work in secure (SSL) and standard (insecure) protocols (using https or http respectively). The simplest scenario is publishing VirtoCommerce as aВ standard website. It's accessible from a browser as a regular website and the Commerce Manager (CM) uses it's default URL while connecting to it.

In order to enable secure communications, only little additional web server configuration is needed. Follow this article to configure it on IIS:В <a href="http://msdn.microsoft.com/en-us/library/hh556232(v=vs.110).aspx" rel="nofollow">How to: Configure an IIS-hosted WCF service with SSL</a>. Remarks:

* If you want your store to be accessible both in http and https, you should skip the "Configure Virtual Directory for SSL" section.
* The "Configure WCF Service for HTTP Transport Security" sectionВ addresses changes to web.config. Ignore the sample XML code and update web.config as following:
  * changeВ every
    ```
    <security mode="None">
      <message clientCredentialType="None" />
    </security>
    ```
    to
    ```
    <security mode="Transport">
      <transport clientCredentialType="None"/>
    </security>
    ```
  * The original web.config file has these scripts already, just some parts are commented out. If you comment out the unwanted and uncomment theВ necessary parts, the final code could be like this:
    ```
    <binding name="NonAuthenticationServiceBinding" maxReceivedMessageSize="2147483647">
      <!-- configuration for services in HTTP -->
      <!--<security mode="None">
        <message clientCredentialType="None" />
      </security>-->
      <!-- Comment HTTP and uncomment this section for services switching to HTTPS/SSL. -->
      <security mode="Transport">
        <transport clientCredentialType="None"/>
      </security>
    </binding>
    <binding name="AuthenticationServiceBinding" maxReceivedMessageSize="2147483647">
      <readerQuotas maxDepth="32" maxStringContentLength="8192" maxArrayLength="506384" maxBytesPerRead="4096" maxNameTableCharCount="16384" />
      <!-- configuration for services in HTTP -->
      <!--<security mode="None">
        <message clientCredentialType="None" />
      </security>-->
      <!-- Comment HTTP and uncomment this section for services switching to HTTPS/SSL. -->
      <security mode="Transport">
        <transport clientCredentialType="None"/>
      </security>
    </binding>
    ```
  * Don't forget to save the file when done.

Now you can manage your store securely trough CM. Just remember to useВ https://В for "Base URL" while connecting:

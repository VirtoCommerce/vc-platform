---
title: Azure development
description: Azure development
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 4
---
This documents describes some specific related to developing on azure platform.

## Configuration

Settings that are typically stored in web.config or connectionstrings.config are stored in theВ ServiceConfiguration.{deployment}.cscfg when running in azure. Settings are stored there to allow changing basic settings like database connection string during runtime without a need to redeploy. This is very useful when you want test code on staging environment against both test database and production database before doing a swap.

The deployment package includes two such configuration files one called ServiceConfiguration.Local.cscfg and anotherВ ServiceConfiguration.Cloud.cscfg. Local is used to run in emulator in dev machine and Cloud for remote deployment.

### Implementation

To make it work, instead of standard configuration manager we use CloudConfigurationManager to get settings. That class will automatically check what environment framework is deployed and will get settings from the correct place (web.config for on-premises and cscfg when on azure). We also introduce some helper classes to deal with connection strings.

> Connection strings in azure are for blob storage connection strings and the actual database connection strings should be specified using app settings

Here is an example of how to get connection string:

```
ConnectionHelper.GetConnectionString(SecurityConfiguration.Instance.Connection.SqlConnectionStringName) // this gets connection string or returns connection string name
```

Sometimes it is necessary to check if the code is executed locally or in the azure environment, for that purpose we provide AzureCommonHelper class:

```
if (AzureCommonHelper.IsAzureEnvironment()) // you can't use RoleEnvironment.IsAvailable directly, since RoleEnvironment is not available when running azure websites
{
  // continue with azure
}
```

## Elastic Search Worker Role

Role that runs Elastic Search using tomcat java http server. JRE7 is distributed with a role as well.

### Settings

* ESLocation - by default 2096MB, stores JRE, tomcat and Elastic Search binaries
* CloudDriveSize - by default 1096MB, persistent storage that contains index data folders, should be increased for larger catalogs.
* DataConnectionString - connection string used by the CloudDrive to save and restore data from. Can't be https.

### Installation

When worker role is deployed, it goes through the installation process:

1. Virtual Hard Drive is created where all the indexes are stored.
2. Storage directories for data and indexes created.
3. Runtime directory is created and jre, tomcat and elastic search binaries are copied.
4. The process is started with all the IP's (if multiple instances are deployed the search is created as a cluster).

### Troubleshooting

By default the elastic search worker role is distributed with windows diagnostics enabled for all operations. So simply opening diagnostics logs should be enough to find any issues.

---
title: Security and Users
description: Security and Users
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 2
---
## Types of users

There are 4 different types of users in the system which define the actions each user can perform. These types are:

* Guest User
* Registered User
* Administrator
* Super Administrator

From these types only Administrators can access and use Commerce Manager / Administration console.

## Working with Permissions

SomeВ functionalityВ is available only if user has corresponding Permission. Initial set of permissions is created during DB setup. Permissions are stored in Permission datatable. Developer can add new Permissions by creating new rows in the Permission datatable directly. The PermissionId should be used for checking if user has access to restricted information orВ functionality. Depending onВ PermissionId, Permissions areВ dividedВ into groups inside [Commerce Manager roles management UI](docs/old-versions/vc113userguide/users-management-roles-and-role-assignment). The rules for Permission grouping:

1. ifВ PermissionId ends with ":config", the group is "Settings". Usually this group includes Permissions for Commerce Manager Settings section.
2. the group name is the beginning of PermissionId until a ":" delimiter is reached. E.g. permission named "Manage Catalog Items" has PermissionIDВ "catalog:items:manage" and belongs to "catalog" group.

The group and Permission names are localizable. That means a manager can give any name to a Permission or Permission group using [Localization UI](docs/old-versions/vc113userguide/settings/application-settings).

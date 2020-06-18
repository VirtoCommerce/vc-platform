---
title: Users Management, Roles and Role Assignment - Virto Commerce 2 User Guide
description: The article about users management, roles and role assignment in Virto Commerce
layout: docs
date: 2015-09-19T00:19:32.537Z
priority: 5
---
## Introduction

Users are individuals working with <a class="crosslink" href="https://virtocommerce.com/b2b-ecommerce-platform" target="_blank">Virto Commerce</a> management application and services. They are internal to the organization and should not be confused with customers.

In order to manage Virto Commerce functionality each user must be assigned to at least one role. Each role provides the user with certain access permissions.These permissions allow or restrict the user's access to functionalities within the Virto Commerce client application. E.g. user can be permitted to view catalog Items but restricted to manipulate Items. For example, these permissions could be enabled to ensure that the Catalog functionality is not available to warehouse users who do not have Catalog Management related roles.

Common Roles are provided by default with each Virto Commerce installation. User roles can be created or edited to provide their staff with permissions reflecting only the tasks they perform within the Virto Commerce management application.

## User management and permissions assignment

This section will explain the different security considerations that affect the Virto Commerce management application and the steps you need to take to ensure that permissions are appropriately assigned for users.

This section is intended to assist Virto Commerce administrators with setting up proper permissions on application data and functionality. These individuals must at least be able to create Users and User Roles in the Virto Commerce administration tool, so they must have User Management permissions in the Virto Commerce administration tool.

## Permissions

Permissions are controlled by assigning Roles to users. A Role is a collection of permissions. A Role can be assigned to multiple users. Each user can have more than one assigned Role.

Through the combination of assigned Roles, you can ensure that users only have access to the information and functionality they need.  
This model provides considerable flexibility, but in complex environments with multiple stores and catalogs, each with their own distinct sets of users, it is critical to establish best practices for managing permissions.

## Users

All individuals in your organization who need access to the Virto Commerce administration tool should have their own personal user accounts. Each user has certain permissions, defined by their role, which are specified during the user’s profile creation. These Virto Commerce users should be configured so that they only have access to the data they need to perform their tasks. See the Roles section for more information.

## Administrator users

After installation, there is one user in the system with Super User (administrator) privileges. Only administrator users have permission to manage users and roles.  

> It is strongly recommended that you create a second user with Super User privileges. If one of the administrator user accounts is locked, the other administrator user will be able to unlock it.

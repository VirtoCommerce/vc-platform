# Registration step scope

Account registration is a scenario that results with the appearance of a new active account (that can be a physical body for B2C case or a company for B2B case) in the system and the user who can normally act in the system on behalf of this account.

## Overview


*Describe main entities definitions, navigation, indexation, etc.*

### Main entities

Accounts data are managed in the Contact module. Native Virto Commerce functionality contains the following type of accounts and contacts:

Type | Description | 
--- | --- 
Organization | Represents a buyer organization in the system, an account (in business terms) in B2B case. Is a container for other organizations (brunches or sub-companies) and contacts.
Contact | Represents a physical body that can be an account (in business terms) by himself in B2C case or may belong to one or more organization accounts in B2B case. 
Vendor | Represents a vendor organization in the system, can not contain other entities (vendors or contacts) by default.
Employee | Represnts the seller (system owner) employee. May belong to one or more organization accounts in B2B case.  

It is important to understand that *users*, i.e. the security entities that has access to the web store is a dedicated entity, that is linked by relation but physically decupled from contact.

Each user is linked with the set of *roles* that define permission level for each store.

It means that a contact may be linked with few (any amount) users, where each user belongs to the only store.

Type | Description | Relations
--- | --- | ---
User | A Security entity that has access to the store according to permissions defined by a role | (one) contact <-> (many) users; (one) user <-> (many) roles
Role | A Security entity that represents a collection of atomic permissions | (many) roles <-> (many) permissions

### Indexation

Organizations, contacts, vendors, and employees are kept in the index.


## Atomic functions / scenarios
 

 Name | Module | Description | Link 
--- | --- | --- | --- 
 Organization management | Customer | How to manage an organization | (module link doesn't work) 
 Employee management | Customer | How to create and manage employee | (module link doesn't work) 
 Contact management | Customer | How to create and manage contact | (module link doesn't work) 
 Vendor management | Customer | How to create and manage a vendor | (module link doesn't work) 
 Address management | Customer | How to manage addresses | (module link doesn't work) 
 User management | Security | How to manage users | (module link doesn't work) 
 Roles management | Security | How to manage roles | (module link doesn't work) 
 AuthApplications | Security | Overview | (module link doesn't work) 
 Indexation | (module) | How to run indexation for organization, contacts, employees, and vendors | (add the link)

## Extensibility

### Declarative extensibility
<>

### Code extensibility

Name | Module | Description | Link
--- | --- | --- | --- 
Extend Organization | Customer | Add field to organization entity | [Extend Organization](https://virtocommerce.com/docs/latest/fundamentals/extensibility/extending-domain-models/)
Extend Contact | Customer | Add field to contact entity | [Extend Contact](https://virtocommerce.com/docs/latest/fundamentals/extensibility/extending-domain-models/)
Extend Vendor | Customer | Add field to vendor entity | [Extend Vendor](https://virtocommerce.com/docs/latest/fundamentals/extensibility/extending-domain-models/)
Extend Employee | Customer | Add field to employee entity | [Extend Employee](https://virtocommerce.com/docs/latest/fundamentals/extensibility/extending-domain-models/)

### Reactive programming extensibility

#### Events

Name | Module | Description | Link
--- | --- | --- | ---
OLEG to do | Customer | (event business description) | (add link to dev doc)
OLEG to do | Security | (event business description) | (add link to dev doc)

#### Webhooks

Name | Module | Description | Link
--- | --- | --- | ---
OLEG to do | Customer | (webhook business description) | (add link to dev doc)
OLEG to do | Security | (webhook business description) | (add link to dev doc)

## User scenarios 

### XAPI built-in scenarios

Following built-in business API scenarios can be used by front-end developers "as is" when create registration user experience

Name | Description | Link
--- | --- | ---
Create Contact | Creating a contact entity | [CreateContact](https://virtocommerce.com/docs/latest/modules/experience-api/x-profile-reference/#createcontact)
Create User | Creating a user entity | [CreateUser](https://virtocommerce.com/docs/latest/modules/experience-api/x-profile-reference/#createuser)

### Custom user scenario examples

Name | Description | Link
---| --- | ---
Organization registration with verification | User sends a request for organization registration. Before the moment of manual organization verification a user can not login to the store | Contact VC team to get more info



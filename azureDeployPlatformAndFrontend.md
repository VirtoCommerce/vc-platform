# Deploy To Azure Button

## Overview
Deploy To Azure button deploys Virto Commerce Platform and Virto Commerce Frontend to Azure. The deployment consists of the following components:
* Virto Commerce Platform with default set of modules
* Virto Commerce Frontend
* Azure Application Gateway

## Process
The Azure deployment process consists of the following steps:

1. Create all Azure resources using the Azure button.
2. Set up the initial configuration for the platform application (install sample data if necessary; set a new password for the Admin user).
3. Add a store URL (`Stores > B2B-store > Store URL`) pointing to the Azure Application Gateway IP address in the form of `http://x.x.x.x/`.
4. Use the Azure Application Gateway IP address to access the front-end part of the Virtocommerce solution.

## References
* [Deploy to Azure Documentation](https://docs.virtocommerce.org/storefront/developer-guide/deployment/#deployment-on-azure)
* [Virto Commerce Frontend Architecture](https://docs.virtocommerce.org/platform/developer-guide/architecture/)


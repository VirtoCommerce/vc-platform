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
2 Optional. If you want to use the Page builder functionality, the front-end application should use the https protocol. To set this up:
    a. add an https listener to the Application gateway
        go to Settings -> Listeners -> Add listener -> Fill in `Listener name` and `Cert name`, set `Protocol` to `HTTPS` and `Port` to `443`. Select .pfx file in `PFX certificate file` and enter the password for the certificate.
    b. change a listener for routing rule
        go to Settings -> Rules -> <routning rule> -> set listener to `https`
3. Add a store URL (`Stores > B2B-store > Store URL`) pointing to the Azure Application Gateway IP address in the form of `http://x.x.x.x/` (in case of using https, use form `https://<hostname-from-certificate>/`).
4. Use the Azure Application Gateway IP address to access the front-end part of the Virtocommerce solution.


## References
* [Deploy to Azure Documentation](https://docs.virtocommerce.org/storefront/developer-guide/deployment/#deployment-on-azure)
* [Virto Commerce Frontend Architecture](https://docs.virtocommerce.org/platform/developer-guide/architecture/)


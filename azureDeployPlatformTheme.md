The Azure deployment process consists of the following steps:
1. Create all Azure resources using the Azure button.
2. Set up the initial configuration for the platform application (install sample data if necessary; set a new password for the Admin user).
3. Add a store URL (`Stores > B2B-store > Store URL`) pointing to the Azure Application Gateway IP address in the form of `http://x.x.x.x/`.
4. Use the Azure Application Gateway IP address to access the front-end part of the Virtocommerce solution.
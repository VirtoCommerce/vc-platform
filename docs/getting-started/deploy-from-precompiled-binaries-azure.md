# Deploy to Azure
Use this guide to <a class="crosslink" href="https://virtocommerce.com/ecommerce-hosting" target="_blank">deploy</a> to Azure and configure Virto Commerce Platform.

!!! note
    You can install modules and sample data later from Virto Commerce Admin UI.

Virto Commerce Team creates default ARM template [azuredeploy.json](https://github.com/VirtoCommerce/vc-platform/blob/master/azuredeploy.json) to simplify deployment of Virto Commerce Platform to Azure.
You can use this template as base for your custom solution.
Read [Azure Resource Manager documentation](https://docs.microsoft.com/en-us/azure/azure-resource-manager/).

The following resources will be created during deployment:
 
* Azure Storage - as Primary blob storage for CMS and Assets.
* SQL Azure - database.
* Website - Virto Commerce Platform.


## From GitHub
This sections describes how to use the Deploy to Azure button to deploy Virto Commerce Platform from a GitHub repository.

!!! note
    * When a user clicks on the **Deploy to Azure** button, a “referrer” header is sent to azuredeploy.net which contains the location of the Git repository to deploy from.
    * This scenario is good if you want to install Virto Commerce platform from the sources.

!!! tip
    * Deploy problem can be related of using “Free” plan, change to Basic or Standard. [Read More](https://www.virtocommerce.org/t/deploy-virto-commerce-to-azure-and-getting-a-deployment-failure/72).
    * Your login name must not contain a SQL Identifier or a typical system name (like admin, administrator, sa, root, dbmanager, loginmanager, etc.) or a built-in database user or role (like dbo, guest, public, etc.)
    * Your password must be at least 8 characters in length.
    * Your password must contain characters from three of the following categories – English uppercase letters, English lowercase letters, numbers (0-9), and non-alphanumeric characters (!, $, #, %, etc.).

This sections describes how to deploy Virto Commerce Platform from Custom [azuredeploy.json](https://github.com/VirtoCommerce/vc-platform/blob/master/azuredeploy.json) ARM Template.

To run the deployment of Platform, select the following button:

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FVirtoCommerce%2Fvc-platform%2Fmaster%2Fazuredeploy.json)

1. The portal displays a pane that allows you to easily provide parameter values. The parameters are pre-filled with the default values from the template.
    ![Deploy to azure - custom template](../media/deploy-to-azure-custom-wizard.png)
1. Select Subscription and Fill required parameters.  
1. Select **Purchase**. 
    ![Deploy to azure - custom template](../media/deploy-to-azure-customtemplate-purchase.png)
1. Wait until the deployment has been completed.
1. Open url in your browser. 
1. The application will create and initialize database on the first request. After that you should see the sign in page. Use the following credentials:
    * User Name: **admin**
    * Password: **store**
    ![Deploy to azure](../media/deploy-to-azure-vc-login.png)
1. Install Commerce modules and restart the platform.
    ![Deploy to azure](../media/deploy-to-azure-vc-restart.png)
1. Install sample data if required. 

## Storefront from GitHub

This sections describes how to deploy Virto Commerce Storefront from Custom [azuredeploy.json](https://github.com/VirtoCommerce/vc-platform/blob/master/azuredeploy.json) ARM Template.

!!! note
    The Virto Commerce Platform should be installed. 

To run the deployment of Storefront, select the following button:

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FVirtoCommerce%2Fvc-storefront%2Fmaster%2Fazuredeploy.json)

!!! tip
    How to get Content Storage Connection String:

    1. Open [Azure Portal](https://portal.azure.com/)
    1. Select **Storage Account** which you created when deploy the Virto Commerce Platform.
    1. Select **Access Keys** and copy **Connection string**. Ex:
        `DefaultEndpointsProtocol=https;AccountName=vc...`
    1. Add prefix **provider=AzureBlobStorage;rootPath=cms;**
    1. So, Content Storage Connection String is: 
        `provider=AzureBlobStorage;rootPath=cms;DefaultEndpointsProtocol=https;AccountName=vc...`

1. The portal displays a pane that allows you to easily provide parameter values. The parameters are pre-filled with the default values from the template.
    ![Deploy to azure - custom template](../media/deploy-to-azure-custom-wizard-storefront.png)
1. Select Subscription and Fill required parameters.  
1. Select **Purchase**. 
1. Wait until the deployment has been completed.
1. Open url in your browser.  


## Deploy via PowerShell
Read [Azure Resource Manager documentation](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/deploy-powershell) to
deploy Virto Commerce Platform from Custom Template [azuredeploy.json](https://github.com/VirtoCommerce/vc-platform/blob/master/azuredeploy.json) to Azure via PowerShell.

## Deploy via Azure CLI
Read  [Azure Resource Manager documentation](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/deploy-cli) to
deploy Virto Commerce Platform from Custom Template [azuredeploy.json](https://github.com/VirtoCommerce/vc-platform/blob/master/azuredeploy.json)  to Azure via CLI.

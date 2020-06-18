---
title: Creating catalog custom import-export extensions
description: Virto Commerce catalog module has the ability to extend import/export functionality with new data formats. This method can also be used to integrate with other systems to migrate data
layout: docs
date: 2016-04-08T14:39:11.363Z
priority: 1
---
Virto Commerce <a class="crosslink" href="https://virtocommerce.com/product-information-management-software" target="_blank">catalog module</a> has the ability to extend import/export functionality with new data formats. This method can also be used to integrate with other systems to migrate data.

To write your own import or export extension you'll need to perform the following steps:
* **Create module**В /В [How to create new module](docs/vc2devguide/working-with-platform-manager/extending-functionality/creating-new-module) with VCF catalog module dependency
* **Register your extension** in catalog export or import extensions list (read below)
* **Register your UI notification template** (if you want to use custom notification template for progress information) [understanding push notifications](docs/vc2devguide/working-with-platform-manager/basic-functions/push-notifications)
* **Define API and code logic**В that is executed on catalog import/export data operation. To get more information about VCF catalog structure please readВ [Virto Commerce Catalog module](docs/vc2userguide/merchandise-management/products-catalog)

## Register extension in catalog export / import extension listВ 

In your main module script file **your_module.js** in run method inject dependency with name **virtoCommerce.catalogModule.catalogExportService** or **virtoCommerce.catalogModule.catalogImportService**.

```
run(['virtoCommerce.catalogModule.catalogImportService', function catalogImportService) {
....
}];
```

Next, register into injected service your extension

```
catalogImportService.register({
  //Represent your extension title in the extensions list
  name: 'VirtoCommerce CSV import',
  //Detailed description
  description: 'Native VirtoCommerce catalog data import from CSV',
  icon: 'fa fa-file-archive-o',
  //extension blade controller which will be displayed when user  choose you extension 
  controller: 'virtoCommerce.catalogModule.catalogCSVimportWizardController',
  //template for extension blade
  template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/import/wizard/catalog-CSV-import-wizard.tpl.html'
});
```

After you created the module installation in target system the extension should be available in the catalog export or import extension list. 

When a user selects it in the extension list, the system will display the extension blade and pass all information about the user's previously selected catalog objects (**blade.catalogId**, **blade.categoryIds**, **blade.productIds**).

Export or import process can be a long running process. So, we recommended to execute all logic with data transformation in background jobs.

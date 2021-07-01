# B2B multi-regional

## Technical view on B2B multi-regional ecommerce architecture

One of the most important and frequent B2B ecommerce business challenges is a growth to new regions, countries and unions of states. When expanding geographically, associated costs are critical to the business, especially until sales in local markets begin. 
Virto Commerce allows you to avoid wasteful costs of duplicating the platform for local markets. Do all the work with one team in all markets.
This article describes an architectural solution for multi-regional stores based on the Virto Commerce platform.

## Potential use cases

This solution is targeted to provide multi-regional support that shares the same master catalog and to have one team managing multiple regions stores.

- Multi regional store with the same or similar assortment.
- Multi country store with the same or similar assortment but different pricelists in different currencies.
- Multi country store with the same or similar assortment but with different localized content such as name, catalog item descriptions etc.
- Mix of the above.

## Architecture diagram

![B2B Multiregional Architecture Reference](../../media/arch-center-b2b-multiregional.png)

In this architecture diagram three webstores targeted to customers in different regions shares the same Virto Commerce backend integrated with one or more third-party systems (such as ERP, PIM, etc.)

Webstores represents the salespoints for the customers from different regions. Customers of different regions use different URLs, utilize the webstore localized to the language of the region including catalog items properties and description. They have prices in the currency of their local region.

Virto Commerce backend has a set of native modules or extended modules. The diagram focuses on the Catalog and Pricelist modules.

Catalog module supports master catalogs. These are the physical catalogs that store master data of the product items. Master catalogs can be used directly to assign to one or multiple regions. In the situations when master catalog structure and contents doesn’t meet the region requirements, Virto Commerce catalog module supports virtual catalog model. Virtual catalog can be created for a specific region (EU) and contain subset of items from the master catalog.

## Components on the diagram

The following modules are included to discussed multi-regional ecommerce solution,

- __Webstore__ — webstore is a salespoint of the store. It can be a website, an application or other.

- __Virto Commerce__ — platform with the native, extended, or new customer specific modules.

- __Catalog module__ — catalog module responsible to run CRUD operations to build seller’s product catalog structure.

- __Master catalog__ — seller’s catalog assortment master data that can be either uploaded via .CSV file import or via RESTful API. Master catalog can be enriched with additional properties from the backoffice. Master catalog can be used for multiple stores configured in Virto Commerce. In the scheme master catalog is used directly for US and CA webstores.

- __Virtual catalog__ — virtual catalog that can be used to build subset or combination of items from one or more master catalogs. This tool adjusts the catalog structure to meet the specific region requirements. The virtual catalog can be created and used specifically for EU region webstore [in the architecture scheme].

- __Pricelist module__ — pricelist module responsible to manage specific webstore, region, catalog, customer group or specific customer prices. Pricelist module supports multi-currency. That way it’s possible to upload and manage pricelists in different currency for different region. In the scheme pricelists for three different currencies are managed (USD – for US webstore, CAD – for CA webstore and EUR – for EU webstore).

- __Integration middleware__ — integration middleware is used to transform API message to support master data or transactional data exchange between Virto Commerce and a third-party system(s). Logic App can be used as an integration middleware.

- __Third-party systems__ — these are ERP, PIM other systems.


## Conclusion

Virto Commerce architecture allows a company to expand its regional presence quickly, flexible and inexpensive. The company can launch its stores in different regions on different languages and currencies, while maintaining a single ecosystem of applications and a single process for order processing and customer service.
A huge advantage of Virto Commerce architecture is ability to scaling ecommerce to regions without multiple platform installations for the regional markets. This provides savings in both IT infrastructure and personnel. The same IT team can serve the platform for all markets, adding only staff with language skills as needed to support local customers.
On other competing platforms, in order to launch a new regional online store, you need to copy the ecommerce platform for each new region, which means, in fact, duplication by the IT resources and team, i.e., a duplication the cost of everything.
And the Virto Commerce architecture avoids cost duplication natively. You can launch a new ecommerce business in new regions, but the infrastructure that serves it remains the same or grows insignificantly. The best practice for scaling the Virto Commerce ecommerce platform to new regional markets is to install it at a cloud provider such as Microsoft Azure, AWS, Google Cloud, Alibaba Cloud.

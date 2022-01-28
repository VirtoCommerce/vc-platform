# Swagger/OpenApi

Swagger UI allows anyone — be it your development team or your end consumers — to visualize and interact with the Virto Commerce REST API’s resources without having any of the implementation logic in place.

It’s automatically generated from Virto Commerce Modules, with the visual documentation making it easy for back end implementation and client side consumption.

## Swagger UI

Swagger UI available in Admin portal by `/docs` path.

Ex: `https://{admin-portal-domain-url}/docs`

![](../media/swagger-ui.png) 

## Platform endpoint
Complete Swagger/OpenApi endpoints for all modules in platform available by `/docs/PlatformUI/swagger.json` path.

## Module endpoint
Swagger/OpenApi endpoints for a module resides at urls like `/docs/{module-id}/swagger.json`.

Example: `https://{admin-portal-domain-url}/docs/VirtoCommerce.Orders/swagger.json`

## Disable Swagger UI
By default, Swagger UI is active. But it can be disabled by [application settings](../user-guide/configuration-settings.md) in VirtoCommerce::Swagger node.

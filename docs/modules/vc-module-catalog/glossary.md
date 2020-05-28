# Glossary

**SKU**- Stock Keeping Unit
SKUs are used by catalogs, online e-tailers, warehouses, and product fulfillment centers to track inventory levels in order to determine which products require reordering. Although SKUs differ from model numbers, businesses may embed model numbers within SKUs.

**Important**: By adding SKUs to every product variation, store owners can easily track the quantity of available products and create threshold limits to let them know when new purchase orders must be made.

**SKU selector** is a visual element that enables selecting product variations.

**GTIN**- see [Manage Products](/docs/manage-physical-products.md) Global Trade Item Number.

GTIN can be used by a company to uniquely identify all of its trade items.  Currently, GTIN is used exclusively within bar codes.

**Can be purchased** - see [Manage Products](/docs/manage-physical-products.md). If this parameter is set for a product on admin side, the item will be available for purchasing on store side. If this parameter is not set on admin side, the item may be visible but not available for purchasing.

**Store visible**- see [Manage Products](/docs/manage-physical-products.md). If this parameter is set on admin side, the product will be visible on Store. If this parameter is not set on admin side, it will not be visible for clients.

**Track inventory**- see [Manage Products](/docs/manage-physical-products.md). Setting this parameter allows the system start tracking the inventory. If this parametr is not set, the inventory will not be tracked by the system.

**Physical Product** is a product that can be purchased and shipped

**Digital Product** is a product that can be purchased, but not shipped.

**Is Active**- see [Manage categories](/docs/manage-categories.md)

**Category** is a container for other categories or items (products).

**Priority** - see [Manage categories](/docs/manage-categories.md)

**Product Property** is an additional characteristics for a catalog entity. All properties defined in parent hierarchy entity are inherited by child items.

**Product Variation** is a variation of the main product that has additional properties.

**Mapping**  is the process of defining how a document, and the fields it contains, are stored and indexed.

**Search engine** is a set of applications designed to search for information.

**Elastic search** search engine from json REST API, uses Lucene and written in Java.

**Index** is a database, document is a table in it. The document is document format JSON, which is stored in elasticsearch. Each document is stored in the index, and has the type and ID. The original JSON document indexing will be stored in the field_source that returns a default receipt or document search.

**Analysis** is the process of converting text into tokens or terms which are added to the inverted index for searching. Analysis is performed by an analyzer, which can be either a built-in analyzer or a custom analyzer defined per index.

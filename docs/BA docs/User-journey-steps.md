# Customer journey steps

After designing the desirable customer journey flow or some dedicated steps of this flow a Business Analyst find the relevant customer journey step in the table below.

Than, the Business Analyst checks the related page(s) mentioned in the "Link" column to see what related functionality is provided by Virto Commerce and identify what should be extended to cover gaps if they exist.

Customer steps in the front end are implemented with using scenarios or commands that are built-in into experience API. On the relevant page(s) you may find information about related XAPI commands, examples of complex custom user scenarios, explanation of useful atomic backend functions, and explanation of possible ways of extending the system.


## Awareness phase

Step | Description | Link
--- | --- | ---
SEO | SEO data delivery to the front-end in experience API. |  [Expereince API](https://docs.virtocommerce.org/modules/experience-api/x-catalog-reference/#querying-product-breadcrumbs)
Social | Integration of Virto Commerce with third-party solutions and services including social media | [Integrations](https://virtocommerce.com/integrations/key-ecommerce-integrations), [Expereince API](https://docs.virtocommerce.org/modules/experience-api/x-catalog-reference/#querying-product-breadcrumbs)
Remarketing | Integration for remarketing | [Integrations](https://virtocommerce.com/integrations/key-ecommerce-integrations), [Expereince API](https://docs.virtocommerce.org/modules/experience-api/x-catalog-reference/#querying-product-breadcrumbs)
Newsletter | Integration of Virto Commerce with third-party solutions and services including email marketing software of your choice| [Integrations](https://virtocommerce.com/integrations/key-ecommerce-integrations), [Expereince API](https://docs.virtocommerce.org/modules/experience-api/x-catalog-reference/#querying-product-breadcrumbs)

## Consideration phase

Step | Description | Link
--- | --- | ---
Homepage | Homepage should be designed in the front end (would it be a storefront, CMS solution, or other) and integrated with Virto Commerce experience API if required | [Expereince API](https://docs.virtocommerce.org/modules/experience-api/)
Landing page | Landing pages should be designed in the front end (would it be a storefront, CMS solution, or other) and integrated with Virto Commerce experience API to deliver eCommerce data like product information, prices, or others. | [Expereince API](https://docs.virtocommerce.org/modules/experience-api/)
Navigation / main menu | Navigation menu should be designed and managed in the front end (would it be a storefront, CMS solution, or other) and integrated with Virto Commerce experience API to deliver required eCommerce data like catalogs, categories, or other| [Expereince API](https://docs.virtocommerce.org/modules/experience-api/)
Forms | Formes should be designed and managed in the front end (would it be a storefront, CMS solution, or other) and integrated with Virto Commerce experience API if required. | [Expereince API](https://docs.virtocommerce.org/modules/experience-api/)
Product search | Product search scenarios for front-end in experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/) 
Product page | Product information browsing scenarios for front-end in experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/) 
Pricing | Product prices browsing scenarios for front-end in experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/) 
Inventory | Product inventory browsing scenarios for front-end in experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/) 
Promotions | Promotion scenarios for front-end in experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/) 
Wishlists | Implementing wish-list scenarions (development required) as a modified Cart instanses | [Cart module](https://docs.virtocommerce.org/modules/cart/) 
Static content | Static content should be designed in the front end (would it be a storefront, CMS solution, or other) and integrated with Virto Commerce experience API if required | [Expereince API](https://docs.virtocommerce.org/modules/experience-api/)
Mobile app | Integration with any touchpoint, including mobile APP or PWA through the experience API.| [Expereince API](https://docs.virtocommerce.org/modules/experience-api/)
Omnichannel (POC, CS) | Integration with any touchpoint, including POC, CS, or other through the experience API.| [Expereince API](https://docs.virtocommerce.org/modules/experience-api/)

## Acquisition phase

Step | Description | Link
--- | --- | ---
Shopping cart | Shopping cart scenarios for front-end in experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/)
Upsell | Upsell and cross-sell tools: products associations, dynamic products associations, personalization | [Products associations](https://docs.virtocommerce.org/modules/catalog/), Dynamic Marketing Segments, Dynamic Product Associations, [Personalization](https://docs.virtocommerce.org/modules/catalog-personalization/)
Unavailable items | Making products unavailable or invisible | [Unavailable product](https://docs.virtocommerce.org/modules/catalog/)
Registration | Buyer registration scenarios: physical buyer user (B2C), organization (B2B) with different levels of organizational complexity| [Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/)
Login | Login scenarios in experience API| [Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/)
Guest visitor | Guest visitor scenarios for front-end in experience API. Creating specific logic for such kinds of visitors: full access, no access, limited access (no prices, no cart, other). How to configure and extend these scenarios| [Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/)
Quick checkout | Quick checkout scenarios for front-end in experience API.  | [Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/)
Delivery options | Delivery selection scenarios for front-end in experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/)
Payment options | Payment options selection scenarios for front-end in experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/)
Invoicing | Invoices availability scenarios for front-end in experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/)  |
Taxation | Taxation-related scenarios for front-end in experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/) 
Checkout | Order review and confirmation |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/) 
Shipping tracking | Scenarios for delivery of shipment tracking information in front-end through the experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/) 
My account data | Scenarios for account data management in front-end through the experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/) 
Order history | Scenarios for order history delivery in front-end through the experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/)
Customer company data (B2B) |Scenarios for organization data management in front-end through the experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/) 
Register additional users in B2B |Scenarios for registering of new users inside the organization in the front-end through the experience API. |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/) 
B2B assigning roles to users | Assigning specific role (pre-defined on a seller side) by organization admin to organization member so that the member has limited access to B2B store functionality or data |[Experience API](https://docs.virtocommerce.org/modules/experience-api/x-profile-reference/) |

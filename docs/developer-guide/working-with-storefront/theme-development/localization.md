---
aliases:
  - docs/vc2devguide/working-with-storefront/how-to-localize-storefront
date: '2017-09-04'
layout: docs
title: 'How to localize theme'

---
## Adding new translation to theme

Storefront theme localization is very similar to [VirtoCommerce Platform localization](docs/vc2devguide/working-with-platform-manager/localization-implementation). Check it for details on working with translation files.

1. Make a copy of &lt;*theme repository location*&gt;\locales\en.default.json file 
2. Rename the copied file to begin with your needed language 2 letter code (e.g., "es.default.json"). 
3. Translate the file content.

## Adding new language to store
1. Open your store in VirtoCommerce Platform ( **More в†’ Browse в†’ Stores в†’ &lt;*your store*&gt;** ).
2. Check whether your language exists in the "Additional languages" available values list or add it in case it's missing:
![](../../assets/images/docs/store-languages.gif)

---
title: Style Guide
description: The article contains Virto Commerce style guide
layout: docs
date: 2015-09-04T00:10:30.440Z
priority: 3
---
## Introduction

Virto Commerce Platform Style Guide will help you develop consistent modules for the platform. You can use existing styles described in our online [Style Guide](https://virtocommerce.com/styleguide/index.html)[В ]()or you can add additional styles using the rules described below.

* [Style Guide](https://virtocommerce.com/guides/style-guide)В - guidance on css and html to be used when creating UI for the platform
* [Blade Constructor](https://virtocommerce.com/guides/blade-constructor) - use to create your custom blades

## Rules

This methodology was developed so every developer can easily make changes to CSS styles in a consistent manner. There is a set of rules to follow in order to keep the CSS in a consistent state:

### Naming

Module name should match the module to have good understanding of what it stands for.

### Inheritance

Inner module classes should be implemented only as part of the module:

.block-module {}

.block-module .element-module {}

.block-module {}

.element-module {}

### Layering

There are four module layers styles:

* *Reset.css*:В Base of styles. Default styles reseted, fonts set, base sizes are set in this layer.В 
* *Base modules.css*:В Base elements, forms and buttons are defined in this layer.
* *Project modules.css*:В Module styles isolation layer. Concrete module style defined in this layer.
* *Cosmetic.css*:В Minor modifications of colors, and links are defined here.

## Naming conventions

"-" - word separator (eg. вЂњinput-fieldвЂќ)

"_" - logic part separator (eg. вЂњtoolbar_logoвЂќ)

"__" - separator modifier (eg. вЂњmodule_list.__modifierвЂќ)

## Module modifier example

Say you have a module defined:

```
.module {}

.module .module-t {}

.module .module-decsr {}
```

Once you need the module to become red, you have to add modifier selectorВ __red:

```
.module.__red {}
```
  
In order to use the styling properly you should become familiar with theВ [Multilayer CSS organization methodology](http://operatino.github.io/MCSS/en/).

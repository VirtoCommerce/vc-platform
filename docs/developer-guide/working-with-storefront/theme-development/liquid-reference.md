---
title: Liquid reference
description: DotLiquid is a templating system ported to the .net framework from Rubys Liquid Markup. You can have your users build their own templates without affecting your server security in any way.
layout: docs
date: 2015-09-17T08:50:25.393Z
priority: 1
---
DotLiquid is a templating system ported to the .net framework from Rubys <a href="http://www.liquidmarkup.org/" rel="nofollow">Liquid Markup</a>. You can have your users build their own templates without affecting your server security in any way.

Liquid utilizes tags, objects, and filters to load content. Each theme consists of .liquid files that contain tags, objects and filters.

## Tags

Tags are used to control the logic inside templates

{% raw %}
```
<ul>
  {% for product in products %}
    <li>{{ product.title }}</li>
  {% endfor %}
</ul>
```
{% endraw %}

[Read more](docs/vc2devguide/working-with-storefront/theme-development/liquid-reference/tags)

## Objects

Object contain properties that can be displayed inside templates

{% raw %}
```
{{ product.title }}

Output:
My Product Title
```
{% endraw %}

[Read more](docs/vc2devguide/working-with-storefront/theme-development/liquid-reference/objects)

## Filters

Filters are used to transform output of strings and objects

{% raw %}
```
{{ product.title | upcase }}

Output:
MY PRODUCT TITLE
```
{% endraw %}

[Read more](docs/vc2devguide/working-with-storefront/theme-development/liquid-reference/filters)

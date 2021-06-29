---
title: Objects
description: The developer guide to a Liquid objects
layout: docs
date: 2015-09-17T04:13:34.997Z
priority: 2
---
Liquid objects contain attributes to output dynamic content on the page. For example, **product** object contains the attribute **title** that can be used to output the title of a product.

**Liquid objects** are also often referred to as **Liquid variables**.

To output an object attribute on a page, wrap them in {% raw %} {{ {% endraw %} and {% raw %} }} {% endraw %}, as shown below:

{% raw %}
```
{{ product.title }}

Output: Sample Product Title
```
{% endraw %}

## Creating custom objects

Objects are defined in the code using c# classes and can be registered in context to be used in the theme. Class must be inherited from Drop class for it be accessible in the theme.

```
public class Price : Drop
{
  public string PricelistId { get; set; }
  public string ProductId { get; set; }
  public decimal? Sale { get; set; }
  public decimal List { get; set; }
  public int MinQuantity { get; set; }
}
```

Each property can then be accessed from the theme. The property names are converted automatically from Camel notation to a more common liquid notation, so for instance MinQuantity becomes min_quantity.

## Global Objects

The following objects can be used and accessed from **any file** in your theme, and are defined as **global objects**, or **global variables**.

### blogs
{% raw %}
```
<ul>
  {% for article in blogs.news.articles %}
    <li>{{ article.title | link_to: article.url }}</li>
  {% endfor %}
</ul>
```
{% endraw %}

The liquid object **blogs** refers to the blogs in your shop.

### cart

Refers to the **cart** object in your shop.

### checkout

Refers to **checkout** object. Available only on checkout pages.

### collections
{% raw %}
```
{% for product in collections.featured.products %}
  {{ product.title }}
{% endfor %}
```
{% endraw %}

Contains a list of all categories in the shop's catalog.

### current_page
{% raw %}
```
{% if current_page != 1 %} Page {{ current_page }}{% endif %}
```
{% endraw %}

Refers to the page number when browsing paginated content.

### current_tags

Refers to tags of currently rendered object. For instance product or category.

### customer

Refers to a **customer** object of either logged in user or anonymous profile.

### language

Refers to a **language** selected for a current shop.

### login_providers

Provides a list of **login providers** objects configured for the shop.

### linklists
{% raw %}
```
<ul>
  {% for link in linklists.topmenu.links %}
    <li>{{ link.title | link_to: link.url }}</li>
  {% endfor %}
</ul>
```
{% endraw %}

Refers to all the **links/menus** available for a shop.

### forms
{% raw %}
```
{% form 'customer_address', customer.new_address %}
  <input type="text" name="address[first_name]" value="" size="40" />
{% endform %}
```
{% endraw %}

Refers to **forms** object associated with a shop.

### order

Refers to a **order** associated with a current view.

### pages
{% raw %}
```
<h1>{{ pages.about.title }}</h1>
<p>{{ pages.about.author }} says...</p>
<div>{{ pages.about.content }}</div>
```
{% endraw %}

All the **pages** configured for the shop. Only contains pages for currently selected language.

### page_description
{% raw %}
```
{% if page_description %}
  <meta name="description" content="{{ page_description }}" /> 
{% endif %}
```
{% endraw %}

### page_title
{% raw %}
```
{{ page_title }}
```
{% endraw %}

### price_lists

Refers to **price lists** object associated with a current shop user.

### shop

Refers to a current **shop** object.

### template
{% raw %}
```
{% if template contains 'product' %}
  WE ARE ON A PRODUCT PAGE.
{% endif %}
```
{% endraw %}

### settings
{% raw %}
```
{% if settings.use_logo %}
  {{ 'logo.png' | asset_url | img_tag: shop.name }}
{% else %}
  <span class="no-logo">{{ shop.name }}</span>
{% endif %}
```
{% endraw %}

### theme

Contains a value for currently selected **theme**.

### themes

A list of all **themes** available for the shop.

---
title: Sitemaps
description: Sitemaps management in Virto Commerce
layout: docs
date: 2015-12-23T14:11:37.930Z
priority: 12
---
## Introduction

Sitemaps are an easy way for webmasters to inform search engines about the pages on their sites that are available for crawling. In its simplest form, a Sitemap is an XML file that lists URLs for a site along with additional metadata about each URL (when it was last updated, how often it usually changes and how important it is, relative to other URLs in the site) so that search engines could crawl the site more intelligently.

Web crawlers usually discover pages from links within the site and from other sites. Sitemaps supplement this data to allow crawlers, that support Sitemaps, to pick up all URLs in the Sitemap and learn about those URLs using the associated metadata. Using the Sitemap protocol does not guarantee that web pages are included in search engines, but provides hints for web crawlers to do a better job while crawling your site.

Virto Commerce provides multiple sitemap files, each sitemap file must include no more than 10,000 URLs (by default, maximum value - 50000 URLs) and must be no larger than 50MB (52,428,800 bytes). Each sitemap file will be placed in a sitemap index file "sitemap.xml". In case of sitemap file has more than maximum records number, it would be separated to several sitemap files, i.e.: "products.xml" sitemap file with 15000 records would be transformed to "products--1.xml" (10000 records) and "products--2.xml" (5000 records). Each of these partial sitemap files would be included in sitemap index file too.

## Installation

Steps to install Virto Commerce Sitemaps module:

1. In Virto Commerce manager navigate to "Configuration" > "Modules" > "Available" and select "Sitemaps module".
2. Click "Install" button, confirm the installation.
3. Click "Restart" button.

## Settings

Virto Commerce Sitemaps module has several settings, each of them influences on sitemap XML generation process. The settings are grouped into sections.

### General settings

* **Records limit** (default value: **10000**) - sets the maximum number of URLs records per sitemap file
* **Filename separator** (default value: **--**) - sets the sitemap location separator in case of sitemap records number exceeds the **Records limit** parameter value (i.e.: "products.xml" -> "products--1.xml" and "products--2.xml")
* **Search bunch size** (default value: **1000**) - this parameter is used in long-term search processes (i.e. catalog search) to divide search requests and sets the search request bunch size parameter
* **Export/Import description** (default value: **Export/Import sitemaps with all sitemap items**) - sets the description for platform export/import process

### Category links

* **Category page priority** (default value: **0.7**) - sets the value of the sitemap **&lt;priority&gt;** parameter of catalog categories pages
* **Category page update frequency** (default value: **weekly**) - sets the value of the sitemap **&lt;changefreq&gt;** parameter of catalog categories pages

### Product links

* **Product page priority** (default value: **1.0**) - sets the value of the sitemap **&lt;priority&gt;** parameter of catalog products pages
* **Product page update frequency** (default value: **daily**) - sets the value of the sitemap **&lt;changefreq&gt;** parameter of catalog products pages

## Configuring sitemaps

In order to configure store sitemaps navigate "Browse" > "Stores" > select an appropriate store and click "Sitemaps" widget. You should see "Sitemaps" blade containing a list of sitemaps to be included in sitemap index file.

<img src="assets/images/docs/sitemaps-1.png" />

In order to add a new sitemap, click "Add sitemap" toolbar button. The blade "New sitemap" will appear. Each sitemap contains 2 required parameters: **Sitemap location** and **Sitemap items location**, and a list of items to be included in the sitemap file.

* **Sitemap location** parameter represents the location of the sitemap file and the requirements to this parameter are the same as to a relative URL. The sitemap location value must end with '.xml' extension. Using of 'sitemap.xml' location is prohibited since this is the reserved filename for sitemap index file. Some good examples for parameter value: **products.xml**, **sitemap/vendors.xml**
* **Sitemap items location** parameter represents the sitemap items location. Since this parameter is a second part of sitemap location - the requirements to its value are the same as to relative URLs. The value of this parameter can be constructed with patterns **{language}** (will be replaced with language code of corresponding SEO info or with default store language if sitemap item had no SEO info; i.e.: **en-US**, **en-GB**, etc.) and **{slug}** (will be replaced with corresponding SEO semantic URL or sitemap item ID if sitemap item had no SEO infos).

Each sitemap contains a list of sitemap items of different types:

* Catalog sitemap items are like products and categories. For each category, there will be performed a catalog search for subcategories and products; so, category sitemap items can be called a 'formal sitemap item' - the number of real sitemap items for category sitemap item can be much more than 1. For each category and product there will be added a different URL record for SEO sematic URL in corresponding language
* Vendor sitemap items. For each vendor, there will be added a different URL record for SEO sematic URL in corresponding language
* Custom sitemap items. If you wish to include a custom URL in a sitemap, set its absolute URL here

<img src="assets/images/docs/sitemaps-2.png" />

<img src="assets/images/docs/sitemaps-3.png" />

## Use cases

* Get a zip-package of sitemaps by clicking "Download sitemaps" button on "Sitemaps" blade and place the contents of it to the store theme asset folder manually.
* Configure the storefront routes to generate a sitemap files on-the-fly (recommended for small stores, where the number of catalog items/vendors is less than 500).
* Schedule and configure a recurring job to generate the sitemap files (recommended for big stores since catalog/vendor search is a long-term process and sitemaps generation may require tens of minutes).

## References

* <a href="http://www.sitemaps.org" rel="nofollow" target="_blank">Official sitemaps guide</a>

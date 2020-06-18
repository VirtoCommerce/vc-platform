---
title: Site map
description: Site map
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 7
---
## Introduction

Sitemap is a list of pages of a web site accessible to crawlers or users. VirtoCommerce uses MVC sitemap provider <a href="https://mvcsitemap.codeplex.com/" rel="nofollow">https://mvcsitemap.codeplex.com/</a> This allows to easily create the sitemap either statically by defining nodes in xml or dynamically using DynamicNodeProviderBase. This sitemap is specifically designed for MVC so it is most flexible as it can use controllers, actions, route values and other ways to resolve nodes.

## Static configuration

The sitemap file has a prefix .sitemap. The configuration of mvc sitemap can be found in web.config file under configuration/appSettings. Use MvcSiteMapProvider_SiteMapFileName to defined the location of sitemap xml file. As can be seen below VirtoCOmmerce use Store.sitemap and custom visibility provider.

```
<add key="MvcSiteMapProvider_SiteMapFileName" value="~/Store.sitemap" />
<add key="MvcSiteMapProvider_EnableLocalization" value="true" />
<add key="MvcSiteMapProvider_DefaultSiteMapNodeVisibiltyProvider" value="VirtoCommerce.Web.Client.Extensions.NamedSiteMapNodeVisibilityProvider, VirtoCommerce.WebClient" />
<add key="MvcSiteMapProvider_IncludeAssembliesForScan" value="VirtoCommerce.Web" />
<add key="MvcSiteMapProvider_UseExternalDIContainer" value="false" />
<add key="MvcSiteMapProvider_ScanAssembliesForSiteMapNodes" value="false" />
```

The sitemap file is used for defining nodes that will be mapped to current page. Below is the sample from Store.sitemap.

```
<?xml version="1.0" encoding="utf-8" ?>
<mvcSiteMap xmlns="http://mvcsitemap.categoryplex.com/schemas/MvcSiteMap-File-4.0">
  <mvcSiteMapNode title="Home" controller="Home" action="Index" changeFrequency="Always" updatePriority="Normal">
    <mvcSiteMapNode key="home" title="Home" controller="Home" action="Index" changeFrequency="Always" updatePriority="Normal" visibility="MainMenu,!*">
      <!-- Categories are created dynamically from database-->
      <mvcSiteMapNode title="Categories" controller="Catalog" dynamicNodeProvider="VirtoCommerce.Web.Virto.Helpers.MVC.CatalogNodeProvider, VirtoCommerce.Web" visibility="MainMenu,!*"></mvcSiteMapNode>
    </mvcSiteMapNode>

    <!--Used for account page side menu-->
    <mvcSiteMapNode key="account" title="Account Dashboard" controller="Account" action="Index" changeFrequency="Always" visibility="account,!*">
      <mvcSiteMapNode title="Account Information" action="Edit"/>
      <mvcSiteMapNode title="Address Book" action="AddressBook"/>
      <mvcSiteMapNode title="My Orders" action="Orders"/>
      <mvcSiteMapNode title="My Wishlist" action="WishList"/>
      <mvcSiteMapNode title="My Returns" action="RmaReturns"/>
    </mvcSiteMapNode>

    <!-- Special node for building breadcrums-->
    <mvcSiteMapNode title="Breadcrumb" controller="Catalog" dynamicNodeProvider="VirtoCommerce.Web.Virto.Helpers.MVC.BreadcrumbNodeProvider, VirtoCommerce.Web"></mvcSiteMapNode>В В  
  </mvcSiteMapNode>
</mvcSiteMap>
```

The root element is mvcSitemap and below are nodes defined. Everything is under Home node. The visibility is used to mark when noted should be visible: in main menu or account menu. Virto uses VirtoCommerce.Web.Client.Extensions.NamedSiteMapNodeVisibilityProvider.

The category and breadrumb nodes use custom dynamic node provider to generate nodes. Account menu has statically defined nodes.

## Dynamic nodes

The categories are loaded dynamically from database. Below is the code for CatalogNodeProvider. This creates hierarchical nodes for all categories found in database.

```
using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Web.Client.Helpers;
namespace VirtoCommerce.Web.Virto.Helpers.MVC
{
  public class CatalogNodeProvider : DynamicNodeProviderBase
  {
    public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
    {
      var catalog = CatalogHelper.CatalogClient.GetCatalog(StoreHelper.CustomerSession.CatalogId);
      var nodes = new List<DynamicNode>();
      foreach (var category in catalog.CategoryBases.OfType<Category>().Where(x => x.IsActive).OrderByDescending(x => x.Priority))
      {
        var pNode = new DynamicNode
        {
          Action = "Display",
          Title = category.Name,
          Key = category.CategoryId,
          ParentKey = category.ParentCategoryId,
          RouteValues = new Dictionary<string, object> {{"category", category.Code}},
        };
        nodes.Add(pNode);
      }
      return nodes;
    }
  }
}
```

All views that are used to render nodes can be found under ~/Views/Shares/DisplayTemplates.

When creating dynamic nodes you can use Attributes dictionary to specifiy custom template and render nodes using that template. Example add pNode.Attributes.Add("Template", "MegaMenu"); and then in view call

```
string template = node.Attributes.ContainsKey("Template") ? node.Attributes["Template"].ToString() : "SiteMapNodeModelList";
@Html.DisplayFor(m => node.Children, template, new { level = ViewBag.level + 1, last = false })
```

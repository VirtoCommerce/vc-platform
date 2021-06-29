---
title: SEO
description: SEO
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 6
---
## Goals

SEO is an important factor in e-commerce site, marketing managers should be able to configure site URLs precisely with a way end user and search engines can understand. It also should support specifying the exact url for different languages.

* create simple URLs that shoppers and search engines can understand
* optimize page metadata to improve search engine page rank
* generate site maps to submit to search engine providers
* each page/product/category should be identifiable by a unique url (even language or store specific one)
* should support filtering SEO (filters should always appear in the same order)
* there should only be one URL for a page, every other URL should be automatically redirected

## How it works (Frontend)

### Routing

The actual implementation of SEO is concentrated in custom Route implementation using ASP.NET MVC engine.

There are three custom routes: ItemRoute ->CategoryRoute->StoreRoute, where arrows show inheritance chain. So the base is StoreRoute. This is done so because each route is only extending its base functionality. So the sequence of route registration is also important. First goes most specific one ItemRoute if it fails CategoryRoute is tested and finally StoreRoute. If all of them fail then its probably standard controller route. Each route segment has its own custom Constraint implementation.

The general route form is as described above in format {lang}/{store}/{category}/{item} where:

* {lang} is current language. This route segment can be omitted but it is always created and user will be redirected to normalized URL containing language in it. It uses LanguageRouteConstraint which checks if language is correct by regex and by trying to create cultureInfo from it.
* {store} is current store. Store is always created and added as route value, but it can be omitted just as language and user will be redirected to normalized URL. The exception case is when store has configured URL or SecureUrl property. In such case store is removed from route url and it becomes shorter ( {lang}/{category}/{item} ). This segment uses StoreRouteConstraint which checks if such store exists in database. It also checks if store supports requested language.
* {category} is a path of codes or keywords (if exists such in SeoUrlKeyword table) of categories. This segment uses CategoryRouteConstraint which checks if such categories exists in database.
* {item} is code or keyword for item. This segment uses ItemRouteConstraint which checks if such item exists in database. It also checks if given category path belongs to requested item.

The main job is done inside Route implementation as mentioned before. There are two main parts: Deconstructing (GetRouteData) and Constructing(GetVirtualPath) URL. 

* **GetRouteData** is responsible for filling RouteValueDIctionary with correct values that can be used by actions. This means values have to be decoded, or in other words URL deconstructed. It is also responsible for determining if this route is suitable in given context.
* **GetVirtualPath** method task is opposite. It must encode routeValues in order to construct desired url.

The implementation for Category and Item route are very simple. For GetRouteData they call base.GetRouteData, then check if appropriate key exists in route values (ex. category in CategoryRoute and item in ItemRoute). If not route does not match else get keyword value, set to route values and return routeData.

The deconstruction is even more simple for Item route. As shown here:

```
public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
{
  EncodeVirtualPath(requestContext, values, SeoUrlKeywordTypes.Item);
  return base.GetVirtualPath(requestContext, values);
}

protected virtual void EncodeVirtualPath(RequestContext requestContext, RouteValueDictionary values, SeoUrlKeywordTypes type)
{
  string routeValueKey = type.ToString().ToLower();
  var language = values.ContainsKey(Constants.Language) ? values[Constants.Language] as string : null;

  if (values.ContainsKey(routeValueKey) && values[routeValueKey] != null)
  {
    values[routeValueKey] = SettingsHelper.SeoEncode(values[routeValueKey].ToString(), type, language);
  }
}

protected override string ModifyCategoryPath(RouteValueDictionary values)
{
  var itemEncoded = values[Constants.Item] as string;

  if (string.IsNullOrEmpty(itemEncoded))
    return null;

  var itemCode = SettingsHelper.SeoDecode(itemEncoded, SeoUrlKeywordTypes.Item, values.ContainsKey(Constants.Language) ? values[Constants.Language] as string : null);
  var item = CartHelper.CatalogClient.GetItem(itemCode, bycode: true);

  if (item == null)
  {
    return null;
  }

  var outline = item.GetItemCategoryRouteValue();
 
  return outline;
}
```

Category Route additionly tarnsforms requested path so that it contains full path. This means that user can skip some parts of category path. Ex if full path is video/tvs/led, user can request using video/led or tvs/led and still be redirected to full path.

```
public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
{
  if (values.ContainsKey(Constants.Category) && values[Constants.Category] != null)
  {
    var oldOutline = values[Constants.Category].ToString();
    var newOutline = ModifyCategoryPath(values);

    if (!string.IsNullOrEmpty(newOutline) && !newOutline.Equals(oldOutline, StringComparison.InvariantCultureIgnoreCase))
    {
      values[Constants.Category] = newOutline;
    }
  }

  EncodeVirtualPath(requestContext, values, SeoUrlKeywordTypes.Category);
  return base.GetVirtualPath(requestContext, values);
}

/// <summary>
/// Convert path to full category path
/// </summary>
/// <param name="values">The route values.</param>
/// <returns></returns>
protected virtual string ModifyCategoryPath(RouteValueDictionary values)
{
  var currentPath = values[Constants.Category].ToString();

  if (string.IsNullOrEmpty(currentPath))
    return null;

  var childCategryEncoded = currentPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();

  if (string.IsNullOrEmpty(childCategryEncoded))
    return null;

  var childCategryCode = SettingsHelper.SeoDecode(childCategryEncoded, SeoUrlKeywordTypes.Category, values.ContainsKey(Constants.Language) ? values[Constants.Language] as string : null);
  var outline = new BrowsingOutline(CartHelper.CatalogOutlineBuilder.BuildCategoryOutline(StoreHelper.CustomerSession.CatalogId, CartHelper.CatalogClient.GetCategory(childCategryCode)));

  return outline.ToString();
}
```

The most complicated is **StoreRoute**. It does all the same as Category and Item routes for its segment {store}, but it must also make sure that store and language are always resolved and also that one or both segments can by omitted by user.

So there are many combinations how user can request and still all should work. The route comibations can be:

* "" (empty) - this mean use current store and current language taken from session.
* {lang}[/{.../...] - can contain language with optional additional segments that can be category and item.
* {store}[/../.} can contain only store with optional additional segments after it.
* {lang}{store}[/.../...[/...]] - can contain both language and store.

As StoreRoute is base class it also maps category path and item to route values if base MVC Route fails to do that.

The construction of virtual path for store route has one exception: If store is defined in URL it has to be omitted in path. That is handled be temporary removing {store} from route Url and getting virtual path, then restoring it back as shown below:

```
//Need to be in lock to make sure other thread does not change originalUrl in this block
lock (thisLock)
{
  var originalUrl = Url;

  //If for request store URL is used do not show it in path
  if (store != null && (!string.IsNullOrEmpty(store.Url) || !string.IsNullOrEmpty(store.SecureUrl)))
  {
    Url = Url.Replace(string.Format("/{{{0}}}", Constants.Store), "");
    values.Remove(Constants.Store);
  }
  else
  {
    ModifyVirtualPath(requestContext, values, SeoUrlKeywordTypes.Store);
  }

  var retVal = base.GetVirtualPath(requestContext, values);

  //Restore original URL
  if (!string.IsNullOrEmpty(originalUrl) && !originalUrl.Equals(Url))
  {
    Url = originalUrl;
  }

  return retVal;
}
```

### Url Canonicalization

Each action request is filtered by using mvc ActionFilterAttribute implementation called Canonicalized. This attribute is added to ControllerBase so that each request is processed and only GET non child requests are standardized. If canonical URL does not mach requested one user is redirected permanent (301) to canonical URL

Canonicalization has following rules:

* Route virtual path must match requested path. This means that if we skip something like store, but route knows default value it creates virtual path containing store
* Query string is always ordered:
  * First goes all filters ordered in same order as FIlteredBrowsing xml elements for current store
  * Second goes remaining query string values ordered alphabetically by key.
* Language parameter if used is converted to five symbols like "en-us", but user can still access using short form "en"
* Segments {store} {category} and {item} are always encoded with keyword if available. This means user can still access by using item or category code, but will be redirected.
* The URL is always in lower case
* The URL path will not end with trailing slash '/"

### Handling Metadata

Metadata retrieved from **SeoUrlKeyword** table if available for each successfully executed non child action. The implementation is in ControllerBase class as shown below:

```
protected override void OnActionExecuted(ActionExecutedContext filterContext)
{
  if (!filterContext.Canceled && filterContext.Result != null && !ControllerContext.IsChildAction)
  {
    FillViewBagWithMetadata(filterContext, Constants.Store);
    FillViewBagWithMetadata(filterContext, Constants.Category);
    FillViewBagWithMetadata(filterContext, Constants.Item);
  }
}

protected virtual void FillViewBagWithMetadata(ActionExecutedContext filterContext, string routeKey)
{
  if (filterContext.RouteData.Values.ContainsKey(routeKey))
  {
    var routeValue = filterContext.RouteData.Values[routeKey] as string;
    if (!String.IsNullOrEmpty(routeValue))
    {
      SeoUrlKeywordTypes type;
      if (Enum.TryParse(routeKey, true, out type))
      {
        var keyword = SettingsHelper.SeoKeyword(routeValue, type, byValue: false);

        if (keyword != null)
        {
          ViewBag.MetaDescription = keyword.MetaDescription;
          ViewBag.Title = keyword.Title;
          ViewBag.MetaKeywords = keyword.MetaKeywords;
          ViewBag.ImageAltDescription = keyword.ImageAltDescription;
        }
      }
    }
  }
}
```

As can be seen this fills ViewBag with meta data values, that are used in pages:

```
<meta name="keywords" content="@ViewBag.MetaKeywords"/>
<meta name="description" content="@ViewBag.MetaDescription"/>
<meta name="title" content="@ViewBag.Title"/>
<title>@ViewBag.Title</title>
```

### Legacy route support

Some legacy routes are supported trough custom RedirectRoute implementation. Those are c/{category} and p/{item}. They can be used when defining url in dynamic content, as there is no need to add store, language or category for item. They are taken from context.

```
//Legacy redirects
routes.Redirect(r => r.MapRoute("old_Category", string.Format("c/{{{0}}}", Constants.Category))).To(categoryRoute);
routes.Redirect(r => r.MapRoute("old_Item", string.Format("p/{{{0}}}", Constants.Item))).To(itemRoute,
  x =>
  {
    //Resolve item category dynamically
    if (x.RouteData.Values.ContainsKey(Constants.Item))
    {
      var item = CatalogHelper.CatalogClient.GetItem(x.RouteData.Values[Constants.Item].ToString(), bycode: true);
      if (item != null)
      {
        return new RouteValueDictionary { { Constants.Category, item.GetItemCategoryRouteValue() } };
      }
    };
    return null;
  });
```

---
title: Closing store
description: Closing store
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 1
---
The store can have three states:

* Open - is accessible for everyone.
* RestrictedAccess - only user that have special permission can open the store. Anonymous user are always redirect to login page in this store
* Closed - can be opened only by users that have special permission. Normally that is only admin.

The status of store can be changed in admin tool by going to Settings/Stores and selecting store. Then changing store state and saving.

<img src="../../../../assets/images/store.png" />

## Permissions

The role that has a permission of accessing Restricted access stores is named "Private Shopper". The permission can be configured in administration tool by going to Users and selecting user that we want to assign the role. Clicking on Roles tab for user and assigning Private Shopper" role

<img src="../../../../assets/images/privateShopper.png" />

## How it works

The central place that check if user is authorized to specif store is in StoreHelper.IsAuthorized method. The method checks the following things:

* First it gets the Account be userName. If account is not found user is not authorized
* If user is administrator user is authorized
* Otherwise method checks if current store and store in account profile is same or if there is linked store to current store that matches account store. If check fails user is not authorized
* If store is closed and user does not have permission to browse closed stores he is not authorized
* If store is restricted and user does not have permission to browse restricted stores he is not authorized

```
/// <summary>
/// Determines whether [is user authorized] [the specified user name].
/// </summary>
/// <param name="userName">Name of the user.</param>
/// <param name="storeId">The store id. Empty means current store</param>
/// <param name="errorMessage">The error message.</param>
/// <returns>
///В В  <c>true</c> if [is user authorized] [the specified user name]; otherwise, <c>false</c>.
/// </returns>
public static bool IsUserAuthorized(string userName, string storeId, out string errorMessage)
{
  Account account;
  var isAuthorized = UserClient.IsAuthorized(userName, out account);
  errorMessage = null;
  if (isAuthorized && account != null)
  {
    if (account.RegisterType != (int)RegisterType.Administrator)
    {
      isAuthorized = StoreClient.IsLinkedAccountAuthorized(account.StoreId, storeId);
      //Check if user has access to current store
      if (isAuthorized)
      {
        var store = StoreClient.GetStoreById(CustomerSession.StoreId);
        if (store.StoreState == StoreState.Closed.GetHashCode())
        {
          isAuthorized = SecurityService.CheckMemberPermission(account.MemberId, new Permission{ PermissionId = PredefinedPermissions.ShopperClosedAccess});
          if (!isAuthorized)
          {
            var setting = store.Settings.SingleOrDefault(n => n.Name.Equals("StoreClosedMessage"));
            errorMessage = setting != null ? setting.ShortTextValue : "The store is temporarily closed for maintenance. Please try again later.";
          }
        }
        else if (store.StoreState == StoreState.RestrictedAccess.GetHashCode())
        {
          isAuthorized = SecurityService.CheckMemberPermission(account.MemberId, new Permission { PermissionId = PredefinedPermissions.ShopperRestrictedAccessВ });
          if (!isAuthorized)
          {
            var setting = store.Settings.SingleOrDefault(n => n.Name.Equals("StoreRestrictedMessage"));
            errorMessage = setting != null ? setting.ShortTextValue : "You do not have permissions to view this store";
          }
        }
      }
      else
      {
        errorMessage = "You do not have permissions to view this store";
      }
    }
  }
  else
  {
    errorMessage = "Account not authorized";
  }
  return isAuthorized;
}
```

The store selection visibiility is controlled in StoreController in web application:

* Sign in users can see only those store where StoreHelper.IsAuthorized is true
* Anonymous users can see open and restricted stores

```
private bool IsStoreVisible(Store store)
{
  if (Request.IsAuthenticated)
  {
    string errorMessage;
    return StoreHelper.IsUserAuthorized(HttpContext.User.Identity.Name, store.StoreId, out errorMessage);
  }
В В В В В В В В В В В В 
  return store.StoreState == StoreState.Open.GetHashCode() || store.StoreState == StoreState.RestrictedAccess.GetHashCode();
}
```

The StoreHttpModule is responsible for taking action when user is not allowed to see restricted stores:

* If user is signed in but StoreHelper.isAuthorized = false user is automaticallt sign off and if store is closed exception 403 thrown
* If user is anonymous and store is resctricted he is redirected to login page, if store is closed exception 403 thrown

```
void context_AuthenticateRequest(object sender, EventArgs e)
{
  if (IsResourceFile())
    return;
  // Check if the user actually exists in our database
  var application = (HttpApplication)sender;
  var session = CustomerSession;
  var context = application.Context;
  var store = GetStore(context);
  if (context.Request.IsAuthenticated)
  {
    string errorMessage;
    if (!StoreHelper.IsUserAuthorized(context.User.Identity.Name, out errorMessage))
    {
      //WebSecurity.Logout();
      FormsAuthentication.SignOut(); // it is ok to use this here, since that is what WebSecurity calls anyway
      // now check if store is accessible
      if (store.StoreState == StoreState.Closed.GetHashCode())
      {
        throw new HttpException(403, "Store Closed");
      }
      context.Response.Redirect(context.Request.RawUrl);
    }
    var account = StoreHelper.UserClient.GetAccountByUserName(context.User.Identity.Name);
    if (account != null)
    {
      session.CustomerId = account.MemberId;
    }
    var contact = StoreHelper.UserClient.GetCurrentCustomer();
    session.CustomerName = contact != null ? contact.FullName : application.User.Identity.Name;
  }
  else
  {
    //Redirect to login page users that are not authenticated but try to navigate to restricted store
    if (store.StoreState == StoreState.RestrictedAccess.GetHashCode() && !HttpContext.Current.Request.RawUrl.Equals(FormsAuthentication.LoginUrl))
    {
      HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
    }
    if (store.StoreState == StoreState.Closed.GetHashCode())
    {
      throw new HttpException(403, "Store Closed");
    }
  }
}
```

---
title: Add new module to Virto Commerce admin
description: Add new module to Virto Commerce admin
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 1
---
## Summary

This tutorial will show you the steps in creating new module and adding it to **VirtoCommerce.Presentation**В application.

## Overview

In this tutorial you will learn how to create custom (pricelist) module and add it to theВ **VirtoCommerce.Presentation**В application. This will be shown on the Reviews module that will be created and added to the application. The module will contain main (Home) view where the list of Pricelists will be viewed. Also the item view/edit form for the selected pricelist will be created and added to the application navigation system to view and edit selected item. Based on the edit view the wizard will be created.
In the end of the tutorial you will be able to create and add new module to theВ **VirtoCommerce.Presentation**В application.

## Prism modularity concept

**VirtoCommerce.Presentation**В application is built with Prism 4.0 library patterns, using the principles of modular expansion, provided by the platform. For detailed specifications of the modular principles see MSDN:В <a href="http://msdn.microsoft.com/en-us/library/gg405479(v=PandP.40).aspx" rel="nofollow">Chapter 4: Modular Application Development</a>.

Unity is used as IoC container to buildВ object tree and resolve dependencies.

**VirtoCommerce.Presentation**В supports the module definition over configuration files of the application as well as automated modules loading though directory modules catalog loader.

## Step 1. Create module project

1. Add new project to the solution.
2. SelectВ "WPF Custom Control Library" project. Name it to PricelistModule. Press OK.
3. Rename Class1.cs file to the PricelistModule.cs. Let the class to be renamed as well
4. Add references to libraries to the project
  * Presentation.Core.dll
  * CommerceFoundation.dll
5. Add Nuget Prism and Unity packages to the project references.

Now let's fill our module with the required functionality. It is to view the list of pricelists and view/edit selected review information.

## Step 2. Create detail ViewModel for the module

Now let's create view model for the PriceList (Detail) view.

1. Add ViewModel folder to the project.
2. Add IPriceListViewModel.cs files to the ViewModel folder.В 

**IPriceList interface**
```
publicВ interfaceВ IPriceListViewModelВ :В IViewModelDetailBase
{
}
```

3. Add PriceListViewModel to the ViewModelВ folder and implement IPriceListViewModel.cs interface.В 

**PriceListViewModel implementation**
```
publicВ classВ PriceListViewModelВ :В IPriceListViewModel
{
}
```

4. Inherit PriceListViewModel from ViewModelDetailAndWizardBase<T> because this viewmodel will be used in wizard dialog. where T is Pricelist to edit.

**PriceListViewModel implementation**
```
publicВ classВ PriceListViewModelВ :В ViewModelDetailAndWizardBase<Pricelist>,В IPriceListViewModel
{
}
```

Inherited properties are:

* **OriginalItem** - PriceList item before edit.
* **InnerItem** - Editing PriceList item into the viewmodel.
* **IsWizardMode** - get or set if the view is in Wizard mode, True means viewmodel object used as wizard step

5. Add required variables as Dependencies

**Dependencies**
```
privateВ readonlyВ IAuthenticationContextВ _authContext;
privateВ readonlyВ NavigationManagerВ _navManager;
privateВ readonlyВ IRepositoryFactory<IPricelistRepository> _repositoryFactory;
privateВ readonlyВ IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;
privateВ readonlyВ IViewModelsFactoryВ _vmFactory;
```

6. Add those dependencies as constructor parameters for edit mode. Public constructor will be used by container to pass dependencies.

**Public Constructor (Edit mode)**
```
public PriceListViewModel(
  IRepositoryFactory<IPricelistRepository> repositoryFactory,
  IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,В 
  IViewModelsFactory<IPriceViewModel> priceVmFactory,
  ICatalogEntityFactory entityFactory,В 
  INavigationManager navManager,
  IAuthenticationContext authContext,В 
  Pricelist item)
  : base(entityFactory, item, false)
{
    ViewTitle = new ViewTitleBase()
    {
      Title = "Price List",
      SubTitle = (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name.ToUpper(CultureInfo.InvariantCulture) : ""
    };
    _repositoryFactory = repositoryFactory;
    _appConfigRepositoryFactory = appConfigRepositoryFactory;
    _priceVmFactory = priceVmFactory;
    _navManager = navManager;
    _authContext = authContext;
    OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
    CommandsInit();
}
```

7. Add protected constructor for wizardВ 

**Protected constructor (Wizard mode)**
```
protected PriceListViewModel(
  IRepositoryFactory<IPricelistRepository> repositoryFactory,
  IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
  ICatalogEntityFactory entityFactory,
  IAuthenticationContext authContext, Pricelist item)
  : base(entityFactory, item, true)
{
  _repositoryFactory = repositoryFactory;
  _appConfigRepositoryFactory = appConfigRepositoryFactory;
  _authContext = authContext;
}
```

8. Add implementation of ViewModelBase properties

* **DisplayName** - name for the PriceListViewModel item for navigation system and another visual elements.

```
publicВ overrideВ stringВ DisplayName
{
  getВ {В returnВ OriginalItem ==В nullВ ?В this.GetHashCode().ToString() : OriginalItem.Name; }
}
```

* **ShellDetailItemMenuBrush** - color of this item in top menu.

```
publicВ overrideВ BrushВ ShellDetailItemMenuBrush
{
  get
  {
    varВ result = (SolidColorBrush)В Application.Current.TryFindResource("PriceListDetailItemMenuBrush");В 
    returnВ result ??В base.ShellDetailItemMenuBrush;
  }
}
```

9. Add implementation of ViewModelDetailAndWizardBase

* **ExceptionContextIdentity** - Identity of viewmodel object for fill info when exception occupied (usually name of viewmodel and DisplayName)

```
publicВ overrideВ stringВ ExceptionContextIdentity {В getВ {В returnВ string.Format("Price list ({0})", DisplayName); } }
```

* **GetRepository** - return repository for PriceList itemВ 

```
protectedВ overrideВ voidВ GetRepository()
{
  Repository = _repositoryFactory.GetRepositoryInstance();
}
```

* **HasPermission** - return true if has permission for edit PriceList item

```
protectedВ overrideВ boolВ HasPermission()
{
  returnВ _authContext.CheckPermission(PredefinedPermissions.PricingPrice_ListsManage);
}
```

* **IsValidForSave** - valid item before save

```
protectedВ overrideВ boolВ IsValidForSave()
{
  returnВ InnerItem.Validate();
}
```

* **CancelConfirm** - Return RefusedConfirmation for Cancel Confirm dialog

```
protectedВ overrideВ RefusedConfirmationВ CancelConfirm()
{
  returnВ newВ RefusedConfirmation
  {
    Content =В "Save changes to price list '"В + DisplayName +В "'?",
    Title =В "Action confirmation"
  };
}
```

* **LoadInnerItem** - Load PriceList item (InnerItem) from repository with all dependencies

```
protectedВ overrideВ voidВ LoadInnerItem()
{
  try
  {
    varВ item = (RepositoryВ asВ IPricelistRepository).Pricelists.Where(x => x.PricelistId == OriginalItem.PricelistId)
      .Expand("Prices/CatalogItem")
      .SingleOrDefault();
    OnUIThread(() => { InnerItem = item; });
В В }
  catchВ (ExceptionВ ex)
  {
    ShowErrorDialog(ex,В string.Format("An error occurred when trying to loadВ {0}",
    ExceptionContextIdentity));
  }
}
```

* **InitializePropertiesForViewing** - Initialize some viewmodel properties after load InnerItem

```
protectedВ overrideВ voidВ InitializePropertiesForViewing()
{
  ifВ (!IsWizardMode)
  {
    InitializeAvailableCurrencies();
  }
}
```

* **AfterSaveChangesUI** - Execute after DoSaveChanges() in UI thread. OriginalItem should be complete here

```
protectedВ overrideВ voidВ AfterSaveChangesUI()
{
  OriginalItem.InjectFrom<CloneInjection>(InnerItem);
}
```

* **SetSubscriptionUI** - Set subscription to tracking changes of ViewModel's properties or InnerItem's collections after load InnerItem

```
protectedВ overrideВ voidВ SetSubscriptionUI()
{
  ifВ (InnerItem.Prices !=В null)
  {
    InnerItem.Prices.CollectionChanged += ViewModel_PropertyChanged;
  }
}
```

* **CloseSubscriptionUI** - Unsubscribe from tracking changes of ViewModel's properties or InnerItem's collections

```
protectedВ overrideВ voidВ CloseSubscriptionUI()
{
  ifВ (InnerItem.Prices !=В null)
  {
    InnerItem.Prices.CollectionChanged -= ViewModel_PropertyChanged;
  }
}
```

* **BeforeDelete**В - Execute before remove item (PriceList) from repository

```
protectedВ overrideВ boolВ BeforeDelete()
{
  CommonExtensions.DeleteCollectionItems<Price>(InnerItem.Prices, RepositoryВ asВ IPricelistRepository);
  returnВ true;
}
```

## Step 3. Create wizard for the module

Now create viewmodel for wizard step.

1. Add Wizard folder into ViewModel folder of the module.
2. Add wizard interface IPriceListOverviewStepViewModel.cs files to the Wizard folder.

```
publicВ interfaceВ IPriceListOverviewStepViewModelВ :В IWizardStep
{
}
```

3. Add wizard step PriceListOverviewStepViewModel.cs file to the Wizard folder. Inherit it from PriceListViewModel that was created in the previous step as it repeats all the funcionality of the pricelist edit. It should also implement IPriceListOverviewStepViewModel interface

```
public class PriceListOverviewStepViewModel : PriceListViewModel, IPriceListOverviewStepViewModel
{
  public PriceListOverviewStepViewModel(
    IRepositoryFactory<IPricelistRepository> repositoryFactory,
    IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, 
    ICatalogEntityFactory entityFactory, IAuthenticationContext authContext, Pricelist item)
    : base(repositoryFactory, appConfigRepositoryFactory,В  entityFactory, authContext, item)
  {
  }
  //implement IWizardStep interface
}
```

4. Create CreatePriceListViewModel class derived fromВ WizardContainerStepsViewModelВ that is also implements ICreatePriceListViewModel empty interfaceВ and register all wizard steps in its constructor.

```
public class CreatePriceListViewModel : WizardContainerStepsViewModel, ICreatePriceListViewModel
{
  public CreatePriceListViewModel(IViewModelsFactory<IPriceListOverviewStepViewModel> vmFactory, Pricelist item)
  {
    RegisterStep(vmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item)));
  }
}
```

5. Implement IWizardStep interface.

* **IsValid** - true if step is valid

```
publicВ overrideВ boolВ IsValid
{
  get
  {
    boolВ doNotifyChanges =В false;
    InnerItem.Validate(doNotifyChanges);
    varВ retval = InnerItem.Errors.Count == 0;
    InnerItem.Errors.Clear();
    returnВ retval;
  }
}
```

* **IsLast** - true if the step is the last step of the wizard

```
publicВ overrideВ boolВ IsLast {В getВ {В returnВ true; } }
```

* **Description** - Description of the step that is shown in the header of the wizard

```
publicВ overrideВ stringВ Description {В getВ {В returnВ "Enter price list general information."; } }
```

## Step 4. Create Home viewmodel for the module

1. Add IPriceListHomeViewModel.cs file to the ViewModel folder. The interface should be derived from the Presentation.Core.Infrastructure.IViewModel interface.

```
publicВ interfaceВ IPriceListHomeViewModelВ :В IViewModel
{
}
```

2. Add PriceListHomeViewModel.cs class file to the ViewModel folder.

The class should be derived from the ViewModelHomeEditableBase<T>В where T is type that the home view working with.

Also theВ PriceListHomeViewModelВ should implement next interfacesВ IPriceListHomeViewModel,В ISupportDelayInitialization andВ IVirtualListLoader<T>, where T is detail viewmodel interface

```
publicВ classВ PriceListHomeViewModelВ :В ViewModelHomeEditableBase<Pricelist>,В IPriceListHomeViewModel,В IVirtualListLoader<IPriceListViewModel>,В ISupportDelayInitialization
{
}
```

3. Add dependencies

```
#regionВ Dependencies
privateВ readonlyВ ICatalogEntityFactoryВ _entityFactory;
privateВ readonlyВ IAuthenticationContextВ _authContext;
privateВ readonlyВ IRepositoryFactory<IPricelistRepository> _pricelistRepository;
privateВ readonlyВ IViewModelsFactoryВ _vmFactory;
privateВ readonlyВ NavigationManagerВ _navManager;
#endregion
```

4. Add constructor and pass dependencies as constructor parameters

```
publicВ PriceListHomeViewModel(
  ICatalogEntityFactoryВ entityFactory,В 
  IViewModelsFactoryВ vmFactory,В 
  IRepositoryFactory<IPricelistRepository> pricelistRepository,В 
  IAuthenticationContextВ authContext,В 
  NavigationManagerВ navManager)
{
  _entityFactory = entityFactory;
  _pricelistRepository = pricelistRepository;
  _authContext = authContext;
  _vmFactory = vmFactory;
  _navManager = navManager;
  _tileManager = tileManager;
}
```

5. Override ViewModelHomeEditableBase methods

* **CanItemAddExecute** return true if allow add new item (PriceList)

```
protectedВ overrideВ boolВ CanItemAddExecute()
{
  returnВ _authContext.CheckPermission(PredefinedPermissions.PricingPrice_ListsManage);
}
```

* **CanItemDeleteExecute** return true if allow remove some items (PriceList)

```
protectedВ overrideВ boolВ CanItemDeleteExecute(IListВ x)
{
  returnВ _authContext.CheckPermission(PredefinedPermissions.PricingPrice_List_AssignmentsManage)В && x !=В nullВ && x.Count > 0;
}
```

* **RaiseItemAddInteractionRequest** create new item (PriceList), create wizard container and show wizard to add new Item

```
protectedВ overrideВ voidВ RaiseItemAddInteractionRequest()
{
  varВ item = _entityFactory.CreateEntity<Pricelist>();
  varВ vm = _vmFactory.Create<ICreatePriceListViewModel>(newВ KeyValuePair<string,В object>("item", item));В 
  varВ confirmation =В newВ ConfirmationВ { Content = vm, Title =В "Create Price List"В };
  ItemAdd(confirmation);
}
```

* **RaiseItemDeleteInteractionRequest** remove selected items (PriceList)

```
protectedВ overrideВ voidВ RaiseItemDeleteInteractionRequest(IListВ selectedItemsList)
{
  varВ selectedItems = selectedItemsList.Cast<VirtualListItem<IPriceListViewModel>>();
  ItemDelete(selectedItems.Select(x => ((IViewModelDetailBase)x.Data)).ToList());
}В 
```

7. Implement IVirtualListLoader<T> interface

```
#regionВ IVirtualListLoader<IPriceListViewModel> MembersВ 
publicВ boolВ CanSort {В getВ {В returnВ false; } }В 
В 
publicВ IList<IPriceListViewModel> LoadRange(intВ startIndex,В intВ count,В SortDescriptionCollectionВ sortDescriptions,В outВ intВ overallCount)
{
  varВ retVal =В newВ List<IPriceListViewModel>();В 
  usingВ (varВ repository = _pricelistRepository.GetRepositoryInstance())
  {
    varВ query = repository.Pricelists;В 
    ifВ (!string.IsNullOrEmpty(SearchFilterKeyword))
      query = query.Where(x => x.Name.Contains(SearchFilterKeyword)
        x.Description.Contains(SearchFilterKeyword));
    else
    {
      if (!string.IsNullOrEmpty(SearchFilterName))
        query = query.Where(x => x.Name.Contains(SearchFilterName));
      if (!string.IsNullOrEmpty(SearchFilterCurrency))
        query = query.Where(x => x.Currency.Contains(SearchFilterCurrency));
    }
    overallCount = query.Count();
    var items = query.OrderBy(x => x.Name).Skip(startIndex).Take(count).ToList();
    foreach (var item in items)
    {
      var itemViewModel = _vmFactory.Create<IPriceListViewModel>(new KeyValuePair<string, object>("item", item));
      retVal.Add(itemViewModel);
    }
  }
  return retVal;
} 
#endregion
```

8. Implement ISupportDelayInitialization interface

```
#region ISupportDelayInitialization Members 
publicВ void InitializeForOpen()
{
  if (ListItemsSource == null)
  {
    OnUIThread(() => ListItemsSource = newВ VirtualList<IPriceListViewModel>(this, 25, SynchronizationContext.Current));
  }
}
#endregion
```

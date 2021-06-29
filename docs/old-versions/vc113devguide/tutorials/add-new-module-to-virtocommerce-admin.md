---
title: Add new module to Virto Commerce admin
description: Add new module to Virto Commerce admin
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 1
---
## Summary

This tutorial will show you the steps in creating new module and adding it to **VirtoCommerce.Presentation** application.

## Overview

In this tutorial you will learn how to create custom (pricelist) module and add it to the **VirtoCommerce.Presentation** application. This will be shown on the Reviews module that will be created and added to the application. The module will contain main (Home) view where the list of Pricelists will be viewed. Also the item view/edit form for the selected pricelist will be created and added to the application navigation system to view and edit selected item. Based on the edit view the wizard will be created.
In the end of the tutorial you will be able to create and add new module to the **VirtoCommerce.Presentation** application.

## Prism modularity concept

**VirtoCommerce.Presentation** application is built with Prism 4.0 library patterns, using the principles of modular expansion, provided by the platform. For detailed specifications of the modular principles see MSDN: <a href="http://msdn.microsoft.com/en-us/library/gg405479(v=PandP.40).aspx" rel="nofollow">Chapter 4: Modular Application Development</a>.

Unity is used as IoC container to build object tree and resolve dependencies.

**VirtoCommerce.Presentation** supports the module definition over configuration files of the application as well as automated modules loading though directory modules catalog loader.

## Step 1. Create module project

1. Add new project to the solution.
2. Select "WPF Custom Control Library" project. Name it to PricelistModule. Press OK.
3. Rename Class1.cs file to the PricelistModule.cs. Let the class to be renamed as well
4. Add references to libraries to the project
  * Presentation.Core.dll
  * CommerceFoundation.dll
5. Add Nuget Prism and Unity packages to the project references.

Now let's fill our module with the required functionality. It is to view the list of pricelists and view/edit selected review information.

## Step 2. Create detail ViewModel for the module

Now let's create view model for the PriceList (Detail) view.

1. Add ViewModel folder to the project.
2. Add IPriceListViewModel.cs files to the ViewModel folder. 

**IPriceList interface**
```
public interface IPriceListViewModel : IViewModelDetailBase
{
}
```

3. Add PriceListViewModel to the ViewModel folder and implement IPriceListViewModel.cs interface. 

**PriceListViewModel implementation**
```
public class PriceListViewModel : IPriceListViewModel
{
}
```

4. Inherit PriceListViewModel from ViewModelDetailAndWizardBase<T> because this viewmodel will be used in wizard dialog. where T is Pricelist to edit.

**PriceListViewModel implementation**
```
public class PriceListViewModel : ViewModelDetailAndWizardBase<Pricelist>, IPriceListViewModel
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
private readonly IAuthenticationContext _authContext;
private readonly NavigationManager _navManager;
private readonly IRepositoryFactory<IPricelistRepository> _repositoryFactory;
private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;
private readonly IViewModelsFactory _vmFactory;
```

6. Add those dependencies as constructor parameters for edit mode. Public constructor will be used by container to pass dependencies.

**Public Constructor (Edit mode)**
```
public PriceListViewModel(
  IRepositoryFactory<IPricelistRepository> repositoryFactory,
  IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, 
  IViewModelsFactory<IPriceViewModel> priceVmFactory,
  ICatalogEntityFactory entityFactory, 
  INavigationManager navManager,
  IAuthenticationContext authContext, 
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

7. Add protected constructor for wizard 

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
public override string DisplayName
{
  get { return OriginalItem == null ? this.GetHashCode().ToString() : OriginalItem.Name; }
}
```

* **ShellDetailItemMenuBrush** - color of this item in top menu.

```
public override Brush ShellDetailItemMenuBrush
{
  get
  {
    var result = (SolidColorBrush) Application.Current.TryFindResource("PriceListDetailItemMenuBrush"); 
    return result ?? base.ShellDetailItemMenuBrush;
  }
}
```

9. Add implementation of ViewModelDetailAndWizardBase

* **ExceptionContextIdentity** - Identity of viewmodel object for fill info when exception occupied (usually name of viewmodel and DisplayName)

```
public override string ExceptionContextIdentity { get { return string.Format("Price list ({0})", DisplayName); } }
```

* **GetRepository** - return repository for PriceList item 

```
protected override void GetRepository()
{
  Repository = _repositoryFactory.GetRepositoryInstance();
}
```

* **HasPermission** - return true if has permission for edit PriceList item

```
protected override bool HasPermission()
{
  return _authContext.CheckPermission(PredefinedPermissions.PricingPrice_ListsManage);
}
```

* **IsValidForSave** - valid item before save

```
protected override bool IsValidForSave()
{
  return InnerItem.Validate();
}
```

* **CancelConfirm** - Return RefusedConfirmation for Cancel Confirm dialog

```
protected override RefusedConfirmation CancelConfirm()
{
  return new RefusedConfirmation
  {
    Content = "Save changes to price list '" + DisplayName + "'?",
    Title = "Action confirmation"
  };
}
```

* **LoadInnerItem** - Load PriceList item (InnerItem) from repository with all dependencies

```
protected override void LoadInnerItem()
{
  try
  {
    var item = (Repository as IPricelistRepository).Pricelists.Where(x => x.PricelistId == OriginalItem.PricelistId)
      .Expand("Prices/CatalogItem")
      .SingleOrDefault();
    OnUIThread(() => { InnerItem = item; });
  }
  catch (Exception ex)
  {
    ShowErrorDialog(ex, string.Format("An error occurred when trying to load {0}",
    ExceptionContextIdentity));
  }
}
```

* **InitializePropertiesForViewing** - Initialize some viewmodel properties after load InnerItem

```
protected override void InitializePropertiesForViewing()
{
  if (!IsWizardMode)
  {
    InitializeAvailableCurrencies();
  }
}
```

* **AfterSaveChangesUI** - Execute after DoSaveChanges() in UI thread. OriginalItem should be complete here

```
protected override void AfterSaveChangesUI()
{
  OriginalItem.InjectFrom<CloneInjection>(InnerItem);
}
```

* **SetSubscriptionUI** - Set subscription to tracking changes of ViewModel's properties or InnerItem's collections after load InnerItem

```
protected override void SetSubscriptionUI()
{
  if (InnerItem.Prices != null)
  {
    InnerItem.Prices.CollectionChanged += ViewModel_PropertyChanged;
  }
}
```

* **CloseSubscriptionUI** - Unsubscribe from tracking changes of ViewModel's properties or InnerItem's collections

```
protected override void CloseSubscriptionUI()
{
  if (InnerItem.Prices != null)
  {
    InnerItem.Prices.CollectionChanged -= ViewModel_PropertyChanged;
  }
}
```

* **BeforeDelete** - Execute before remove item (PriceList) from repository

```
protected override bool BeforeDelete()
{
  CommonExtensions.DeleteCollectionItems<Price>(InnerItem.Prices, Repository as IPricelistRepository);
  return true;
}
```

## Step 3. Create wizard for the module

Now create viewmodel for wizard step.

1. Add Wizard folder into ViewModel folder of the module.
2. Add wizard interface IPriceListOverviewStepViewModel.cs files to the Wizard folder.

```
public interface IPriceListOverviewStepViewModel : IWizardStep
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
    : base(repositoryFactory, appConfigRepositoryFactory,  entityFactory, authContext, item)
  {
  }
  //implement IWizardStep interface
}
```

4. Create CreatePriceListViewModel class derived from WizardContainerStepsViewModel that is also implements ICreatePriceListViewModel empty interface and register all wizard steps in its constructor.

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
public override bool IsValid
{
  get
  {
    bool doNotifyChanges = false;
    InnerItem.Validate(doNotifyChanges);
    var retval = InnerItem.Errors.Count == 0;
    InnerItem.Errors.Clear();
    return retval;
  }
}
```

* **IsLast** - true if the step is the last step of the wizard

```
public override bool IsLast { get { return true; } }
```

* **Description** - Description of the step that is shown in the header of the wizard

```
public override string Description { get { return "Enter price list general information."; } }
```

## Step 4. Create Home viewmodel for the module

1. Add IPriceListHomeViewModel.cs file to the ViewModel folder. The interface should be derived from the Presentation.Core.Infrastructure.IViewModel interface.

```
public interface IPriceListHomeViewModel : IViewModel
{
}
```

2. Add PriceListHomeViewModel.cs class file to the ViewModel folder.

The class should be derived from the ViewModelHomeEditableBase<T> where T is type that the home view working with.

Also the PriceListHomeViewModel should implement next interfaces IPriceListHomeViewModel, ISupportDelayInitialization and IVirtualListLoader<T>, where T is detail viewmodel interface

```
public class PriceListHomeViewModel : ViewModelHomeEditableBase<Pricelist>, IPriceListHomeViewModel, IVirtualListLoader<IPriceListViewModel>, ISupportDelayInitialization
{
}
```

3. Add dependencies

```
#region Dependencies
private readonly ICatalogEntityFactory _entityFactory;
private readonly IAuthenticationContext _authContext;
private readonly IRepositoryFactory<IPricelistRepository> _pricelistRepository;
private readonly IViewModelsFactory _vmFactory;
private readonly NavigationManager _navManager;
#endregion
```

4. Add constructor and pass dependencies as constructor parameters

```
public PriceListHomeViewModel(
  ICatalogEntityFactory entityFactory, 
  IViewModelsFactory vmFactory, 
  IRepositoryFactory<IPricelistRepository> pricelistRepository, 
  IAuthenticationContext authContext, 
  NavigationManager navManager)
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
protected override bool CanItemAddExecute()
{
  return _authContext.CheckPermission(PredefinedPermissions.PricingPrice_ListsManage);
}
```

* **CanItemDeleteExecute** return true if allow remove some items (PriceList)

```
protected override bool CanItemDeleteExecute(IList x)
{
  return _authContext.CheckPermission(PredefinedPermissions.PricingPrice_List_AssignmentsManage) && x != null && x.Count > 0;
}
```

* **RaiseItemAddInteractionRequest** create new item (PriceList), create wizard container and show wizard to add new Item

```
protected override void RaiseItemAddInteractionRequest()
{
  var item = _entityFactory.CreateEntity<Pricelist>();
  var vm = _vmFactory.Create<ICreatePriceListViewModel>(new KeyValuePair<string, object>("item", item)); 
  var confirmation = new Confirmation { Content = vm, Title = "Create Price List" };
  ItemAdd(confirmation);
}
```

* **RaiseItemDeleteInteractionRequest** remove selected items (PriceList)

```
protected override void RaiseItemDeleteInteractionRequest(IList selectedItemsList)
{
  var selectedItems = selectedItemsList.Cast<VirtualListItem<IPriceListViewModel>>();
  ItemDelete(selectedItems.Select(x => ((IViewModelDetailBase)x.Data)).ToList());
} 
```

7. Implement IVirtualListLoader<T> interface

```
#region IVirtualListLoader<IPriceListViewModel> Members 
public bool CanSort { get { return false; } } 
 
public IList<IPriceListViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
{
  var retVal = new List<IPriceListViewModel>(); 
  using (var repository = _pricelistRepository.GetRepositoryInstance())
  {
    var query = repository.Pricelists; 
    if (!string.IsNullOrEmpty(SearchFilterKeyword))
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
public void InitializeForOpen()
{
  if (ListItemsSource == null)
  {
    OnUIThread(() => ListItemsSource = new VirtualList<IPriceListViewModel>(this, 25, SynchronizationContext.Current));
  }
}
#endregion
```

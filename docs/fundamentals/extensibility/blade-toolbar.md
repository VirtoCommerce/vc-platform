## Introduction and glossary

A blade toolbar is a dedicated area inside **blade** for adding **commands**. Hereinafter, the toolbar commands shall be referenced to as **toolbar items**.

![](../../media/image2015-7-9_18-36-5.png)

Sample depicting 4 commands inside toolbar.  

Toolbar items can be added to a **blade instance** directly or using Virto Commerce platform toolbar extensibility service.

## Adding toolbar items to blade instance directly.

Toolbar items are usually defined and added in angularJS controller, which is bound to blade (UI). Command adding to toolbar is straight forward: define **$scope.blade.toolbarCommands** array with necessary commands.

```JS
$scope.blade.toolbarCommands = [
  {
    name: "Save",
    icon: 'fa fa-save',
    title: 'Save action tooltip text', // or you can use keys for translation "platform.commands.titles.save"
    executeMethod: function () {
      $scope.saveChanges();
    },
    canExecuteMethod: function () {
      return isDirty();
    },
    permission: 'platform:security:manage'
  },
  {
    name: "Reset",
    icon: 'fa fa-undo',
    title: 'Reset action tooltip text',
    executeMethod: function () {
      angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
      userStateCommand.updateName();
    },
    canExecuteMethod: function () {
      return isDirty();
    },
    permission: 'platform:security:manage'
  },
  {
    name: "Change password",
    icon: 'fa fa-refresh',
    title: 'Change password action tooltip text',
    executeMethod: function () {
      var newBlade = {
        id: 'accountDetailChild',
        currentEntityId: $scope.blade.currentEntity.userName,
        title: $scope.blade.title,
        subtitle: "Change your password",
        controller: 'platformWebApp.accountChangePasswordController',
        template: 'Scripts/common/security/blades/account-changePassword.tpl.html'
      };
      bladeNavigationService.showBlade(newBlade, $scope.blade);
    },
    canExecuteMethod: function () {
      return true;
    },
    permission: 'platform:security:manage'
  }
];
```

## Adding toolbar items from external code

A dedicated service is used for adding toolbar items externally (without having a reference to blade instance). The preferred place for toolbar item registration is angularJS **module run** block. Reference to '**platformWebApp.toolbarService**' as **toolbarService**, create command option and call '**register**' on the service:

```JS
toolbarService.register({
  name: "ADDITIONAL COMMAND", icon: 'fa fa-cloud',
  title: 'Action command tooltip text',
  executeMethod: function (blade) {
    console.log('test: ' + this.name + this.icon + blade);
  },
  canExecuteMethod: function () { return true; },
  index: 2
}, 'virtoCommerce.marketingModule.itemsDynamicContentListController');
```

**toolbarService.register** parameters:

|Parameter|Description|
|---------|-----------|
|toolbarItem|Toolbar item definition.|
|toolbarController|The full toolbar controller name.|

Toolbar item definition structure:

|Member|Description|
|------|-----------|
|name|Display name for the command.|
|icon|The icon.|
|title|Tooltip message for the button.|
|executeMethod|Function to execute on command click. The current blade parameter is passed.|
|canExecuteMethod|Function to determine if command could be executed. The command is disabled in the toolbar if boolean *false *value**returned.|
|index|The 0 based position to insert the item in the toolbar.|

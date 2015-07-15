//Call this to register our module to main application
var moduleName = "virtoCommerce.avataxModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
])

.run(
  ['platformWebApp.toolbarService', 'platformWebApp.dialogService', 'virtoCommerce.avataxModule.resources', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', 'platformWebApp.authService', function (toolbarService, dialogService, avataxModuleResources, mainMenuService, widgetService, authService) {

      //Register widgets in catalog item details
      //widgetService.registerWidget({
      //    isVisible: function (blade) { return blade.currentEntity.id == 'Avalara.Tax' && authService.checkPermission('tax:manage'); },
      //    controller: 'virtoCommerce.avataxModule.avataxWidgetController',
      //    template: 'Modules/$(Avalara.Tax)/Scripts/widgets/avataxWidget.tpl.html'
      //}, 'moduleDetail');

      var validateCommand = {
          name: "Validate",
          icon: 'fa fa-check-square-o',
          index: 2,
          executeMethod: function (blade) {
              avataxModuleResources.validate(blade.currentEntity, function (result) {
                  var dialog = {
                      id: "avaTaxNotification",
                      title: "Address valid",
                      message: "Address validation passed."
                  };
                  dialogService.showNotificationDialog(dialog);
              },
                  function (error) {
                      var dialog = {
                          id: "avaTaxNotification",
                          title: "Validation error",
                          message: error.data.message
                      };
                      dialogService.showNotificationDialog(dialog);
                  });
          },
          canExecuteMethod: function () { return true; },
          permission: 'tax:manage'
      };

      toolbarService.register(validateCommand, 'virtoCommerce.coreModule.common.coreAddressDetailController');

      var testAvaTaxCommand = {
          name: "Test connection",
          icon: 'fa fa-check-square-o',
          index: 2,
          executeMethod: function () {
              avataxModuleResources.ping(function () {
                  var dialog = {
                      id: "avaTaxNotification",
                      title: "Success",
                      message: "Connected successfully!"
                  };
                  dialogService.showNotificationDialog(dialog);
              },
                  function (error) {
                      var dialog = {
                          id: "avaTaxNotification",
                          title: "Failed",
                          message: error.data.message
                      };
                      dialogService.showNotificationDialog(dialog);
                  });
          },
          canExecuteMethod: function () { return true; },
          permission: 'tax:manage'
      };

      toolbarService.register(testAvaTaxCommand, 'platformWebApp.settingsDetailController');
  }])
;
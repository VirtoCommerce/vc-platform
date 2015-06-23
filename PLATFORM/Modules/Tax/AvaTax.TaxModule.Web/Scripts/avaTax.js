//Call this to register our module to main application
var moduleName = "virtoCommerce.avataxModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
])

.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', 'platformWebApp.authService', function ($rootScope, mainMenuService, widgetService, authService) {
     
      //Register widgets in catalog item details
      widgetService.registerWidget({
          isVisible: function (blade) { return blade.currentEntity.id == 'Avalara.Tax' && authService.checkPermission('tax:manage'); },
          controller: 'virtoCommerce.avataxModule.avataxWidgetController',
          template: 'Modules/$(Avalara.Tax)/Scripts/widgets/avataxWidget.tpl.html'
      }, 'moduleDetail');
  }])
;
//Call this to register our module to main application
var moduleName = "virtoCommerce.gshoppingModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
])

.run(
  ['$rootScope', 'mainMenuService', 'widgetService', 'authService', function ($rootScope, mainMenuService, widgetService, authService) {
     
      //Register widgets in catalog item details
      /*widgetService.registerWidget({
          controller: 'virtoCommerce.gshoppingModule.gshoppingWidgetController',
          template: 'Modules/$(GoogleShopping.Merchant)/Scripts/widgets/gshoppingWidget.tpl.html'
      }, 'catalogDetail');*/

      widgetService.registerWidget({
          isVisible: function (blade) { return authService.checkPermission('googleShopping:manage'); },
          controller: 'virtoCommerce.gshoppingModule.gshoppingSyncCatWidgetController',
          size: [2, 1],
          template: 'Modules/$(GoogleShopping.Merchant)/Scripts/widgets/gshoppingWidget.tpl.html'
      }, 'categoryDetail');
  }]);
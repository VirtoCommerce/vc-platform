//Call this to register our module to main application
var moduleName = "virtoCommerce.amazonModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
])

.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', 'platformWebApp.authService', function ($rootScope, mainMenuService, widgetService, authService) {
     
      //Register widgets in catalog item details
      /*widgetService.registerWidget({
          controller: 'virtoCommerce.amazonModule.amazonWidgetController',
          template: 'Modules/$(Amazon.Merchant)/Scripts/widgets/amazonWidget.tpl.html'
      }, 'catalogDetail');*/

      widgetService.registerWidget({
          isVisible: function (blade) { return authService.checkPermission('amazon:manage'); },
          controller: 'virtoCommerce.amazonModule.amazonSyncCatWidgetController',
          size: [2, 1],
          template: 'Modules/$(Amazon.Merchant)/Scripts/widgets/amazonWidget.tpl.html'
      }, 'categoryDetail');
  }]);
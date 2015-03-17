//Call this to register our module to main application
var moduleName = "virtoCommerce.coreModule.fulfillment";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    'virtoCommerce.coreModule.fulfillment.widgets'
])
.run(
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      //Register module widget
      widgetService.registerWidget({
          controller: 'fulfillmentWidgetController',
          template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/widgets/fulfillmentWidget.tpl.html'
      }, 'moduleDetail');
  }])
;

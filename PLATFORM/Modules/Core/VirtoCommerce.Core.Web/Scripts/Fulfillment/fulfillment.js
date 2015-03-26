angular.module("virtoCommerce.coreModule.fulfillment", [])
.run(
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      //Register module widget
      widgetService.registerWidget({
          controller: 'fulfillmentWidgetController',
          template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/widgets/fulfillmentWidget.tpl.html'
      }, 'moduleDetail');

      //Register fulfillment center widgets
      widgetService.registerWidget({
          controller: 'fulfillmentCenterContactWidgetController',
          template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/widgets/fulfillmentCenterContactWidget.tpl.html'
      }, 'fulfillmentCenterDetail');
  }])
;

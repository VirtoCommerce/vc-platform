angular.module("virtoCommerce.coreModule.fulfillment", [])
.run(
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      //Register module widget
      widgetService.registerWidget({
          controller: 'fulfillmentWidgetController',
          template: 'Modules/Core/VirtoCommerce.Core.Web/Scripts/fulfillment/widgets/fulfillmentWidget.tpl.html'
      }, 'moduleDetail');
  }])
;

angular.module("virtoCommerce.coreModule.fulfillment", [])
.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      //Register module widget
      widgetService.registerWidget({
          isVisible: function (blade) { return blade.currentEntity.id == 'VirtoCommerce.Core'; },
          controller: 'virtoCommerce.coreModule.fulfillment.fulfillmentWidgetController',
          template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/widgets/fulfillmentWidget.tpl.html'
      }, 'moduleDetail');

      //Register fulfillment center widgets
      widgetService.registerWidget({
          controller: 'virtoCommerce.coreModule.fulfillment.fulfillmentCenterContactWidgetController',
          template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/widgets/fulfillmentCenterContactWidget.tpl.html'
      }, 'fulfillmentCenterDetail');
  }])
;

angular.module("virtoCommerce.coreModule.currency", [])
.run(
  ['platformWebApp.widgetService', function (widgetService) {
      //Register module widget
      widgetService.registerWidget({
          isVisible: function (blade) { return blade.currentEntity.id == 'VirtoCommerce.Core'; },
          controller: 'virtoCommerce.coreModule.currency.currencyWidgetController',
          template: 'Modules/$(VirtoCommerce.Core)/Scripts/currency/widgets/currencyWidget.tpl.html'
      }, 'moduleDetail');
  }])
;
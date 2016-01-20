angular.module("virtoCommerce.coreModule.currency", [])
.run(['platformWebApp.widgetService', function (widgetService) {
    //Register module widget
    widgetService.registerWidget({
        isVisible: function (blade) { return blade.currentEntity.id == 'VirtoCommerce.Core'; },
        controller: 'virtoCommerce.coreModule.currency.currencyWidgetController',
        template: 'Modules/$(VirtoCommerce.Core)/Scripts/currency/widgets/currencyWidget.tpl.html'
    }, 'moduleDetail');
}])

.factory('virtoCommerce.coreModule.currency.currencyUtils', ['virtoCommerce.coreModule.currency.currencyApi', 'platformWebApp.bladeNavigationService', function (currencyApi, bladeNavigationService) {
    var currenciesRef;
    return {
        getCurrencies: function () { return currenciesRef = currencyApi.query(); },
        editCurrencies: function (blade) {
            var newBlade = {
                id: 'currencyList',
                parentRefresh: function (data) { angular.copy(data, currenciesRef); },
                controller: 'virtoCommerce.coreModule.currency.currencyListController',
                template: 'Modules/$(VirtoCommerce.Core)/Scripts/currency/blades/currency-list.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        }
    };
}])
;
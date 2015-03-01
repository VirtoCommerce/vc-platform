//Call this to register our module to main application
var moduleName = "virtoCommerce.customerModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    'virtoCommerce.customerModule.blades',
    'virtoCommerce.customerModule.widgets'
])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.customerModule', {
              url: '/customers',
              // templateUrl: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/home.tpl.html',
              templateUrl: 'Modules/Core/VirtoCommerce.Core.Web/Scripts/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'customer',
                          title: 'Customers',
                          controller: 'customerListController',
                          template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/blades/!customer-list.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                  }
              ]
          });
  }]
)
.run(
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/customer',
          icon: 'fa fa-shopping-cart',
          title: 'customers',
          priority: 180,
          action: function () { $state.go('workspace.customerModule'); },
          permission: 'customersMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register widgets in customer details
      widgetService.registerWidget({
          controller: 'customerAddressesWidgetController',
          template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/widgets/!customerAddressesWidget.tpl.html'
      }, 'customerDetail1');
      widgetService.registerWidget({
          controller: 'customerEmailsWidgetController',
          template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/widgets/customerEmailsWidget.tpl.html'
      }, 'customerDetail1');
      widgetService.registerWidget({
          controller: 'customerPhonesWidgetController',
          template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/widgets/customerPhonesWidget.tpl.html'
      }, 'customerDetail1');
      

      widgetService.registerWidget({
          controller: 'customerPropertyWidgetController',
          template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/widgets/customerPropertyWidget.tpl.html'
      }, 'customerDetail2');
  }]);
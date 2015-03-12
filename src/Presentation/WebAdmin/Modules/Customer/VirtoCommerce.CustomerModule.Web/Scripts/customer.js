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
                          id: 'memberList',
                          breadcrumbs: [],
                          controller: 'memberListController',
                          template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/blades/member-list.tpl.html',
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
          path: 'browse/member',
          icon: 'fa fa-user',
          title: 'customers',
          priority: 180,
          action: function () { $state.go('workspace.customerModule'); },
          permission: 'customersMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register widgets in customer details
      widgetService.registerWidget({
          controller: 'memberAddressesWidgetController',
          template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/widgets/!memberAddressesWidget.tpl.html'
      }, 'customerDetail1');
      widgetService.registerWidget({
          controller: 'memberEmailsWidgetController',
          template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/widgets/memberEmailsWidget.tpl.html'
      }, 'customerDetail1');
      widgetService.registerWidget({
          controller: 'memberPhonesWidgetController',
          template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/widgets/memberPhonesWidget.tpl.html'
      }, 'customerDetail1');
      

      widgetService.registerWidget({
          controller: 'memberPropertyWidgetController',
          template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/widgets/memberPropertyWidget.tpl.html'
      }, 'customerDetail2');
  }]);
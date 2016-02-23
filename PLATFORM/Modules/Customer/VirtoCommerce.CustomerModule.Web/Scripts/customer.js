//Call this to register our module to main application
var moduleName = "virtoCommerce.customerModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.customerModule', {
              url: '/customers',
              templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
              controller: [
                  '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'memberList',
                          currentEntity: { id: null },
                          controller: 'virtoCommerce.customerModule.memberListController',
                          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-list.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                  }
              ]
          });
  }]
)
.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/member',
          icon: 'fa fa-user',
          title: 'customers',
          priority: 180,
          action: function () { $state.go('workspace.customerModule'); },
          permission: 'customer:access'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register widgets in customer details
      widgetService.registerWidget({
          controller: 'virtoCommerce.customerModule.memberAccountsWidgetController',
          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/memberAccountsWidget.tpl.html'
      }, 'customerDetail1');
      widgetService.registerWidget({
          controller: 'virtoCommerce.customerModule.memberAddressesWidgetController',
          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/memberAddressesWidget.tpl.html'
      }, 'customerDetail1');
      widgetService.registerWidget({
          controller: 'virtoCommerce.customerModule.memberEmailsWidgetController',
          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/memberEmailsWidget.tpl.html'
      }, 'customerDetail1');
      widgetService.registerWidget({
          controller: 'virtoCommerce.customerModule.memberPhonesWidgetController',
          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/memberPhonesWidget.tpl.html'
      }, 'customerDetail2');
      widgetService.registerWidget({
          controller: 'platformWebApp.dynamicPropertyWidgetController',
          template: '$(Platform)/Scripts/app/dynamicProperties/widgets/dynamicPropertyWidget.tpl.html'
      }, 'customerDetail2');

      //Register widgets in organization details
      widgetService.registerWidget({
          controller: 'virtoCommerce.customerModule.memberAddressesWidgetController',
          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/memberAddressesWidget.tpl.html'
      }, 'organizationDetail1');
      widgetService.registerWidget({
          controller: 'virtoCommerce.customerModule.memberEmailsWidgetController',
          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/memberEmailsWidget.tpl.html'
      }, 'organizationDetail1');
      widgetService.registerWidget({
          controller: 'virtoCommerce.customerModule.memberPhonesWidgetController',
          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/memberPhonesWidget.tpl.html'
      }, 'organizationDetail1');
      widgetService.registerWidget({
          controller: 'platformWebApp.dynamicPropertyWidgetController',
          template: '$(Platform)/Scripts/app/dynamicProperties/widgets/dynamicPropertyWidget.tpl.html'
      }, 'organizationDetail2');
  }]);
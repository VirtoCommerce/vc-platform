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
              // templateUrl: 'Modules/$(VirtoCommerce.Customer)/Scripts/home.tpl.html',
              templateUrl: 'Modules/$(VirtoCommerce.Core)/Scripts/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'memberList',
                          currentEntity: {},
                          controller: 'virtoCommerce.customerModule.memberListController',
                          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-list.tpl.html',
                          isClosingDisabled: true
                      };

                      blade.breadcrumbs = [{
                          id: null,
                          name: "all",
                          navigate: function () {
                              bladeNavigationService.closeBlade(blade,
                              function () {
                                  bladeNavigationService.showBlade(blade);
                                  blade.refresh();
                              });
                          }
                      }];

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
          permission: 'customer:query'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register widgets in customer details
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
      }, 'customerDetail1');
      widgetService.registerWidget({
          controller: 'virtoCommerce.customerModule.memberPropertyWidgetController',
          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/memberPropertyWidget.tpl.html'
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
      //widgetService.registerWidget({
      //    controller:'virtoCommerce.customerModule.memberPropertyWidgetController',
      //    template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/memberPropertyWidget.tpl.html'
      //}, 'organizationDetail2');
  }]);
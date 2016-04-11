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
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', 'virtoCommerce.customerModule.memberTypesResolverService', function ($rootScope, mainMenuService, widgetService, $state, memberTypesResolverService) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/member',
          icon: 'fa fa-user',
          title: 'customer.main-menu-title',
          priority: 180,
          action: function () { $state.go('workspace.customerModule'); },
          permission: 'customer:access'
      };
      mainMenuService.addMenuItem(menuItem);

      var accountsWidget = {
          isVisible: function (blade) { return !blade.isNew; },
          controller: 'virtoCommerce.customerModule.customerAccountsWidgetController',
          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/customerAccountsWidget.tpl.html'
      };
      var addressesWidget = {
          controller: 'virtoCommerce.customerModule.memberAddressesWidgetController',
          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/memberAddressesWidget.tpl.html'
      };
      var emailsWidget = {
          controller: 'virtoCommerce.customerModule.memberEmailsWidgetController',
          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/memberEmailsWidget.tpl.html'
      };
      var phonesWidget = {
          controller: 'virtoCommerce.customerModule.memberPhonesWidgetController',
          template: 'Modules/$(VirtoCommerce.Customer)/Scripts/widgets/memberPhonesWidget.tpl.html'
      };
      var dynamicPropertyWidget = {
          controller: 'platformWebApp.dynamicPropertyWidgetController',
          template: '$(Platform)/Scripts/app/dynamicProperties/widgets/dynamicPropertyWidget.tpl.html'
      }
      //Register widgets in customer details
      widgetService.registerWidget(accountsWidget, 'customerDetail1');
      widgetService.registerWidget(addressesWidget, 'customerDetail1');
      widgetService.registerWidget(emailsWidget, 'customerDetail1');
      widgetService.registerWidget(phonesWidget, 'customerDetail2');
      widgetService.registerWidget(dynamicPropertyWidget, 'customerDetail2');

      //Register widgets in organization details
      widgetService.registerWidget(addressesWidget, 'organizationDetail1');
      widgetService.registerWidget(emailsWidget, 'organizationDetail1');
      widgetService.registerWidget(phonesWidget, 'organizationDetail1');
      widgetService.registerWidget(dynamicPropertyWidget, 'organizationDetail2');

      //Register widgets in employee details
      widgetService.registerWidget(accountsWidget, 'employeeDetail1');
      widgetService.registerWidget(addressesWidget, 'employeeDetail1');
      widgetService.registerWidget(emailsWidget, 'employeeDetail1');
      widgetService.registerWidget(phonesWidget, 'employeeDetail2');
      widgetService.registerWidget(dynamicPropertyWidget, 'employeeDetail2');

      //Register widgets in vendor details
      widgetService.registerWidget(addressesWidget, 'vendorDetail1');
      widgetService.registerWidget(emailsWidget, 'vendorDetail1');
      widgetService.registerWidget(phonesWidget, 'vendorDetail1');
      widgetService.registerWidget(dynamicPropertyWidget, 'vendorDetail2');

      // register member types
      memberTypesResolverService.registerType({ memberType: 'Organization', fullTypeName: 'VirtoCommerce.Domain.Customer.Model.Organization', icon: 'fa-university', template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/organization-detail.tpl.html' });
      memberTypesResolverService.registerType({ memberType: 'Employee', fullTypeName: 'VirtoCommerce.Domain.Customer.Model.Employee', icon: ' fa-user', template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/employee-detail.tpl.html' });
      memberTypesResolverService.registerType({ memberType: 'Contact', fullTypeName: 'VirtoCommerce.Domain.Customer.Model.Contact', icon: 'fa-smile-o', template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/customer-detail.tpl.html' });
      memberTypesResolverService.registerType({ memberType: 'Vendor', fullTypeName: 'VirtoCommerce.Domain.Customer.Model.Vendor', icon: 'fa-balance-scale', template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/vendor-detail.tpl.html', topLevelElementOnly: true });
  }]);
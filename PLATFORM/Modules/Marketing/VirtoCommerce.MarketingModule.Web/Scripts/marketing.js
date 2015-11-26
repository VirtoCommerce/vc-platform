﻿//Call this to register our module to main application
var moduleName = "virtoCommerce.marketingModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.marketing', {
              url: '/marketing',
              templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
              controller: [
                  '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'marketing',
                          title: 'marketing.blades.marketing-main.title',
                          controller: 'virtoCommerce.marketingModule.marketingMainController',
                          template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/common/marketing-main.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                  }
              ]
          });
  }]
)
.run(
  ['platformWebApp.mainMenuService', 'platformWebApp.widgetService', 'platformWebApp.toolbarService', '$state', 'platformWebApp.authService', function (mainMenuService, widgetService, toolbarService, $state, authService) {
      // // test toolbar commands and content
      //toolbarService.register({
      //    name: "ADDITIONAL COMMAND", icon: 'fa fa-cloud',
      //    executeMethod: function (blade) {
      //        console.log('test: ' + this.name + this.icon + blade);
      //    },
      //    canExecuteMethod: function () { return true; },
      //    index: 2
      //}, 'virtoCommerce.marketingModule.itemsDynamicContentListController');
      //toolbarService.register({
      //    name: "EXTERNAL ACTION", icon: 'fa fa-bolt',
      //    executeMethod: function (blade) {
      //        console.log('test: ' + this.name + this.icon + blade);
      //    },
      //    canExecuteMethod: function () { return true; },
      //    index: 0
      //}, 'virtoCommerce.marketingModule.itemsDynamicContentListController');
      
      //Register module in main menu
      var menuItem = {
          path: 'browse/marketing',
          icon: 'fa fa-flag',
          title: 'marketing.main-menu-title',
          priority: 40,
          action: function () { $state.go('workspace.marketing'); },
          permission: 'marketing:access'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register dashboard widgets
      //widgetService.registerWidget({
      //    isVisible: function (blade) { return authService.checkPermission('marketing:read'); },
      //    controller: 'virtoCommerce.marketingModule.dashboard.promotionsWidgetController',
      //    template: 'tile-count.html'
      //}, 'mainDashboard');
  }]);
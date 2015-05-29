//Call this to register our module to main application
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
              templateUrl: 'Scripts/common/templates/home.tpl.html',
              controller: [
                  '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'marketing',
                          title: 'Marketing',
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
      //    name: "test 1", icon: 'fa fa-cloud',
      //    executeMethod: function (blade) {
      //        console.log('test: ' + this.name + this.icon + blade);
      //    },
      //    canExecuteMethod: function () { return true; },
      //    permission: 'catalog:catalogs:manage',
      //    index: 2
      //}, 'virtoCommerce.marketingModule.promotionDetailController');
      //toolbarService.register({
      //    name: "DO DO DO", icon: 'fa fa-bolt',
      //    executeMethod: function (blade) {
      //        console.log('test: ' + this.name + this.icon + blade);
      //    },
      //    canExecuteMethod: function () { return true; },
      //    permission: 'catalog:catalogs:manage',
      //    index: 0
      //}, 'virtoCommerce.marketingModule.promotionDetailController');

      //toolbarService.register({
      //    template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/toolbar-isActive.tpl.html',
      //    index: 1
      //}, 'virtoCommerce.marketingModule.promotionDetailController', true);
      //toolbarService.register({
      //    template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/operation-detail-toolbar.tpl.html',
      //    index: 0
      //}, 'virtoCommerce.marketingModule.promotionDetailController', true);

      //Register module in main menu
      var menuItem = {
          path: 'browse/marketing',
          icon: 'fa fa-flag',
          title: 'Marketing',
          priority: 40,
          action: function () { $state.go('workspace.marketing'); },
          permission: 'marketing:query'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register dashboard widgets
      widgetService.registerWidget({
          isVisible: function (blade) { return authService.checkPermission('marketing:query'); },
          controller: 'virtoCommerce.marketingModule.dashboard.promotionsWidgetController',
          template: 'tile-count.html'
      }, 'mainDashboard');
  }]);
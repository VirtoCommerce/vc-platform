//Call this to register our module to main application
var moduleName = "virtoCommerce.quoteModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.quoteModule', {
              url: '/quotes',
              templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
              controller: [
                  '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'quote',
                          title: 'Quotes',
                          controller: 'virtoCommerce.quoteModule.quotesListController',
                          template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quotes-list.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                  	 //Need for isolate and prevent conflict module css to another modules 
                  	 //it value included in bladeContainer as ng-class='moduleName'
                      $scope.moduleName = "vc-quote";
                  }
              ]
          });
  }]
)
.run(
  ['platformWebApp.toolbarService', 'platformWebApp.bladeNavigationService', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', function (toolbarService, bladeNavigationService, mainMenuService, widgetService, $state) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/quote',
          icon: 'fa fa-file-text-o',
          title: 'quotes',
          priority: 95,
          action: function () { $state.go('workspace.quoteModule'); },
          permission: 'quote:query'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register widgets in quote details
      //widgetService.registerWidget({
      //    size: [2, 1],
      //    controller: 'virtoCommerce.quoteModule.quoteAddressWidgetController',
      //    template: 'Modules/$(VirtoCommerce.quote)/Scripts/widgets/quote-address-widget.tpl.html'
      //}, 'quoteDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.quoteModule.quoteItemsWidgetController',
          template: 'Modules/$(VirtoCommerce.quote)/Scripts/widgets/quote-items-widget.tpl.html'
      }, 'quoteDetail');
  }])
;
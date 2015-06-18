var moduleName = "virtoCommerce.searchModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    'ngSanitize'
])
.run(
  ['platformWebApp.toolbarService', 'platformWebApp.bladeNavigationService', function (toolbarService, bladeNavigationService) {

      var rebuildIndexCommand = {
          name: "Rebuild Index",
          icon: 'fa fa-refresh',
          index: 4,
          executeMethod: function (blade) {
              var newBlade = {
                  id: 'rebuildIndex',
                  controller: 'virtoCommerce.searchModule.rebuildIndexController',
                  template: 'Modules/$(VirtoCommerce.Search)/Scripts/blades/rebuildIndex.tpl.html'
              };
              bladeNavigationService.showBlade(newBlade, blade);
          },
          canExecuteMethod: function () { return true; },
          permission: 'VirtoCommerce.Search:Index:Rebuild'
      };

      toolbarService.register(rebuildIndexCommand, 'virtoCommerce.catalogModule.catalogsListController');
  }]
);

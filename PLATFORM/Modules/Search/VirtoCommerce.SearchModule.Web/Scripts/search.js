﻿var moduleName = "virtoCommerce.searchModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    'ngSanitize'
])
.run(
  ['platformWebApp.toolbarService', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function (toolbarService, bladeNavigationService, dialogService) {

      var rebuildIndexCommand = {
          name: "Rebuild Index",
          icon: 'fa fa-recycle',
          index: 4,
          executeMethod: function (blade) {
              var dialog = {
                  id: "confirmRebuildIndex",
                  title: "search.dialogs.rebuild-index.title",
                  message: "search.dialogs.rebuild-index.message",
                  callback: function (confirm) {
                      if (confirm) {
                          var newBlade = {
                              id: 'rebuildIndex',
                              controller: 'virtoCommerce.searchModule.rebuildIndexController',
                              template: 'Modules/$(VirtoCommerce.Search)/Scripts/blades/rebuildIndex.tpl.html'
                          };
                          bladeNavigationService.showBlade(newBlade, blade);
                      }
                  }
              }
              dialogService.showConfirmationDialog(dialog);
          },
          canExecuteMethod: function () { return true; },
          permission: 'VirtoCommerce.Search:Index:Rebuild'
      };

      toolbarService.register(rebuildIndexCommand, 'virtoCommerce.catalogModule.catalogsListController');
  }]
);

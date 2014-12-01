angular.module('catalogModule.blades.import.importJobList', ['catalogModule.resources.import'])
.controller('importJobListController', ['$scope', 'bladeNavigationService', 'dialogService', 'imports', function ($scope, bladeNavigationService, dialogService, imports)
{
    $scope.selectedAll = false;
    $scope.selectedItem = null;

    $scope.blade.refresh = function ()
    {
        $scope.blade.isLoading = true;

        var result = imports.list({ catalogId: $scope.blade.catalogId }, function (results)
        {
            $scope.blade.isLoading = false;
            $scope.items = results;

        });

    };

    $scope.setSelectedItem = function (data)
    {
        $scope.selectedItem = data;
    }

    $scope.checkAll = function (selected)
    {
        angular.forEach($scope.items, function (item)
        {
            item.selected = selected;
        });
    };

    function deleteChecked()
    {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected import jobs?",
            callback: function (remove)
            {
                if (remove)
                {
                    closeChildrenBlades();

                    var selection = _.where($scope.items, { selected: true });

                    var itemIds = [];
                    angular.forEach(selection, function (listItem)
                    {
                        itemIds.push(listItem.id);
                    });
                    if (itemIds.length > 0)
                    {
                        imports.remove({ ids: itemIds }, function (data, headers)
                        {
                            $scope.blade.refresh();
                        });
                    }
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    function closeChildrenBlades()
    {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child)
        {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.bladeToolbarCommands = [
          {
              name: "Refresh", icon: 'icon-spin',
              executeMethod: function ()
              {
                  $scope.blade.refresh();
              },
              canExecuteMethod: function ()
              {
                  return true;
              }
          },
        {
            name: "Add", icon: 'icon-plus',
            executeMethod: function ()
            {
                imports.new({ catalogId: $scope.blade.catalogId }, function (data)
                {
                    var newBlade = {
                        id: 'newImportJobWizard',
                        item: data,
                        title: 'New import job',
                        subtitle: 'Create an import job',
                        controller: 'importJobWizardController',
                        bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/importWizard/import-job-wizard-create-action.tpl.html',
                        template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/importWizard/import-job-wizard.tpl.html'
                    };
                    closeChildrenBlades();
                    bladeNavigationService.showBlade(newBlade, $scope.blade);
                });
            },
            canExecuteMethod: function ()
            {
                return true;
            }
        },
        {
            name: "Manage", icon: 'icon-new-tab-2',
            executeMethod: function ()
            {
                var newBlade = {
                    id: 'importJobWizard',
                    item: $scope.selectedItem,
                    title: 'Edit import job',
                    subtitle: 'Manage an import job',
                    controller: 'importJobWizardController',
                    bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/importWizard/import-job-wizard-update-action.tpl.html',
                    template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/importWizard/import-job-wizard.tpl.html'
                };
                closeChildrenBlades();
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function ()
            {
                return $scope.selectedItem;
            }
        },
        {
            name: "Run", icon: 'icon-enter',
            executeMethod: function ()
            {
                //TODO
            },
            canExecuteMethod: function ()
            {
                return $scope.selectedItem;
            }
        },
        {
            name: "Delete", icon: 'icon-remove',
            executeMethod: function ()
            {
                deleteChecked();
            },
            canExecuteMethod: function ()
            {
                return _.some($scope.items, { selected: true });;
            }
        }
    ];

    $scope.blade.refresh();

}]);

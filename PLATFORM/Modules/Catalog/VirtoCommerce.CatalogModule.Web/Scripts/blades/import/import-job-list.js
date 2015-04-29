angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.importJobListController', ['$scope', 'bladeNavigationService', 'dialogService', 'virtoCommerce.catalogModule.imports', function ($scope, bladeNavigationService, dialogService, imports) {
    $scope.selectedAll = false;
    $scope.selectedItem = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        imports.list({ catalogId: $scope.blade.catalogId }, function (results) {
            $scope.blade.isLoading = false;
            $scope.items = results;
        });
    };

    $scope.setSelectedItem = function (data) {
        $scope.selectedItem = data;
    }

    $scope.checkAll = function (selected) {
        angular.forEach($scope.items, function (item) {
            item.selected = selected;
        });
    };

    $scope.edit = function (listItem) {
        closeChildrenBlades();
        $scope.setSelectedItem(listItem);

        var newBlade = {
            id: 'importJobWizard',
            item: $scope.selectedItem,
            isNew: false,
            title: 'Edit import job',
            subtitle: 'Manage import job',
            controller: 'virtoCommerce.catalogModule.importJobWizardController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/importWizard/import-job-wizard.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.run = function (listItem) {
        closeChildrenBlades();
        $scope.setSelectedItem(listItem);

        var newBlade = {
            id: 'runImportJob',
            item: $scope.selectedItem,
            title: 'Run import job',
            subtitle: 'Run import job',
            controller: 'virtoCommerce.catalogModule.importJobRunController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/import/import-job-run.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.delete = function () {
        if (isAnyChecked()) {
            deleteChecked();
        } else {
            var dialog = {
                id: "notifyNoTargetCategory",
                title: "Message",
                message: "Nothing selected. Check some Import jobs first."
            };
            dialogService.showNotificationDialog(dialog);
        }
    };

    function isAnyChecked() {
        return _.some($scope.items, { selected: true });
    };

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected Import jobs?",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var selection = _.where($scope.items, { selected: true });

                    var itemIds = [];
                    angular.forEach(selection, function (listItem) {
                        itemIds.push(listItem.id);
                    });
                    if (itemIds.length > 0) {
                        imports.remove({ ids: itemIds }, function (data, headers) {
                            $scope.blade.refresh();
                        });
                    }
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.bladeToolbarCommands = [
          {
              name: "Refresh", icon: 'fa fa-refresh',
              executeMethod: function () {
                  $scope.blade.refresh();
              },
              canExecuteMethod: function () {
                  return true;
              }
          },
        {
            name: "Add", icon: 'fa fa-plus',
            executeMethod: function () {
                imports.new({ catalogId: $scope.blade.catalogId }, function (data) {
                    var newBlade = {
                        id: 'newImportJobWizard',
                        item: data,
                        isNew: true,
                        title: 'New import job',
                        subtitle: 'Create an import job',
                        controller: 'virtoCommerce.catalogModule.importJobWizardController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/importWizard/import-job-wizard.tpl.html'
                    };
                    closeChildrenBlades();
                    bladeNavigationService.showBlade(newBlade, $scope.blade);
                });
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'catalog:catalogs:manage'
        },
        {
            name: "Manage", icon: 'fa fa-edit',
            executeMethod: function () {
                $scope.edit($scope.selectedItem);
            },
            canExecuteMethod: function () {
                return $scope.selectedItem;
            }
        },
        {
            name: "Run", icon: 'fa fa-sign-in',
            executeMethod: function () {
                $scope.run($scope.selectedItem);
            },
            canExecuteMethod: function () {
                return $scope.selectedItem;
            },
            permission: 'catalog:catalogs:manage'
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteChecked();
            },
            canExecuteMethod: function () {
                return isAnyChecked();
            },
            permission: 'catalog:catalogs:manage'
        }
    ];


    $scope.blade.refresh();
}]);

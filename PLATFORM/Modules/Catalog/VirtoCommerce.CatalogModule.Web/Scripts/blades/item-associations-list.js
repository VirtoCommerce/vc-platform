﻿angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemAssociationsListController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.catalogModule.items', function ($rootScope, $scope, bladeNavigationService, dialogService, items) {

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        $scope.blade.parentBlade.refresh().$promise.then(function (data) {
            initializeBlade(data.associations);
        });
    };

    function initializeBlade(data) {
        $scope.selectedAll = false;
        $scope.blade.currentEntities = angular.copy(data);
        $scope.blade.origItem = data;
        $scope.blade.isLoading = false;

        $scope.blade.currentEntities.sort(function (a, b) {
            return a.priority > b.priority;
        });
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    function openAddEntityWizard() {
        var newBlade = {
            id: "associationWizard",
            associations: $scope.blade.currentEntities,
            controller: 'virtoCommerce.catalogModule.associationWizardController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/newAssociation/association-wizard.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                openAddEntityWizard();
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'catalog:update'
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                var dialog = {
                    id: "confirmDeleteItem",
                    title: "catalog.dialogs.association-delete.title",
                    message: "catalog.dialogs.association-delete.message",
                    callback: function (remove) {
                        if (remove) {
                            $scope.blade.isLoading = true;
                            closeChildrenBlades();

                            var undeletedEntries = _.reject($scope.blade.currentEntities, function (x) { return x.selected; });
                            items.update({ id: $scope.blade.currentEntityId, associations: undeletedEntries }, function () {
                                $scope.blade.refresh();
                            },
                            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
                        }
                    }
                }

                dialogService.showConfirmationDialog(dialog);
            },
            canExecuteMethod: function () {
                return _.any($scope.blade.currentEntities, function (x) { return x.selected; });;
            },
            permission: 'catalog:update'
        }
    ];

    $scope.checkAll = function (selected) {
        angular.forEach($scope.blade.currentEntities, function (item) {
            item.selected = selected;
        });
    };

    $scope.sortableOptions = {
        stop: function (e, ui) {
            for (var i = 0; i < $scope.blade.currentEntities.length; i++) {
                $scope.blade.currentEntities[i].priority = i + 1;
            }

            items.update({ id: $scope.blade.currentEntityId, associations: $scope.blade.currentEntities }, function () {
                $scope.blade.refresh();
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        },
        axis: 'y',
        cursor: "move"
    };


    initializeBlade($scope.blade.currentEntities);
}]);

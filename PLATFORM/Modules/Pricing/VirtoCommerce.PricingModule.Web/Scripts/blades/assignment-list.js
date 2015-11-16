angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.assignmentListController', ['$scope', 'virtoCommerce.pricingModule.pricelistAssignments', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService',
function ($scope, assignments, bladeNavigationService, dialogService) {
    var selectedNode = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        $scope.blade.selectedAll = false;

        assignments.query({}, function (data) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = data;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'listItemChild',
            currentEntityId: selectedNode.id,
            title: selectedNode.name,
            subtitle: $scope.blade.subtitle,
            controller: 'virtoCommerce.pricingModule.assignmentDetailController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/assignment-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.toggleAll = function () {
        angular.forEach($scope.blade.currentEntities, function (item) {
            item.selected = $scope.blade.selectedAll;
        });
    };

    function isItemsChecked() {
        return $scope.blade.currentEntities && _.any($scope.blade.currentEntities, function (x) { return x.selected; });
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "pricing.dialogs.assignments-delete.title",
            message: "pricing.dialogs.assignments-delete.message",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var selection = _.where($scope.blade.currentEntities, { selected: true });
                    var itemIds = _.pluck(selection, 'id');
                    assignments.remove({ ids: itemIds }, function () {
                        $scope.blade.refresh();
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, $scope.blade);
                    });
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

    $scope.blade.headIcon = 'fa-usd';

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.refresh", icon: 'fa fa-refresh',
            executeMethod: function () {
                $scope.blade.refresh();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                closeChildrenBlades();

                var newBlade = {
                    id: 'listItemChild',
                    title: 'pricing.blades.assignment-detail.new-title',
                    subtitle: $scope.blade.subtitle,
                    isNew: true,
                    controller: 'virtoCommerce.pricingModule.assignmentDetailController',
                    template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/assignment-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'pricing:create'
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteChecked();
            },
            canExecuteMethod: function () {
                return isItemsChecked();
            },
            permission: 'pricing:delete'
        }
    ];

    // actions on load
    $scope.blade.refresh();
}]);
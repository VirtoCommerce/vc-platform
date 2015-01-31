angular.module('virtoCommerce.orderModule.blades')
.controller('customerOrderDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'customerOrders', function ($scope, dialogService, bladeNavigationService, customerOrders) {

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        customerOrders.get({ id: $scope.blade.currentEntityId }, function (results) {
            $scope.blade.currentEntity = angular.copy(results);
            $scope.blade.origEntity = results;
            $scope.blade.isLoading = false;

            var newBlade = {
                id: 'customerOrderItems',
                title: $scope.blade.title + ' items',
                subtitle: 'Edit order items',
                currentEntity: $scope.blade.currentEntity,
                isClosingDisabled: true,
                controller: 'customerOrderItemsController',
                template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/customerOrder-items.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade);
        },
        function (error) {
            $scope.blade.isLoading = false;
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        customerOrders.update({}, $scope.blade.currentEntity, function (data, headers) {
            $scope.blade.refresh();
        });
    };

    $scope.bladeToolbarCommands = [
        {
            name: "New document", icon: 'fa fa-plus',
            executeMethod: function () {
                openAddEntityWizard();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "Save", icon: 'icon-floppy',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Reset", icon: 'icon-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        }
    ];

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The order has been modified. Do you want to save changes?",
                callback: function (needSave) {
                    if (needSave) {
                        saveChanges();
                    }
                    closeChildrenBlades();
                    closeCallback();
                }
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeChildrenBlades();
            closeCallback();
        }
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    // actions on load
    $scope.toolbarTemplate = 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/customerOrder-detail-toolbar.tpl.html';
    $scope.blade.refresh();

}]);
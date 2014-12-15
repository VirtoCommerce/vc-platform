angular.module('virtoCommerce.packaging.blades.moduleDetail', [])
.controller('moduleDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'modules', function ($scope, dialogService, bladeNavigationService, modules) {

    function initializeBlade(data) {
        $scope.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
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

    $scope.bladeToolbarCommands = [
        {
            name: "Update", icon: 'icon-arrow-up',
            executeMethod: function () {
                openUpdateEntityBlade();
            },
            canExecuteMethod: function () {
                // return $scope.currentEntity.isRemovable;
                return true;
            }
        },
        {
            name: "Delete", icon: 'icon-remove',
            executeMethod: function () {
                openDeleteEntityBlade();
            },
            canExecuteMethod: function () {
                // return $scope.currentEntity.isRemovable;
                return true;
            }
        }
    ];

    function openUpdateEntityBlade() {
        closeChildrenBlades();

    }

    function openDeleteEntityBlade() {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to uninstall this Module?",
            callback: function (remove) {
                if (remove) {
                    /*
                    $scope.blade.isLoading = true;

                    var idx = $scope.blade.parentBlade.currentEntities.indexOf($scope.blade.origEntity);
                    if (idx >= 0) {
                        var entriesCopy = $scope.blade.parentBlade.currentEntities.slice();
                        entriesCopy.splice(idx, 1);
                        items.updateitem({ id: $scope.blade.parentBlade.currentEntityId, reviews: entriesCopy }, function () {
                            $scope.bladeClose();
                            $scope.blade.parentBlade.refresh(true);
                        });
                    }
                    */
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    // on load
    initializeBlade($scope.blade.currentEntity);
}]);

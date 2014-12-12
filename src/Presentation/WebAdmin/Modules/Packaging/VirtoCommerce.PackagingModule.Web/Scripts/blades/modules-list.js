angular.module('virtoCommerce.packaging.blades.modulesList', [
    'virtoCommerce.packaging.resources.modules'
])
.controller('modulesListController', ['$rootScope', '$scope', 'bladeNavigationService', 'dialogService', 'modules', function ($rootScope, $scope, bladeNavigationService, dialogService, modules) {
    $scope.selectedEntityId = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        modules.getModules({}, function (results) {
            $scope.blade.isLoading = false;

            $scope.currentEntities = results;
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

    $scope.bladeToolbarCommands = [
        {
            name: "Add", icon: 'icon-plus',
            executeMethod: function () {
                openAddEntityBlade();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "Update", icon: 'icon-arrow-up',
            executeMethod: function () {
                openUpdateEntityBlade();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "View", icon: 'fa fa-eye',
            executeMethod: function () {
                openViewEntityBlade();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "Delete", icon: 'icon-remove',
            executeMethod: function () {
                openDeleteEntityBlade();
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    function openAddEntityBlade() {
        closeChildrenBlades();
    }

    function openUpdateEntityBlade() {
        closeChildrenBlades();
    }

    function openViewEntityBlade() {
        closeChildrenBlades();
    }

    function openDeleteEntityBlade() {
        closeChildrenBlades();
    }

    $scope.blade.refresh();
}]);

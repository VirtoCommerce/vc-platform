angular.module('virtoCommerce.customerModule.blades')
.controller('customerAddressListController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.selectedItem = null;

    $scope.openBlade = function (data) {
        $scope.selectedItem = data;

        var newBlade = {
            id: 'customerAddressDetail',
            origEntity: data,
            title: $scope.blade.title,
            subtitle: 'Edit address',
            controller: 'customerAddressDetailController',
            template: 'Modules/customer/VirtoCommerce.customerModule.Web/Scripts/blades/customer-address-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    function openAddEntityBlade() {
        var data = {
            isNew: true
        };
        $scope.openBlade(data);
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Add", icon: 'fa fa-plus',
            executeMethod: function () {
                openAddEntityBlade();
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    $scope.blade.isLoading = false;
    $scope.$watch('blade.parentBlade.currentEntity.addresses', function (currentEntities) {
        $scope.blade.currentEntities = currentEntities;
    });

    // open blade for new setting
    if (!_.some($scope.blade.currentEntities)) {
        openAddEntityBlade();
    }
}]);

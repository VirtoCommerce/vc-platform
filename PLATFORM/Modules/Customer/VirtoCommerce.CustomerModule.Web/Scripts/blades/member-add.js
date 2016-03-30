angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberAddController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.addOrganization = function () {
        bladeNavigationService.closeBlade(blade, function () {
            blade.parentBlade.showDetailBlade({ memberType: 'Organization' }, 'customer.blades.new-organization.title');
        });
    };

    $scope.addCustomer = function () {
        bladeNavigationService.closeBlade($scope.blade, function () {
            blade.parentBlade.showDetailBlade({ memberType: 'Contact' }, 'customer.blades.new-customer.title');
        });
    };

    blade.headIcon = blade.parentBlade.headIcon;
    blade.isLoading = false;
}]);

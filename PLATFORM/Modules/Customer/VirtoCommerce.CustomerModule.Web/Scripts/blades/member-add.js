angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberAddController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var pb = $scope.blade.parentBlade;

    $scope.addOrganization = function () {
        bladeNavigationService.closeBlade($scope.blade, function () {
            pb.showDetailBlade({ memberType: 'Organization' }, 'customer.blades.new-organization.title');
        });
    };

    $scope.addCustomer = function () {
        bladeNavigationService.closeBlade($scope.blade, function () {
            pb.showDetailBlade({}, 'customer.blades.new-customer.title');
        });
    };

    blade.headIcon = pb.headIcon;
    $scope.blade.isLoading = false;
}]);

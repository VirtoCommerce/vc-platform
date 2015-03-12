angular.module('virtoCommerce.customerModule.blades')
.controller('memberAddController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    var pb = $scope.blade.parentBlade;

    $scope.addOrganization = function () {
        bladeNavigationService.closeBlade($scope.blade, function () {
            pb.showOrganizationBlade(null, 'New Organization');
        });
    };

    $scope.addCustomer = function () {
        bladeNavigationService.closeBlade($scope.blade, function () {
            pb.showContactBlade(null, 'New Customer');
        });
    };

    $scope.blade.isLoading = false;
}]);

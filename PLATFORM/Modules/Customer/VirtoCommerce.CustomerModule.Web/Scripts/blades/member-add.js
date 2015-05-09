angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberAddController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var pb = $scope.blade.parentBlade;

    $scope.addOrganization = function () {
        bladeNavigationService.closeBlade($scope.blade, function () {
            pb.showDetailBlade({memberType : 'Organization'}, 'New Organization');
        });
    };

    $scope.addCustomer = function () {
        bladeNavigationService.closeBlade($scope.blade, function () {
            pb.showDetailBlade({}, 'New Customer');
        });
    };

    $scope.blade.isLoading = false;
}]);

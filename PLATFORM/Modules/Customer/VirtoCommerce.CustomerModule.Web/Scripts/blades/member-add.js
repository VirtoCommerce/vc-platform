angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberAddController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.customerModule.memberTypesResolverService', function ($scope, bladeNavigationService, memberTypesResolverService) {
    var blade = $scope.blade;

    $scope.addMember = function (node) {
        bladeNavigationService.closeBlade($scope.blade, function () {
            blade.parentBlade.showDetailBlade({ memberType: node.memberType }, node.newInstanceBladeTitle);
        });
    };

    $scope.memberTypes = memberTypesResolverService.objects;
    blade.headIcon = blade.parentBlade.headIcon;
    blade.isLoading = false;
}]);

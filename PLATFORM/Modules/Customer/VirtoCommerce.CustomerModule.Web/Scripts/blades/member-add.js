angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberAddController', ['$scope', 'virtoCommerce.customerModule.memberTypesResolverService', function ($scope, memberTypesResolverService) {
    var blade = $scope.blade;

    $scope.addMember = function (node) {
        $scope.bladeClose(function () {
            blade.parentBlade.showDetailBlade({ memberType: node.memberType }, node.newInstanceBladeTitle);
        });
    };

    $scope.memberTypes = _.filter(memberTypesResolverService.objects, function (x) {
        return !x.topLevelElementOnly || !blade.parentBlade.currentEntity.id;
    });

    blade.headIcon = blade.parentBlade.headIcon;
    blade.isLoading = false;
}]);
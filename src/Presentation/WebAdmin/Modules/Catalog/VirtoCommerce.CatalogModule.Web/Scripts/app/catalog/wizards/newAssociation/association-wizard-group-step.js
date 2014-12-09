angular.module('catalogModule.wizards.associationWizard.associationGroup', [])
.controller('associationGroupSelectController', ['$scope', function ($scope) {
    $scope.blade.refresh = function () {
        $scope.selectedId = $scope.blade.parentBlade.item.groupId;
        $scope.items = ['Accesories', 'Related Items'];
        $scope.blade.isLoading = false;
    };

    $scope.setSelected = function (data) {
        $scope.selectedId = data;
        $scope.blade.parentBlade.item.groupId = data;
        //$scope.blade.parentBlade.item.catalogName = catalog.name;
        $scope.bladeClose();
    }

    $scope.blade.refresh();
}]);

angular.module('catalogModule.wizards.associationWizard.associationGroup', [])
.controller('associationGroupSelectController', ['$scope', function ($scope) {
    $scope.blade.refresh = function () {
        $scope.selectedId = $scope.blade.parentBlade.groupName;
        $scope.items = ['Accessories', 'Related Items'];
        $scope.blade.isLoading = false;
    };

    $scope.setSelected = function (data) {
        $scope.blade.parentBlade.groupName = data;
        $scope.bladeClose();
    }

    $scope.blade.refresh();
}]);

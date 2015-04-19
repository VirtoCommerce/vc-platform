angular.module('platformWebApp')
.controller('rolePermissionsController', ['$scope', 'dialogService', function ($scope, dialogService) {

    function initializeBlade(data) {
        $scope.blade.data = data;
        $scope.blade.promise.then(function (promiseData) {
            _.each(promiseData, function (x) {
                x.isChecked = _.some(data.permissions, function (curr) { return curr.id === x.id; });
            });

            $scope.blade.currentEntities = promiseData;
            $scope.blade.isLoading = false;
        });
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return true;
    }

    $scope.saveChanges = function () {
        $scope.blade.data.permissions = _.where($scope.blade.currentEntities, { isChecked: true });

        $scope.bladeClose();
    };

    $scope.bladeHeadIco = 'fa-lock';

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);
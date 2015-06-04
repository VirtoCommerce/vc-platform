angular.module('platformWebApp')
.controller('platformWebApp.rolePermissionsController', ['$scope', 'platformWebApp.dialogService', function ($scope, dialogService) {

    function initializeBlade(data) {
        $scope.blade.data = data;
        $scope.blade.promise.then(function (promiseData) {
            _.each(promiseData, function (x) {
                x.isChecked = _.some(data.permissions, function (curr) { return curr.id === x.id; });
            });

            $scope.blade.currentEntities = _.groupBy(promiseData, 'groupName');
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
        var values = _.flatten(_.values($scope.blade.currentEntities), true);
        $scope.blade.data.permissions = _.where(values, { isChecked: true });

        $scope.bladeClose();
    };

    $scope.blade.headIcon = 'fa-lock';

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);
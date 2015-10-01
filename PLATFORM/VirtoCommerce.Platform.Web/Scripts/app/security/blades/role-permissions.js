angular.module('platformWebApp')
.controller('platformWebApp.rolePermissionsController', ['$scope', 'platformWebApp.dialogService', function ($scope, dialogService) {
    var blade = $scope.blade;
    var allPermissions;

    function initializeBlade(data) {
        blade.data = data;
        blade.promise.then(function (promiseData) {
            allPermissions = _.filter(angular.copy(promiseData), function (x) {
                return _.all(data.permissions, function (curr) { return curr.id !== x.id; });
            });
            
            blade.currentEntities = _.groupBy(allPermissions, 'groupName');
            blade.isLoading = false;
        });
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return _.any(allPermissions, function (x) { return x.$selected; });
    }

    $scope.saveChanges = function () {
        var selection = _.where(allPermissions, { $selected: true })
        _.each(selection, function (x) { x.$selected = false });
        blade.data.permissions = _.union(blade.data.permissions, selection);

        $scope.bladeClose();
    };

    blade.headIcon = 'fa-key';

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);
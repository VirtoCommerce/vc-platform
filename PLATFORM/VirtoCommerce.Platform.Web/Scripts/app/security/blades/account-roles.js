angular.module('platformWebApp')
.controller('platformWebApp.accountRolesController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, dialogService) {
    var blade = $scope.blade;

    function initializeBlade(data) {
        blade.data = data;
        blade.promise.then(function (promiseData) {
            var allRoles = angular.copy(promiseData.roles);
            blade.currentEntities = _.filter(allRoles, function (x) {
                return _.all(data.roles, function (curr) { return curr.id !== x.id; });
            });

            blade.isLoading = false;
        });
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    };

    $scope.isValid = function () {
        return _.any(blade.currentEntities, function (x) { return x.$selected; });
    };

    $scope.saveChanges = function () {
        blade.data.roles = _.union(blade.data.roles, _.where(blade.currentEntities, { $selected: true }));

        $scope.bladeClose();
    };

    blade.headIcon = 'fa-key';

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);
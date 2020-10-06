angular.module('platformWebApp')
    .controller('platformWebApp.accountRolesController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.roles', function ($scope, bladeNavigationService, dialogService, roles) {
        var blade = $scope.blade;
        blade.headIcon = 'fa-key';

        function initializeBlade(data) {
            blade.data = data;
            roles.search({ take: 10000 }, function (result) {
                var allRoles = angular.copy(result.results);
                blade.currentEntities = _.filter(allRoles, function (x) {
                    return _.all(data.roles, function (curr) { return curr.id !== x.id; });
                });
                blade.isLoading = false;
            });
        }

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

        $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

        // on load:
        // $scope.$watch('blade.parentBlade.currentEntity' gets fired
    }]);

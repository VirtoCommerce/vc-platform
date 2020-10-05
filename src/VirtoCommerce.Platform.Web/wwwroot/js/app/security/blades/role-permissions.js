angular.module('platformWebApp')
    .controller('platformWebApp.rolePermissionsController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.roles', function ($scope, dialogService, roles) {
        var blade = $scope.blade;
        blade.headIcon = 'fa-key';
        var allPermissions;

        function initializeBlade(data) {
            blade.data = data;
            roles.queryPermissions({ take: 10000 }, function (result) {
                allPermissions = _.filter(angular.copy(result), function (x) {
                    return _.all(data.permissions, function (curr) { return curr.name !== x.name; });
                });

                blade.currentEntities = _.groupBy(allPermissions, 'groupName');
                blade.isLoading = false;
            });
        }

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

        $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);
    }]);

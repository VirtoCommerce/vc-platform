angular.module('platformWebApp')
    .controller('platformWebApp.rolePermissionsController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.roles', function ($scope, dialogService, roles) {
        var blade = $scope.blade;
        blade.headIcon = 'fas fa-key';
        var allPermissions;

        function initializeBlade(data) {
            blade.data = data;
            roles.queryPermissions({ take: 10000 }, (result) => {
                allPermissions = _.filter(angular.copy(result), (x) => {
                    return _.all(data.permissions, (curr) => curr.name !== x.name );
                });

                blade.currentEntities = _.groupBy(allPermissions, 'groupName');

                blade.currentEntities = Object.keys(blade.currentEntities).sort().reduce(
                    (obj, key) => { 
                        obj[key] = blade.currentEntities[key]; 
                        return obj;
                    }, 
                    {}
                  );

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

        blade.toolbarCommands = [
            {
                name: "platform.commands.confirm",
                icon: 'fas fa-check',
                executeMethod: $scope.saveChanges,
                canExecuteMethod: () => $scope.isValid(),
            },
            {
                name: "platform.commands.cancel",
                icon: 'fas fa-times',
                executeMethod: $scope.cancelChanges,
                canExecuteMethod: () => true,
            }];
        
    }]);

angular.module('virtoCommerce.coreModule.settings.blades')
.controller('settingsDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'settings', function ($scope, dialogService, bladeNavigationService, settings) {
    $scope.settingGroups = [{ name: '-' }, { name: 'Advanced' }];

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        settings.getSettings({ moduleId: $scope.blade.moduleId }, function (results) {
            // $scope.blade.objectsGrouped = _.groupBy(results, 'groupName');

            $scope.blade.objects = angular.copy(results);
            $scope.blade.origEntity = results;
            $scope.blade.isLoading = false;
        },
        function (error) {
            $scope.blade.isLoading = false;
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    function isDirty() {
        return !angular.equals($scope.blade.objects, $scope.blade.origEntity);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        settings.update({}, $scope.blade.objects, function (data, headers) {
            $scope.blade.refresh();
        });
    };

    $scope.bladeToolbarCommands = [
        {
            name: "Save", icon: 'icon-floppy',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Reset", icon: 'icon-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.objects);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        }
    ];

    $scope.blade.onClose = function (closeCallback) {
        closeCallback();
    };

    // actions on load
    $scope.blade.refresh();
}]);

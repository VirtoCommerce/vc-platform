angular.module('platformWebApp')
.controller('platformWebApp.exportImport.exportMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.exportImport.resource', 'platformWebApp.authService', function ($scope, bladeNavigationService, exportImportResourse, authService) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-upload';
    blade.title = 'platform.blades.export-main.title';

    $scope.exportRequest = {};

    function initializeBlade() {
        exportImportResourse.getNewExportManifest(function (data) {
            $scope.exportRequest.exportManifest = data;
            blade.isLoading = false;
        });
    };

    $scope.$on("new-notification-event", function (event, notification) {
        if (blade.notification && notification.id == blade.notification.id) {
            angular.copy(notification, blade.notification);
            if (notification.errorCount > 0) {
                bladeNavigationService.setError('Export error', blade);
            }
        }
    });

    $scope.canStartProcess = function () {
        return authService.checkPermission('platform:exportImport:export') && (_.any($scope.exportRequest.modules) || $scope.exportRequest.handleSecurity || $scope.exportRequest.handleSettings);
    }

    $scope.updateModuleSelection = function () {
        var selection = _.where($scope.exportRequest.exportManifest.modules, { isChecked: true });
        $scope.exportRequest.modules = _.pluck(selection, 'id');
    };

    $scope.startExport = function () {
        blade.isLoading = true;
        exportImportResourse.runExport($scope.exportRequest,
            function (data) { blade.notification = data; blade.isLoading = false; });
    }

    blade.toolbarCommands = [
		{
		    name: "platform.commands.select-all", icon: 'fa fa-check-square-o',
		    executeMethod: function () { selectAll(true) },
		    canExecuteMethod: function () { return $scope.exportRequest.exportManifest && !blade.notification; }
		},
        {
            name: "platform.commands.unselect-all", icon: 'fa fa-square-o',
            executeMethod: function () { selectAll(false) },
            canExecuteMethod: function () { return $scope.exportRequest.exportManifest && !blade.notification; }
        }
    ];

    var selectAll = function (action) {
        $scope.exportRequest.handleSecurity = action;
        $scope.exportRequest.handleBinaryData = action;
        $scope.exportRequest.handleSettings = action;
        _.forEach($scope.exportRequest.exportManifest.modules, function (module) { module.isChecked = action; });

        $scope.updateModuleSelection();
    }

    initializeBlade();
}]);

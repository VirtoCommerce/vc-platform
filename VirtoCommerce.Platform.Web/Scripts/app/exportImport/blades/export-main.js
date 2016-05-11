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
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    //$scope.setForm = function (form) {
    //    $scope.formScope = form;
    //}

    $scope.$on("new-notification-event", function (event, notification) {
        if (blade.notification && notification.id == blade.notification.id) {
            angular.copy(notification, blade.notification);
            if (notification.errorCount > 0) {
                bladeNavigationService.setError('Export error', blade);
            }
        }
    });

    $scope.canStartProcess = function () {
    	return authService.checkPermission('platform:exportImport:export') && (($scope.exportRequest.modules && $scope.exportRequest.modules.length > 0) || $scope.exportRequest.handleSecurity || $scope.exportRequest.handleSettings);
    }

    $scope.updateModuleSelection = function () {
        var selection = _.where($scope.exportRequest.exportManifest.modules, { isChecked: true });
        $scope.exportRequest.modules = _.pluck(selection, 'id');
    };

    $scope.startExport = function () {
        blade.isLoading = true;
        $scope.updateModuleSelection();
        exportImportResourse.runExport($scope.exportRequest,
        function (data) { blade.notification = data; blade.isLoading = false; },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    $scope.blade.toolbarCommands = [
		{
		    name: "platform.commands.select-all", icon: 'fa fa-check-square-o',
		    executeMethod: function () { selectAll(true) },
		    canExecuteMethod: function () {
		        return true;
		    }
		},
    {
        name: "platform.commands.unselect-all", icon: 'fa fa-square-o',
        executeMethod: function () { selectAll(false) },
        canExecuteMethod: function () {
            return true;
        }
    }
    ];

    var selectAll = function (action) {
        if ($scope.exportRequest.exportManifest && $scope.exportRequest.exportManifest.modules) {
            _.forEach($scope.exportRequest.exportManifest.modules, function (module) { module.isChecked = action; });
            $scope.exportRequest.handleSecurity = action;
            $scope.exportRequest.handleBinaryData = action;
            $scope.exportRequest.handleSettings = action;
        }
    }

    initializeBlade();
}]);

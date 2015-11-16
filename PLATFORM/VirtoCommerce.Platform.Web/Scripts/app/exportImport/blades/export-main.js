angular.module('platformWebApp')
.controller('platformWebApp.exportImport.exportMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.exportImport.resource', function ($scope, bladeNavigationService, exportImportResourse) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-upload';
    blade.title = 'platform.export-main.title';

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
    	return ($scope.exportRequest.modules && $scope.exportRequest.modules.length > 0) || $scope.exportRequest.handleSecurity || $scope.exportRequest.handleSettings;
    }

    $scope.updateModuleSelection = function () {
    	var selection = _.where($scope.exportRequest.exportManifest.modules, { isChecked: true });
    	$scope.exportRequest.modules = _.pluck(selection, 'id');
    };

    $scope.startExport = function () {
    	blade.isLoading = true;

    	exportImportResourse.runExport($scope.exportRequest,
        function (data) { blade.notification = data; blade.isLoading = false; },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }


    initializeBlade();
}]);

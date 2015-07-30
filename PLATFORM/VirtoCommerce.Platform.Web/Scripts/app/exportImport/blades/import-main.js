angular.module('platformWebApp')
.controller('platformWebApp.exportImport.importMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.exportImport.resource', 'FileUploader', function ($scope, bladeNavigationService, exportImportResourse, FileUploader) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-download';
    blade.title = 'Data import';
    blade.isLoading = false;

    $scope.importRequest = {};

    $scope.$on("new-notification-event", function (event, notification) {
        if (blade.notification && notification.id == blade.notification.id) {
        	angular.copy(notification, blade.notification);
        	if(notification.errorCount > 0)
        	{
        		bladeNavigationService.setError('Import error', blade);
        	}
        }
    });

    $scope.canStartProcess = function () {
    	return ($scope.importRequest.modules && $scope.importRequest.modules.length > 0) || $scope.importRequest.handleSecurity || $scope.importRequest.handleSettings;
    }

    $scope.startProcess = function () {
        blade.isLoading = true;
        exportImportResourse.runImport($scope.importRequest,
        function (data) { blade.notification = data; blade.isLoading = false; },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    $scope.updateModuleSelection = function () {
    	var selection = _.where(blade.currentEntities, { isChecked: true });
    	$scope.importRequest.modules = _.pluck(selection, 'id');
    };

    if (!$scope.uploader) {
        function endsWith(str, suffix) {
            return str.indexOf(suffix, str.length - suffix.length) !== -1;
        }

        // Creates an uploader
        var uploader = $scope.uploader = new FileUploader({
            scope: $scope,
            headers: { Accept: 'application/json' },
            url: 'api/assets',
            method: 'POST',
            autoUpload: true,
            removeAfterUpload: true
        });
        
        // ADDING FILTERS
        // zip only
        uploader.filters.push({
            name: 'zipFilter',
            fn: function (i /*{File|FileLikeObject}*/, options) {
                return endsWith(i.name.toLowerCase(), '.zip');
            }
        });

        uploader.onBeforeUploadItem = function (fileItem) {
            blade.isLoading = true;
        };

        uploader.onSuccessItem = function (fileItem, asset, status, headers) {
        	$scope.importRequest.fileUrl = asset[0].relativeUrl;

        	exportImportResourse.getImportInfo({ fileUrl: $scope.importRequest.fileUrl }, function (data) {
	            console.log(data);
        	    blade.info = data.exportManifest;
        	    blade.currentEntities = data.modules;
        	    blade.isHasSecurity = data.exportManifest.isHasSecurity;
        	    blade.isHasSettings = data.exportManifest.isHasSettings;
        	    blade.isLoading = false;
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };
    }
}]);

angular.module('platformWebApp')
.controller('platformWebApp.exportImport.importMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.exportImport.resource', 'FileUploader', function ($scope, bladeNavigationService, exportImportResourse, FileUploader) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:exportImport:import';
    blade.headIcon = 'fa-download';
    blade.title = 'platform.blades.import-main.title';
    blade.isLoading = false;
    $scope.importRequest = {};
    var origManifest;

    $scope.$on("new-notification-event", function (event, notification) {
        if (blade.notification && notification.id == blade.notification.id) {
            angular.copy(notification, blade.notification);
            if (notification.errorCount > 0) {
                bladeNavigationService.setError('Import error', blade);
            }
        }
    });

    $scope.canStartProcess = function () {
        return blade.hasUpdatePermission() && (_.any($scope.importRequest.modules) || $scope.importRequest.handleSecurity || $scope.importRequest.handleSettings || $scope.importRequest.handleBinaryData);
    }

    $scope.startProcess = function () {
        blade.isLoading = true;
        exportImportResourse.runImport($scope.importRequest,
            function (data) { blade.notification = data; blade.isLoading = false; },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });

        blade.toolbarCommands.splice(0, 2, commandCancel);
    }

    var commandCancel = {
        name: 'platform.commands.cancel',
        icon: 'fa fa-times',
        canExecuteMethod: function () {
            return blade.notification && !blade.notification.finished;
        },
        executeMethod: function () {
            exportImportResourse.taskCancel({ jobId: blade.notification.jobId }, null, function (data) {
            });
        }
    };

    $scope.updateModuleSelection = function () {
        var selection = _.where($scope.importRequest.exportManifest.modules, { isChecked: true });
        $scope.importRequest.modules = _.pluck(selection, 'id');
    };

    if (!$scope.uploader) {
        // create the uploader
        var uploader = $scope.uploader = new FileUploader({
            scope: $scope,
            url: 'api/platform/assets/localstorage',
            method: 'POST',
            autoUpload: true,
            removeAfterUpload: true
        });

        // ADDING FILTERS
        // zip only
        uploader.filters.push({
            name: 'zipFilter',
            fn: function (i /*{File|FileLikeObject}*/, options) {
                return i.name.toLowerCase().endsWith('.zip');
            }
        });

        uploader.onBeforeUploadItem = function (fileItem) {
            blade.isLoading = true;
            bladeNavigationService.setError(null, blade);
        };

        uploader.onErrorItem = function (item, response, status, headers) {
            bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
        };

        uploader.onSuccessItem = function (fileItem, asset, status, headers) {
            $scope.importRequest.fileUrl = asset[0].url;
            $scope.importRequest.fileName = asset[0].name;

            exportImportResourse.loadExportManifest({ fileUrl: $scope.importRequest.fileUrl }, function (data) {
                origManifest = angular.copy(data);

                // select all available data for import
                $scope.importRequest.handleSecurity = data.handleSecurity;
                $scope.importRequest.handleSettings = data.handleSettings;
                $scope.importRequest.handleBinaryData = data.handleBinaryData;

                _.each(data.modules, function (x) { x.isChecked = true; });

                $scope.importRequest.exportManifest = data;
                $scope.updateModuleSelection();
                blade.isLoading = false;
            });
        };
    }


    blade.toolbarCommands = [
        {
            name: "platform.commands.select-all", icon: 'fa fa-check-square-o',
            executeMethod: function () { selectAll(true) },
            canExecuteMethod: function () { return $scope.importRequest.exportManifest && !blade.notification; }
        },
        {
            name: "platform.commands.unselect-all", icon: 'fa fa-square-o',
            executeMethod: function () { selectAll(false) },
            canExecuteMethod: function () { return $scope.importRequest.exportManifest && !blade.notification; }
        }
    ];

    var selectAll = function (action) {
        if (origManifest.handleSecurity)
            $scope.importRequest.handleSecurity = action;
        if (origManifest.handleBinaryData)
            $scope.importRequest.handleBinaryData = action;
        if (origManifest.handleSettings)
            $scope.importRequest.handleSettings = action;

        _.forEach($scope.importRequest.exportManifest.modules, function (module) { module.isChecked = action; });

        $scope.updateModuleSelection();
    }
}]);

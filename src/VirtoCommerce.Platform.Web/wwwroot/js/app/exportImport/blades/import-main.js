angular.module('platformWebApp')
    .controller('platformWebApp.exportImport.importMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.exportImport.resource', 'FileUploader', function ($scope, bladeNavigationService, exportImportResourse, FileUploader) {
        var blade = $scope.blade;
        blade.updatePermission = 'platform:exportImport:import';
        blade.headIcon = 'fa fa-download';
        blade.title = 'platform.blades.import-main.title';
        blade.isLoading = false;
        $scope.importRequest = {};

        $scope.$on("new-notification-event", function (event, notification) {
            if (notification.jobId && notification.finished) {
                $scope.switchCommandButton("close");
            }
            if (blade.notification && notification.id === blade.notification.id) {
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

            exportImportResourse.runImport($scope.importRequest, function (data) {
                blade.notification = data;
                blade.isLoading = false;
            });

            $scope.switchCommandButton("cancel");
        }

        $scope.switchCommandButton = function (state) {
            const stateCommandIndex = blade.toolbarCommands.findIndex(x => x.target === 'import');

            if (state === "cancel") {
                blade.toolbarCommands[stateCommandIndex] = commandCancel;
            } else if (state === "close") {
                blade.toolbarCommands[stateCommandIndex] = commandClose;
            }
        }

        var commandCancel = {
            name: 'platform.commands.cancel',
            icon: 'fa fa-times',
            canExecuteMethod: function () {
                return blade.notification && !blade.notification.finished;
            },
            executeMethod: function () {
                exportImportResourse.taskCancel({ jobId: blade.notification.jobId }, null, null);
            },
            target: 'import'
        };

        var commandClose = {
            name: 'Close',
            icon: 'fa fa-times',
            canExecuteMethod: function () { return blade.error || blade.notification && blade.notification.finished; },
            executeMethod: function () { bladeNavigationService.closeBlade(blade); },
            target: 'import'
        };

        $scope.updateModuleSelection = function () {
            var selection = _.where($scope.importRequest.exportManifest.modules, { isChecked: true });
            $scope.importRequest.modules = _.pluck(selection, 'id');
        };

        if (!$scope.uploader) {
            // create the uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                url: 'api/assets/localstorage',
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
                bladeNavigationService.setError(null, blade);
            };

            uploader.onErrorItem = function (item, response, status, headers) {
                bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
            };

            uploader.onSuccessItem = function (fileItem, asset, status, headers) {
                $scope.importRequest.fileUrl = asset[0].url;
                $scope.importRequest.fileName = asset[0].name;

                exportImportResourse.loadExportManifest({ fileUrl: $scope.importRequest.fileUrl }, function (data) {
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
                name: "platform.blades.import-main.labels.start-import", icon: 'fa fa-download',
                executeMethod: () => $scope.startProcess(),
                canExecuteMethod: () => $scope.canStartProcess() && !blade.notification,
                target: 'import'
            },
            {
                name: "platform.commands.select-all", icon: 'far fa-check-square',
                executeMethod: () => selectAll(true),
                canExecuteMethod: () => $scope.importRequest.exportManifest && !blade.notification
            },
            {
                name: "platform.commands.unselect-all", icon: 'far fa-square',
                executeMethod: () => selectAll(false),
                canExecuteMethod: () => $scope.importRequest.exportManifest && !blade.notification && $scope.canStartProcess()
            }
        ];

        var selectAll = function (action) {
            $scope.importRequest.handleSecurity = $scope.importRequest.exportManifest.handleSecurity && action;
            $scope.importRequest.handleBinaryData = $scope.importRequest.exportManifest.handleBinaryData && action;
            $scope.importRequest.handleSettings = $scope.importRequest.exportManifest.handleSettings && action;
            _.forEach($scope.importRequest.exportManifest.modules, (module) => module.isChecked = action);

            $scope.updateModuleSelection();
        }
    }]);

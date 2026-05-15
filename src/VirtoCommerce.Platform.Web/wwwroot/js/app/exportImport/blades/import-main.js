angular.module('platformWebApp')
    .controller('platformWebApp.exportImport.importMainController', [
        '$scope',
        'platformWebApp.bladeNavigationService',
        'platformWebApp.exportImport.resource',
        'platformWebApp.authService',
        'platformWebApp.exportImport.progressService',
        'FileUploader',
        function ($scope, bladeNavigationService, exportImportResourse, authService, progressService, FileUploader) {
        var blade = $scope.blade;
        blade.updatePermission = 'platform:import';
        blade.headIcon = 'fa fa-download';
        blade.title = 'platform.blades.import-main.title';
        blade.isLoading = false;
        $scope.importRequest = {};
        // Surfaced in the sensitive-data warning banner so the admin sees exactly which account
        // the backend will preserve during the user-import phase. Matches PlatformExportManifest.CallerUserName.
        $scope.currentUserName = authService.userName;
        // Wire up shared progress UI helpers (copy*, progressItems/Stats, detailed-log toggle).
        progressService.attach($scope, blade, 'import');

        $scope.passwordError = false;
        $scope.clearPasswordError = function () { $scope.passwordError = false; };

        $scope.$on("new-notification-event", function (event, notification) {
            if (!blade.notification || notification.id !== blade.notification.id) {
                return;
            }

            // Intercept wrong-password failures BEFORE the generic "Import error" path: those
            // are recoverable (the file uploaded fine, only decrypt failed), so we keep the
            // user in the password-form state instead of flipping into the "Upload failed"
            // / blade.error UI which would otherwise duplicate the file row on the blade.
            if (notification.finished && notification.errors && notification.errors.length > 0) {
                var hasPasswordError = _.any(notification.errors, function (e) {
                    return typeof e === 'string' && e.toLowerCase().indexOf('invalid backup password') !== -1;
                });
                if (hasPasswordError) {
                    $scope.passwordError = true;
                    blade.notification = null;
                    bladeNavigationService.setError(null, blade);
                    $scope.switchCommandButton("start");
                    blade.isLoading = false;
                    // Drop the wrong password from the request so it doesn't get re-sent
                    // unmodified on a quick second click — and so a stale value isn't kept
                    // around in scope longer than it has to be.
                    $scope.importRequest.password = '';
                    return;
                }
            }

            if (notification.jobId && notification.finished) {
                $scope.switchCommandButton("close");
            }
            angular.copy(notification, blade.notification);
            progressService.parseProgressLog($scope, blade);
            if (notification.errorCount > 0) {
                bladeNavigationService.setError('Import error', blade);
            }
        });

        $scope.canStartProcess = function () {
            var hasAnySection = _.any($scope.importRequest.modules) || $scope.importRequest.handleSecurity || $scope.importRequest.handleSettings || $scope.importRequest.handleBinaryData || $scope.importRequest.handleDynamicProperties;
            // Disable the start button until a password is entered for encrypted backups —
            // otherwise the request would fail at the first decrypt and the admin would have
            // to retry. Catching it pre-submit is cheaper and clearer.
            var hasRequiredPassword = !($scope.importRequest.exportManifest && $scope.importRequest.exportManifest.isEncrypted)
                || !!$scope.importRequest.password;
            return blade.hasUpdatePermission() && hasAnySection && hasRequiredPassword;
        }

        $scope.startProcess = function () {
            blade.isLoading = true;
            $scope.passwordError = false;

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
            } else if (state === "start") {
                blade.toolbarCommands[stateCommandIndex] = commandStart;
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
                    $scope.importRequest.handleDynamicProperties = data.handleDynamicProperties;

                    _.each(data.modules, function (x) { x.isChecked = true; });

                    $scope.importRequest.exportManifest = data;
                    $scope.updateModuleSelection();
                    blade.isLoading = false;
                });
            };
        }

        var commandStart = {
            name: "platform.blades.import-main.labels.start-import", icon: 'fa fa-download',
            executeMethod: () => $scope.startProcess(),
            canExecuteMethod: () => $scope.canStartProcess() && !blade.notification,
            target: 'import'
        };

        blade.toolbarCommands = [
            commandStart,
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
            $scope.importRequest.handleDynamicProperties = $scope.importRequest.exportManifest.handleDynamicProperties && action;

            _.forEach($scope.importRequest.exportManifest.modules, (module) => module.isChecked = action);

            $scope.updateModuleSelection();
        }
    }]);

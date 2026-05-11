angular.module('platformWebApp')
    .controller('platformWebApp.exportImport.importMainController', [
        '$scope',
        'platformWebApp.bladeNavigationService',
        'platformWebApp.exportImport.resource',
        'platformWebApp.authService',
        'FileUploader',
        function ($scope, bladeNavigationService, exportImportResourse, authService, FileUploader) {
        var blade = $scope.blade;
        blade.updatePermission = 'platform:import';
        blade.headIcon = 'fa fa-download';
        blade.title = 'platform.blades.import-main.title';
        blade.isLoading = false;
        $scope.importRequest = {};
        // Surfaced in the sensitive-data warning banner so the admin sees exactly which account
        // the backend will preserve during the user-import phase. Matches PlatformExportManifest.CallerUserName.
        $scope.currentUserName = authService.userName;
        $scope.progressItems = [];
        $scope.progressStats = { total: 0, completed: 0, percent: 0 };
        $scope.showDetailedLog = false;
        $scope.toggleDetailedLog = function () { $scope.showDetailedLog = !$scope.showDetailedLog; };

        $scope.copyToClipboard = function (text, $event) {
            if ($event) { $event.stopPropagation(); }
            if (!text) { return; }
            var done = function () { /* visual feedback could go here */ };
            if (navigator.clipboard && navigator.clipboard.writeText) {
                navigator.clipboard.writeText(text).then(done, done);
            } else {
                var ta = document.createElement('textarea');
                ta.value = text;
                ta.style.position = 'fixed';
                ta.style.opacity = '0';
                document.body.appendChild(ta);
                ta.select();
                try { document.execCommand('copy'); } catch (e) { /* ignore */ }
                document.body.removeChild(ta);
                done();
            }
        };

        $scope.copyItemErrors = function (item, $event) {
            var lines = _.pluck(item.messages || [], 'message');
            $scope.copyToClipboard((item.name ? item.name + '\n' : '') + lines.join('\n'), $event);
        };

        $scope.copyDetailedLog = function ($event) {
            var entries = (blade.notification && blade.notification.progressLog) || [];
            var lines = _.map(entries, function (e) { return `[${e.level || 'Info'}] ${e.message || ''}`; });
            $scope.copyToClipboard(lines.join('\n'), $event);
        };

        function parseProgressLog(progressLog) {
            var items = [];
            var currentItem = null;

            _.each(progressLog, function (entry) {
                var msg = entry.message || '';
                var startMatch = msg.match(/^Importing '(.+?)'/);

                if (startMatch) {
                    if (currentItem && currentItem.status === 'active') {
                        currentItem.status = 'done';
                    }
                    currentItem = {
                        id: items.length,
                        name: startMatch[1],
                        status: 'active',
                        messages: []
                    };
                    items.push(currentItem);
                } else if (entry.level === 'Error' && currentItem) {
                    currentItem.status = 'error';
                    currentItem.messages.push(entry);
                } else if (currentItem && currentItem.status === 'active') {
                    if (msg.match(/^Successfully imported '(.+?)'/)) {
                        currentItem.status = 'done';
                    } else if (msg.match(/^Failed to import '(.+?)'/)) {
                        currentItem.status = 'error';
                    }
                    currentItem.messages.push(entry);
                }
            });

            // Fallback: if the backend dumped errors only into the legacy `errors` collection
            // (e.g. a module reported them out-of-band), retro-attach them to the matching item
            // so the failed item still shows red even without an Error-level progressLog entry.
            var legacyErrors = (blade.notification && blade.notification.errors) || [];
            _.each(legacyErrors, function (errorText) {
                var match = _.find(items, function (i) {
                    return errorText && errorText.indexOf(i.name) !== -1;
                });
                if (match) {
                    match.status = 'error';
                    if (!_.any(match.messages, function (m) { return m.message === errorText; })) {
                        match.messages.push({ level: 'Error', message: errorText });
                    }
                }
            });

            // If errorCount > 0 but no item was flagged, mark the last finished item as error
            // so the user at least gets a visible breadcrumb (better than all-green-but-banner-says-errors).
            if (blade.notification && blade.notification.errorCount > 0 && !_.any(items, function (i) { return i.status === 'error'; }) && items.length > 0) {
                var last = items[items.length - 1];
                last.status = 'error';
                _.each(legacyErrors, function (e) {
                    last.messages.push({ level: 'Error', message: e });
                });
            }

            var completed = _.filter(items, function (i) { return i.status === 'done' || i.status === 'error'; }).length;
            var total = (blade.notification && blade.notification.totalCount) || items.length;

            $scope.progressItems = items;
            $scope.progressStats = {
                total: total,
                completed: completed,
                percent: total > 0 ? Math.round((completed / total) * 100) : 0
            };
        }

        $scope.$on("new-notification-event", function (event, notification) {
            if (notification.jobId && notification.finished) {
                $scope.switchCommandButton("close");
            }
            if (blade.notification && notification.id === blade.notification.id) {
                angular.copy(notification, blade.notification);
                parseProgressLog(blade.notification.progressLog);
                if (notification.errorCount > 0) {
                    bladeNavigationService.setError('Import error', blade);
                }
            }
        });

        $scope.canStartProcess = function () {
            return blade.hasUpdatePermission() && (_.any($scope.importRequest.modules) || $scope.importRequest.handleSecurity || $scope.importRequest.handleSettings || $scope.importRequest.handleBinaryData || $scope.importRequest.handleDynamicProperties);
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
                    $scope.importRequest.handleDynamicProperties = data.handleDynamicProperties;

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
            $scope.importRequest.handleDynamicProperties = $scope.importRequest.exportManifest.handleDynamicProperties && action;

            _.forEach($scope.importRequest.exportManifest.modules, (module) => module.isChecked = action);

            $scope.updateModuleSelection();
        }
    }]);

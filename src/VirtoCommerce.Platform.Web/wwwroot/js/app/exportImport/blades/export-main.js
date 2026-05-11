angular.module('platformWebApp')
    .controller('platformWebApp.exportImport.exportMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.exportImport.resource', 'platformWebApp.authService', 'platformWebApp.toolbarService', function ($scope, bladeNavigationService, exportImportResourse, authService, toolbarService) {
        var blade = $scope.blade;
        blade.headIcon = 'fa fa-upload';
        blade.title = 'platform.blades.export-main.title';

        $scope.exportRequest = {};
        $scope.progressItems = [];
        $scope.progressStats = { total: 0, completed: 0, percent: 0 };
        $scope.showDetailedLog = false;
        $scope.toggleDetailedLog = function () { $scope.showDetailedLog = !$scope.showDetailedLog; };

        $scope.copyToClipboard = function (text, $event) {
            if ($event) { $event.stopPropagation(); }
            if (!text) { return; }
            var done = function () { };
            if (navigator.clipboard && navigator.clipboard.writeText) {
                navigator.clipboard.writeText(text).then(done, done);
            } else {
                var ta = document.createElement('textarea');
                ta.value = text;
                ta.style.position = 'fixed';
                ta.style.opacity = '0';
                document.body.appendChild(ta);
                ta.select();
                try { document.execCommand('copy'); } catch (e) { }
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
            var lines = _.map(entries, function (e) { return '[' + (e.level || 'Info') + '] ' + (e.message || ''); });
            $scope.copyToClipboard(lines.join('\n'), $event);
        };

        function initializeBlade() {
            exportImportResourse.getNewExportManifest(function (data) {
                $scope.exportRequest.exportManifest = data;
                blade.isLoading = false;
            });
        }

        function parseProgressLog(progressLog) {
            var items = [];
            var currentItem = null;

            _.each(progressLog, function (entry) {
                var msg = entry.message || '';
                var startMatch = msg.match(/^Exporting '(.+?)'/);

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
                    if (msg.match(/^Successfully exported '(.+?)'/)) {
                        currentItem.status = 'done';
                    } else if (msg.match(/^Failed to export '(.+?)'/)) {
                        currentItem.status = 'error';
                    }
                    currentItem.messages.push(entry);
                }
            });

            // Fallback: retro-attach legacy `errors` to the matching item by name match.
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
            if (blade.notification && notification.id == blade.notification.id) {
                angular.copy(notification, blade.notification);
                parseProgressLog(blade.notification.progressLog);
                if (notification.errorCount > 0) {
                    bladeNavigationService.setError('Export error', blade);
                }
            }
        });

        $scope.canStartProcess = function () {
            return authService.checkPermission('platform:export') && (_.any($scope.exportRequest.modules) || $scope.exportRequest.handleSecurity || $scope.exportRequest.handleSettings || $scope.exportRequest.handleDynamicProperties);
        }

        $scope.updateModuleSelection = function () {
            var selection = _.where($scope.exportRequest.exportManifest.modules, { isChecked: true });
            $scope.exportRequest.modules = _.pluck(selection, 'id');
        };

        var startExport = function () {
            blade.isLoading = true;
            exportImportResourse.runExport($scope.exportRequest,
                function (data) {
                    blade.notification = data;
                    blade.isLoading = false;
                });

            blade.toolbarCommands.splice(0, 2, commandCancel);
        };

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

        blade.toolbarCommands = [
            {
                name: "platform.commands.start-export", icon: 'fa fa-upload',
                executeMethod: () => startExport(),
                canExecuteMethod: () => $scope.canStartProcess() && !blade.notification
            },
            {
                name: "platform.commands.select-all", icon: 'far fa-check-square',
                executeMethod: () => selectAll(true),
                canExecuteMethod: () => $scope.exportRequest.exportManifest && !blade.notification
            },
            {
                name: "platform.commands.unselect-all", icon: 'far fa-square',
                executeMethod: () => selectAll(false),
                canExecuteMethod: () => $scope.exportRequest.exportManifest && !blade.notification && $scope.canStartProcess()
            }
        ];

        var selectAll = function (action) {
            $scope.exportRequest.handleSecurity = action;
            $scope.exportRequest.handleBinaryData = action;
            $scope.exportRequest.handleSettings = action;
            $scope.exportRequest.handleDynamicProperties = action;
            _.forEach($scope.exportRequest.exportManifest.modules, (module) => module.isChecked = action);

            $scope.updateModuleSelection();
        }

        initializeBlade();
    }]);

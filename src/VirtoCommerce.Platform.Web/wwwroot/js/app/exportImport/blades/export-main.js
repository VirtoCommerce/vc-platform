angular.module('platformWebApp')
    .controller('platformWebApp.exportImport.exportMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.exportImport.resource', 'platformWebApp.authService', 'platformWebApp.toolbarService', function ($scope, bladeNavigationService, exportImportResourse, authService, toolbarService) {
        var blade = $scope.blade;
        blade.headIcon = 'fa fa-upload';
        blade.title = 'platform.blades.export-main.title';

        $scope.exportRequest = {};

        function initializeBlade() {
            exportImportResourse.getNewExportManifest(function (data) {
                $scope.exportRequest.exportManifest = data;
                blade.isLoading = false;
            });
        }

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
            _.forEach($scope.exportRequest.exportManifest.modules, (module) => module.isChecked = action);

            $scope.updateModuleSelection();
        }

        initializeBlade();
    }]);

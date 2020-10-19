angular.module('platformWebApp')
    .config(['$stateProvider', function ($stateProvider) {
        $stateProvider.state('workspace.exportImport', {
            url: '/exportImport',
            templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
            controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                var blade = {
                    id: 'exportImport',
                    title: 'platform.blades.exportImport-main.title',
                    controller: 'platformWebApp.exportImport.mainController',
                    template: '$(Platform)/Scripts/app/exportImport/blades/exportImport-main.tpl.html',
                    isClosingDisabled: true
                };
                bladeNavigationService.showBlade(blade);
            }]
        });

        $stateProvider.state('setupWizard.sampleDataInstallation', {
            url: '/sampleDataInstallation',
            templateUrl: '$(Platform)/Scripts/app/exportImport/templates/sampleDataInstallation.tpl.html',
            controller: ['$scope', '$state', '$stateParams', 'platformWebApp.exportImport.resource', 'platformWebApp.setupWizard', function ($scope, $state, $stateParams, exportImportResourse, setupWizard) {
                $scope.notification = {};
                if ($stateParams.notification) {
                    $scope.notification = $stateParams.notification;
                }
                $scope.sampleDataInfos = {};
                //that's needed when state directly opened by url or push notification
                var step = setupWizard.findStepByState($state.current.name);

                $scope.close = function () {
                    setupWizard.showStep(step.nextStep);
                };

                //copy notification to scope
                $scope.$on("new-notification-event", function (event, notification) {
                    if (notification.notifyType == 'SampleDataImportPushNotification') {
                        angular.copy(notification, $scope.notification);
                        if (notification.finished && notification.errorCount == 0) {
                            $scope.close();
                        }
                    }
                });

                $scope.importData = function (sampleData) {
                    if (sampleData.url) {
                        exportImportResourse.importSampleData({ url: sampleData.url }, function (data) {
                            //need check notification.created because not exist any way to check empty response
                            if (data && data.created) {
                                angular.copy(data, $scope.notification);
                            }
                            else {
                                setupWizard.showStep(step.nextStep);
                            }
                        }, function (error) { setupWizard.showStep(step.nextStep); });
                    }
                    else {
                        setupWizard.showStep(step.nextStep);
                    }
                }

                function discoverSampleData() {
                    $scope.loading = true;
                    exportImportResourse.sampleDataDiscover({}, function (sampleDataInfos) {
                        $scope.loading = false;
                        //run obvious sample data installation
                        if (angular.isArray(sampleDataInfos) && sampleDataInfos.length > 0) {
                            if (sampleDataInfos.length > 1) {
                                $scope.sampleDataInfos = sampleDataInfos;
                            }
                            else {
                                $scope.importData(sampleDataInfos[0]);
                            }
                        }
                        else {
                            //nothing to import - skip step
                            setupWizard.showStep(step.nextStep);
                        }
                    });
                }

                discoverSampleData();
            }]
        });
    }])
    .run(
        ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', 'platformWebApp.pushNotificationTemplateResolver', 'platformWebApp.bladeNavigationService', 'platformWebApp.exportImport.resource', 'platformWebApp.setupWizard', function ($rootScope, mainMenuService, widgetService, $state, pushNotificationTemplateResolver, bladeNavigationService, exportImportResourse, setupWizard) {
            var menuItem = {
                path: 'configuration/exportImport',
                icon: 'fa fa-database',
                title: 'platform.menu.export-import',
                priority: 10,
                action: function () { $state.go('workspace.exportImport'); },
                permission: 'platform:exportImport:access'
            };
            mainMenuService.addMenuItem(menuItem);

            //Push notifications
            var menuExportImportTemplate =
            {
                priority: 900,
                satisfy: function (notify, place) { return place == 'header-notification' && (notify.notifyType == 'PlatformExportPushNotification' || notify.notifyType == 'PlatformImportPushNotification'); },
                template: '$(Platform)/Scripts/app/exportImport/notifications/headerNotification.tpl.html',
                action: function (notify) { $state.go('workspace.pushNotificationsHistory', notify) }
            };
            pushNotificationTemplateResolver.register(menuExportImportTemplate);

            var historyExportImportTemplate =
            {
                priority: 900,
                satisfy: function (notify, place) { return place == 'history' && (notify.notifyType == 'PlatformExportPushNotification' || notify.notifyType == 'PlatformImportPushNotification'); },
                template: '$(Platform)/Scripts/app/exportImport/notifications/history.tpl.html',
                action: function (notify) {
                    var isExport = notify.notifyType == 'PlatformExportPushNotification';
                    var blade = {
                        id: 'platformExportImport',
                        controller: isExport ? 'platformWebApp.exportImport.exportMainController' : 'platformWebApp.exportImport.importMainController',
                        template: isExport ? '$(Platform)/Scripts/app/exportImport/blades/export-main.tpl.html' : '$(Platform)/Scripts/app/exportImport/blades/import-main.tpl.html',
                        notification: notify
                    };
                    bladeNavigationService.showBlade(blade);
                }
            };
            pushNotificationTemplateResolver.register(historyExportImportTemplate);

            $rootScope.$on("new-notification-event", function (event, notification) {
                if (notification.notifyType == 'SampleDataImportPushNotification' && $state.current && $state.current.name != 'setupWizard.sampleDataInstallation') {
                    $state.go('setupWizard.sampleDataInstallation', { notification: notification });
                }
            });
            //register setup wizard step - sample data auto installation
            setupWizard.registerStep({
                state: "setupWizard.sampleDataInstallation",
                priority: 10
            });

            // Register widget in Platform\Setup settings
            widgetService.registerWidget({
                isVisible: function (blade) {
                    return blade.currentEntities && blade.currentEntities['Setup'] && blade.currentEntities['Setup'].length
                        && blade.currentEntities['Setup'][0]['name'] === 'VirtoCommerce.SetupStep';
                },
                controller: 'platformWebApp.importSampleDataWidgetController',
                template: '$(Platform)/Scripts/app/exportImport/widgets/sampleDataImportWidget.tpl.html'
            },
                'settingsDetail');
        }]);

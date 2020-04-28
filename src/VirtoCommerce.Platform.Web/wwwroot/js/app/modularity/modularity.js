angular.module('platformWebApp').config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('workspace.modularity', {
        url: '/modules',
        templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
        controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
            var blade = {
                id: 'modulesMain',
                title: 'platform.blades.modules-main.title',
                controller: 'platformWebApp.modulesMainController',
                template: '$(Platform)/Scripts/app/modularity/blades/modules-main.tpl.html',
                isClosingDisabled: true
            };
            bladeNavigationService.showBlade(blade);
        }]
    });

    $stateProvider.state('setupWizard.modulesInstallation', {
        url: '/modulesInstallation',
        templateUrl: '$(Platform)/Scripts/app/modularity/templates/modulesInstallation.tpl.html',
        controller: ['$scope', '$state', '$stateParams', '$window', 'platformWebApp.modules', 'platformWebApp.WaitForRestart', 'platformWebApp.exportImport.resource', 'platformWebApp.setupWizard', '$timeout', function ($scope, $state, $stateParams, $window, modules, waitForRestart, exportImportResourse, setupWizard, $timeout) {
            $scope.notification = {};
            if ($stateParams.notification) {
                $scope.notification = $stateParams.notification;
            }
            //thats need when state direct open by url or push notification
            var step = setupWizard.findStepByState($state.current.name);
            if (!$scope.notification.created) {
                modules.autoInstall({}, function (data) {
                    //if already installed need skip this step
                    if (data.finished) {
                        setupWizard.showStep(step.nextStep);
                    }
                }, function (error) {
                    setupWizard.showStep(step.nextStep);
                });
            }

            $scope.restart = function () {
                $scope.restarted = true;
                modules.restart({}, function () {
                    setupWizard.showStep(step.nextStep);
                    // delay initial start for 3 seconds
                    $timeout(function () { }, 3000).then(function () {
                        return waitForRestart(1000);
                    });
                });
            };

            $scope.$on("new-notification-event", function (event, notification) {
                if (notification.notifyType == 'ModuleAutoInstallPushNotification') {
                    angular.copy(notification, $scope.notification);
                    if (notification.finished && notification.errorCount == 0) {
                        $scope.close();
                    }
                }
            });
        }]
    });
}])
    .run(
        ['platformWebApp.pushNotificationTemplateResolver', 'platformWebApp.bladeNavigationService', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', '$rootScope', 'platformWebApp.modules', 'platformWebApp.setupWizard', function (pushNotificationTemplateResolver, bladeNavigationService, mainMenuService, widgetService, $state, $rootScope, modules, setupWizard) {
            //Register module in main menu
            var menuItem = {
                path: 'configuration/modularity',
                icon: 'fa fa-cubes',
                title: 'platform.menu.modules',
                priority: 6,
                action: function () { $state.go('workspace.modularity'); },
                permission: 'platform:module:access'
            };
            mainMenuService.addMenuItem(menuItem);

            // Register push notification template
            pushNotificationTemplateResolver.register({
                priority: 900,
                satisfy: function (notify, place) { return place == 'header-notification' && notify.notifyType == 'ModulePushNotification'; },
                template: '$(Platform)/Scripts/app/modularity/notifications/headerNotification.tpl.html',
                action: function (notify) { $state.go('workspace.pushNotificationsHistory', notify); }
            });

            var historyExportImportTemplate =
            {
                priority: 900,
                satisfy: function (notify, place) { return place == 'history' && notify.notifyType == 'ModulePushNotification'; },
                template: '$(Platform)/Scripts/app/modularity/notifications/history.tpl.html',
                action: function (notify) {
                    var blade = {
                        id: 'moduleInstallProgress',
                        title: notify.title,
                        currentEntity: notify,
                        controller: 'platformWebApp.moduleInstallProgressController',
                        template: '$(Platform)/Scripts/app/modularity/wizards/newModule/module-wizard-progress-step.tpl.html'
                    };
                    bladeNavigationService.showBlade(blade);
                }
            };
            pushNotificationTemplateResolver.register(historyExportImportTemplate);

            //Switch to  setupWizard.modulesInstallation state when receive ModuleAutoInstallPushNotification push notification
            $rootScope.$on("new-notification-event", function (event, notification) {
                if (notification.notifyType == 'ModuleAutoInstallPushNotification' && $state.current && $state.current.name != 'setupWizard.modulesInstallation') {
                    $state.go('setupWizard.modulesInstallation', { notification: notification });
                }
            });
            //register setup wizard step - modules auto installation
            setupWizard.registerStep({
                state: "setupWizard.modulesInstallation",
                priority: 1
            });

        }])
    .factory('platformWebApp.moduleHelper', function () {
        // semver comparison: https://gist.github.com/TheDistantSea/8021359
        return {};
    });

angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('workspace.modularity', {
        url: '/modules',
        templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
        controller: ['platformWebApp.bladeNavigationService', function (bladeNavigationService) {
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
        controller: ['$scope', '$state', '$stateParams', 'platformWebApp.modulesApi', 'platformWebApp.WaitForRestart', 'platformWebApp.setupWizard', '$timeout', function ($scope, $state, $stateParams, modulesApi, waitForRestart, setupWizard, $timeout) {
            $scope.notification = {};
            if ($stateParams.notification) {
                $scope.notification = $stateParams.notification;
            }
            //thats need when state direct open by url or push notification
            var step = setupWizard.findStepByState($state.current.name);
            if (!$scope.notification.created) {
                modulesApi.autoInstall({}, function (data) {
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
                modulesApi.restart({}, function () {
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
.run(['platformWebApp.pushNotificationTemplateResolver', 'platformWebApp.bladeNavigationService', 'platformWebApp.mainMenuService', '$state', '$rootScope', 'platformWebApp.setupWizard', function (pushNotificationTemplateResolver, bladeNavigationService, mainMenuService, $state, $rootScope, setupWizard) {
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
.factory('platformWebApp.moduleHelper', ['platformWebApp.modulesApi', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function (modulesApi, bladeNavigationService, dialogService) {
    // semver comparison: https://gist.github.com/TheDistantSea/8021359
    var listeners = [];

    function isModuleInstalled(moduleId) {
        return _.some(this.modules, function (x) {
            return x.id === moduleId && x.isInstalled;
        });
    }

    function register(callback) {
        listeners.push(callback);
    }

    function onLoaded(args) {
        listeners.forEach(function (callback) {
            callback(args);
        });
    }

    function performAction(action, selection, blade, useParentBlade) {
        if (_.any(selection)) {
            blade.isLoading = true;

            // eliminate duplicating nodes, if any
            var grouped = _.groupBy(selection, 'id');
            selection = [];
            _.each(grouped, function (vals) {
                selection.push(_.last(vals));
            });

            // find not installed versions
            if (action === 'update') {
                var installed = angular.copy(selection);
                selection = [];
                var modules = this.modules;
                _.each(installed, function (x) {
                    var notInstalled = _.last(_.where(modules, { id: x.id, isInstalled: false }));
                    selection.push(notInstalled);
                });
            }

            var modulesApiMethod = action === 'uninstall' ? modulesApi.getDependents : modulesApi.getDependencies;

            modulesApiMethod(selection, function (data) {
                blade.isLoading = false;

                var dialog = {
                    id: "confirm",
                    action: action,
                    selection: selection,
                    dependencies: data,
                    callback: function (resume) {
                        if (resume) {
                            blade.isLoading = true;

                            // combine selected modules with dependencies/dependents
                            _.each(selection, function (x) {
                                if (!_.findWhere(data, { id: x.id })) {
                                    data.push(x);
                                }
                            });

                            // apply the action
                            var apiActionDescriptor = getApiActionDescriptor(action);
                            if (apiActionDescriptor.method) {
                                apiActionDescriptor.method(data, function (progressData) {
                                    blade.isLoading = false;
                                    // show module (un)installation progress
                                    var newBlade = {
                                        id: 'moduleInstallProgress',
                                        currentEntity: progressData,
                                        controller: 'platformWebApp.moduleInstallProgressController',
                                        template: '$(Platform)/Scripts/app/modularity/wizards/newModule/module-wizard-progress-step.tpl.html',
                                        title: apiActionDescriptor.title
                                    };

                                    bladeNavigationService.showBlade(newBlade, useParentBlade ? blade.parentBlade : blade);
                                }, function (error) {
                                    bladeNavigationService.setError('Error ' + error.status, blade);
                                });
                            }
                        }
                    }
                }
                dialogService.showDialog(dialog, '$(Platform)/Scripts/app/modularity/dialogs/moduleAction-dialog.tpl.html', 'platformWebApp.confirmDialogController');
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        }
    }

    function getApiActionDescriptor(action) {
        switch (action) {
            case 'install':
                return { title: 'platform.blades.module-wizard-progress-step.title-install', method: modulesApi.install };
            case 'update':
                return { title: 'platform.blades.module-wizard-progress-step.title-update', method: modulesApi.update };
            case 'uninstall':
                return { title: 'platform.blades.module-wizard-progress-step.title-uninstall', method: modulesApi.uninstall };
            default:
                return {};
        }
    }

    return {
        modules: [],
        isModuleInstalled: isModuleInstalled,
        register: register,
        onLoaded: onLoaded,
        performAction: performAction
    }
}]);

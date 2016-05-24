angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
	$stateProvider
        .state('workspace.modularity', {
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

	$stateProvider
	.state('modulesAutoInstallation', {
		url: '/modulesAutoInstallation',
		templateUrl: '$(Platform)/Scripts/app/modularity/templates/modulesAutoInstallation.tpl.html',
		controller: ['$scope', '$state', '$window', 'platformWebApp.modules', '$localStorage', 'platformWebApp.exportImport.resource', function ($scope, $state, $window, modules, $localStorage, exportImportResourse) {
			$scope.notification = {};		

			$scope.restart = function () {
				$scope.restarted = true;
				$localStorage['RunSampleDataImport'] = true;				
				modules.restart({}, function () {
					$state.go('workspace');
					$window.location.reload();
				});
			};
			$scope.close = function () { $state.go('workspace') };
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
  ['platformWebApp.pushNotificationTemplateResolver', 'platformWebApp.bladeNavigationService', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', '$rootScope', 'platformWebApp.modules', function (pushNotificationTemplateResolver, bladeNavigationService, mainMenuService, widgetService, $state, $rootScope, modules) {
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

  	//Push notifications
  	var menuExportImportTemplate =
	   {
	   	priority: 900,
	   	satisfy: function (notify, place) { return place == 'menu' && notify.notifyType == 'ModulePushNotification'; },
	   	template: '$(Platform)/Scripts/app/modularity/notifications/menu.tpl.html',
	   	action: function (notify) { $state.go('pushNotificationsHistory', notify); }
	   };
  	pushNotificationTemplateResolver.register(menuExportImportTemplate);

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

  	//Switch to  modulesAutoInstallation state when receive ModuleAutoInstallPushNotification push notification
  	$rootScope.$on("new-notification-event", function (event, notification) {
  		if (notification.notifyType == 'ModuleAutoInstallPushNotification' && $state.current && $state.current.name != 'modulesAutoInstallation') {
  			$state.go('modulesAutoInstallation')
  		}
  	});
  	//Try to auto install  modules
  	$rootScope.$on('loginStatusChanged', function (event, authContext) {
  		if (authContext.isAuthenticated) {
  			modules.autoInstall();
  		}
  	});
  }])
.factory('platformWebApp.moduleHelper', function () {
	// semver comparison: https://gist.github.com/TheDistantSea/8021359
	return {};
})
;

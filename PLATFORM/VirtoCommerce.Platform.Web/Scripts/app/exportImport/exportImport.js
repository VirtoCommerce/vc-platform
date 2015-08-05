angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
	$stateProvider
        .state('workspace.exportImport', {
        	url: '/exportImport',
        	templateUrl: 'Scripts/common/templates/home.tpl.html',
        	controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
        		var blade = {
        			id: 'exportImport',
        			title: 'Data export and import',
        			controller: 'platformWebApp.exportImport.mainController',
        			template: 'Scripts/app/exportImport/blades/exportImport-main.tpl.html',
        			isClosingDisabled: true
        		};
        		bladeNavigationService.showBlade(blade);
        	}
        	]
        });

	$stateProvider
        .state('sampleDataInitialization', {
        	url: '/sampleDataInitialization',
        	templateUrl: 'Scripts/app/exportImport/templates/sampleDataInitialization.tpl.html',
        	controller: ['$scope', '$state', function ($scope, $state) {
        		$scope.notification = {};
        		$scope.close = function () { $state.go('workspace') };
        		$scope.$on("new-notification-event", function (event, notification) {
        			if (notification.notifyType == 'SampleDataImportPushNotification') {
        				angular.copy(notification, $scope.notification);
        				if (notification.finished && notification.errorCount == 0) {
        					$scope.close();
        				}
        			}
        		});
        	}
        	]
        });
}]
)
.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', 'platformWebApp.pushNotificationTemplateResolver', 'platformWebApp.bladeNavigationService', 'platformWebApp.exportImport.resource', function ($rootScope, mainMenuService, widgetService, $state, pushNotificationTemplateResolver, bladeNavigationService, exportImportResourse) {
  	var menuItem = {
  		path: 'configuration/exportImport',
  		icon: 'fa fa-download',
  		title: 'Export & Import',
  		priority: 10,
  		action: function () { $state.go('workspace.exportImport'); },
  		permission: 'platform:backupAdministrator'
  	};
  	mainMenuService.addMenuItem(menuItem);

  	//Push notifications
  	var menuExportImportTemplate =
	   {
	   	priority: 900,
	   	satisfy: function (notify, place) { return place == 'menu' && (notify.notifyType == 'PlatformExportPushNotification' || notify.notifyType == 'PlatformImportPushNotification'); },
	   	template: 'Scripts/app/exportImport/notifications/menu.tpl.html',
	   	action: function (notify) { $state.go('pushNotificationsHistory', notify) }
	   };
  	pushNotificationTemplateResolver.register(menuExportImportTemplate);

  	var historyExportImportTemplate =
	  {
	  	priority: 900,
	  	satisfy: function (notify, place) { return place == 'history' && (notify.notifyType == 'PlatformExportPushNotification' || notify.notifyType == 'PlatformImportPushNotification'); },
	  	template: 'Scripts/app/exportImport/notifications/history.tpl.html',
	  	action: function (notify) {
	  		var isExport = notify.notifyType == 'PlatformExportPushNotification';
	  		var blade = {
	  			id: 'platformExportImport',
	  			controller: isExport ? 'platformWebApp.exportImport.exportMainController' : 'platformWebApp.exportImport.importMainController',
	  			template: isExport ? 'Scripts/app/exportImport/blades/export-main.tpl.html' : 'Scripts/app/exportImport/blades/import-main.tpl.html',
	  			notification: notify
	  		};
	  		bladeNavigationService.showBlade(blade);
	  	}
	  };
  	pushNotificationTemplateResolver.register(historyExportImportTemplate);

  	//Try to import sample data
  	$rootScope.$on('loginStatusChanged', function (event, authContext) {
  		if (authContext.isAuthenticated) {
  			exportImportResourse.importSampleData({}, function (notification) {
  				if (angular.isDefined(notification) && notification.notifyType == 'SampleDataImportPushNotification') {
  					$state.go('sampleDataInitialization')
  				}
  			});
  		}
  	});
  
  }]);

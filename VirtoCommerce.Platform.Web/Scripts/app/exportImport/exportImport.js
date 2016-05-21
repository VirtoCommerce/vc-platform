angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
	$stateProvider
        .state('workspace.exportImport', {
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
        	}
        	]
        });

	$stateProvider
        .state('sampleDataChoose', {
        	url: '/sampleDataChoose',
        	params: { sampleDataInfos: null },
        	templateUrl: '$(Platform)/Scripts/app/exportImport/templates/sampleDataChoose.tpl.html',
        	controller: ['$scope', '$state', '$stateParams', 'platformWebApp.exportImport.resource', function ($scope, $state, $stateParams, exportImportResource) {
        		$scope.sampleDataInfos = $stateParams.sampleDataInfos;
        		$scope.loading = false;
        		$scope.selectItem = function (sampleData) {
        			$scope.loading = true;
        			exportImportResource.importSampleData({ url: sampleData.url }, function () {
        				$state.go(sampleData.url ? 'sampleDataInitialization' : 'workspace');
        			});
        		};
        	}
        	]
        });

	$stateProvider
        .state('sampleDataInitialization', {
        	url: '/sampleDataInitialization',
        	templateUrl: '$(Platform)/Scripts/app/exportImport/templates/sampleDataInitialization.tpl.html',
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
	   	satisfy: function (notify, place) { return place == 'menu' && (notify.notifyType == 'PlatformExportPushNotification' || notify.notifyType == 'PlatformImportPushNotification'); },
	   	template: '$(Platform)/Scripts/app/exportImport/notifications/menu.tpl.html',
	   	action: function (notify) { $state.go('pushNotificationsHistory', notify) }
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
  		if (notification.notifyType == 'SampleDataImportPushNotification' && $state.current && $state.current.name != 'sampleDataInitialization')
  		{
  			$state.go('sampleDataInitialization')
  		}
  	});
  	//Try to import sample data
  	$rootScope.$on('loginStatusChanged', function (event, authContext) {
  		if (authContext.isAuthenticated) {
  			exportImportResourse.sampleDataDiscover({}, function (sampleDataInfos) {
  				if (angular.isArray(sampleDataInfos) && sampleDataInfos.length > 0)
  				{
  					if (sampleDataInfos.length > 1) {
  						$state.go('sampleDataChoose', { sampleDataInfos: sampleDataInfos });
  					}
  					else
  					{
  						exportImportResourse.importSampleData({ url: sampleDataInfos[0].url });
  					}
  				}
  			});
  		}
  	});
  
  }]);

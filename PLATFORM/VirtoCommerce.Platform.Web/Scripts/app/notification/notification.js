angular.module('platformWebApp')
.config(
  ['$stateProvider', function ($stateProvider) {
  	$stateProvider
		.state('notificationsHistory', {
			url: '/notification/:id',
			templateUrl: 'Scripts/app/notification/notification.tpl.html',
			controller: ['$scope', 'bladeNavigationService', 'notificationService', function ($scope, bladeNavigationService, notificationService) {
				var blade = {
					id: 'notifications',
					title: 'Notifications',
					breadcrumbs: [],
					subtitle: 'Notifications history',
					controller: 'notificationsHistoryController',
					template: 'Scripts/app/notification/blade/history.tpl.html',
					isClosingDisabled: true
				};
				bladeNavigationService.showBlade(blade);
			}
			]
		});
  }])
.factory('notificationTemplateResolver', ['bladeNavigationService', '$state', function (bladeNavigationService, $state) {
	var notificationTemplates = [];

	var defaultTypes = ['error', 'info', 'warning'];
	function register(template) {
		notificationTemplates.push(template);
		notificationTemplates.sort(function (a, b) { return a.priority - b.priority; })
	};
	function resolve(notification, place) {
		return _.find(notificationTemplates, function (x) { return x.satisfy(notification, place); })
	};
	var retVal = {
		register: register,
		resolve: resolve,
	};
	
	//Recent events notification template (error, info, debug) 
	var menuDefaultTemplate =
		{
			priority: 1000,
			satisfy: function (notification, place) { return place == 'menu'; },
			//template for display that notification in menu and list
			template: 'Scripts/app/notification/menuDefault.tpl.html',
			//action executed when notification selected
			action: function (notify) { $state.go('notificationsHistory', notify) }
		};

	//In history list notification template (error, info, debug)
	var historyDefaultTemplate =
		{
			priority: 1000,
			satisfy: function (notification, place) { return place == 'history'; },
			//template for display that notification in menu and list
			template: 'Scripts/app/notification/blade/historyDefault.tpl.html',
			//action executed in event detail
			action: function (notify) {
				var blade = {
					id: 'notifyDetail',
					title: 'Event detail',
					subtitle: 'Event detail',
					template: 'Scripts/app/notification/blade/historyDetailDefault.tpl.html',
					isClosingDisabled: false,
					notify: notify
				};
				bladeNavigationService.showBlade(blade);
			}
		};

	retVal.register(menuDefaultTemplate);
	retVal.register(historyDefaultTemplate);

	return retVal;
}])
.factory('notificationService', ['signalRHubProxy', '$interval', '$state', 'mainMenuService', 'notificationTemplateResolver', 'notifications', function (signalRHubProxy, $interval, $state, mainMenuService, notificationTemplateResolver, notifications) {

    var clientPushHubProxy = signalRHubProxy(signalRHubProxy.defaultServer, 'clientPushHub', { logging: true });
    clientPushHubProxy.on('notification', function (data) {
        var notifyMenu = mainMenuService.findByPath('notification');
        var notificationTemplate = notificationTemplateResolver.resolve(data, 'menu');

        var menuItem = {
            parent: notifyMenu,
            path: 'notification/events',
            icon: 'fa fa-comment',
            title: data.title,
            priority: 2,
            permission: '',
            action: notificationTemplate.action,
            template: notificationTemplate.template,
            notify: data
        };
        notifyMenu.children.push(menuItem);

        notifyMenu.incremented = true;
    });

	//var timer = new Date().getUTCDate();
	var notifyStatusEnum =
		{
			running: 0,
			aborted: 1,
			finished: 2,
			error: 3
		};

	function innerNotification(notification) {
		
		//Group notification by text
		
		notifications.upsert(notification, function (data, status, headers, config) {
			notificationRefresh();
		});
	};

	function markAllAsRead() {
		notifications.markAllAsRead(null, function (data, status, headers, config) {
		    notificationRefresh();

		    var notifyMenu = mainMenuService.findByPath('notification');
		    notifyMenu.incremented = false;
		});

	};

	function notificationRefresh() {
		//notifyMenu.incremented = false;
		//notifications.query({ start: 0, count: 15 }, function (data, status, headers, config) {

		//	notifyMenu.incremented = notifyMenu.newCount < data.newCount;
		//	notifyMenu.newCount = data.newCount;
		//	notifyMenu.progress = _.some(data.notifyEvents, function (x) { return x.isRunning; });

		//	//clear all child
		//	notifyMenu.children.splice(0, notifyMenu.children.length);

		//	//Add all events
		//	angular.forEach(data.notifyEvents, function (x) {

		//		var notificationTemplate = notificationTemplateResolver.resolve(x, 'menu');

		//		var menuItem = {
		//			parent: notifyMenu,
		//			path: 'notification/events',
		//			icon: 'fa fa-comment',
		//			title: x.title,
		//			priority: 2,
		//			permission: '',
		//			action: notificationTemplate.action,
		//			template: notificationTemplate.template,
		//			notify: x
		//		};
		//		notifyMenu.children.push(menuItem);
		//	});

		//});
	};

	var retVal = {
		run: function () {
			if (!this.running) {
			    var notifyMenu = mainMenuService.findByPath('notification');
			    if (!angular.isDefined(notifyMenu)) {
			        notifyMenu = {
			            path: 'notification',
			            icon: 'fa fa-comments',
			            title: 'Notifications',
			            priority: 2,
			            permission: '',
			            headerTemplate: 'Scripts/app/notification/menuHeader.tpl.html',
			            listTemplate: 'Scripts/app/notification/menuList.tpl.html',
			            template: 'Scripts/app/notification/menu.tpl.html',
			            action: function () { markAllAsRead(); },
			            showHistory: function () { $state.go('notificationsHistory'); },
			            clearRecent: function () { notifyMenu.children.splice(0, notifyMenu.children.length); },
			            children: [],
			            newCount: 0
			        };
			        mainMenuService.addMenuItem(notifyMenu);
			    }
				this.running = true;
			};
		},
		running: false,
		error: function (notification) {
			notification.notifyType = 'error';
			return innerNotification(notification);
		},
		warning: function (notification) {
			notification.notifyType = 'warning';
			return innerNotification(notification);
		},
		info: function (notification) {
			notification.notifyType = 'info';
			return innerNotification(notification);
		}
	};
	return retVal;

}]);
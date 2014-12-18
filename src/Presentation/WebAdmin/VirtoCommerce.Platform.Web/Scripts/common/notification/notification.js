angular.module('platformWebApp.notification', [
	'platformWebApp.mainMenu',
	'notifications.blades.history',
	'platformWebApp.notification.resources',
	'angularMoment'
])
.config(
  ['$stateProvider', function ($stateProvider) {
  	$stateProvider
		.state('notification', {
			url: '/notification/:id',
			templateUrl: 'Scripts/common/notification/notification.tpl.html',
			controller: ['$scope', 'bladeNavigationService', 'notificationService', function ($scope, bladeNavigationService, notificationService) {
				var blade = {
					id: 'notifications',
					title: 'Notifications',
					breadcrumbs: [],
					subtitle: 'Notifications history',
					controller: 'notificationsHistoryController',
					template: 'Scripts/common/notification/blade/history.tpl.html',
					isClosingDisabled: true
				};
				bladeNavigationService.showBlade(blade);
			}
			]
		});
  }])
.factory('notificationTemplateResolver', ['bladeNavigationService', '$state', function (bladeNavigationService, $state) {
	var notificationTemplates = [];

	function register(template) {
		notificationTemplates.push(template);
		notificationTemplates.sort(function (a, b) { return a.priority - b.priority; })
	};
	function resolve(notification) {
		return _.find(notificationTemplates, function (x) { return x.satisfy(notification); })
	};
	var retVal = {
		register: register,
		resolve: resolve,
	};
	//Default notification template
	var defaultTemplate =
		{
			priority: 1000,
			satisfy: function () { return true; },
			//template for display that notification in menu and list
			template: 'Scripts/common/notification/default.tpl.html',
			//action excecuted in event detail
			action: function (notify) {
				var blade = {
					id: 'notifyDetail',
					title: 'Event detail',
					subtitle: 'Event detail',
					template: 'Scripts/common/notification/blade/defaultDetail.tpl.html',
					isClosingDisabled: false,
					notify: notify
				};
				bladeNavigationService.showBlade(blade);
			}
		};
	retVal.register(defaultTemplate)
	return retVal;
}])
.factory('notificationService', ['$http', '$interval', '$state', 'mainMenuService', 'notificationTemplateResolver', 'notifications', function ($http, $interval, $state, mainMenuService, notificationTemplateResolver, notifications) {

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
		});

	};

	function notificationRefresh() {
		var notifyMenu = mainMenuService.findByPath('notification');
		if (!angular.isDefined(notifyMenu)) {
			notifyMenu = {
				path: 'notification',
				icon: 'glyphicon glyphicon-comment',
				title: 'Notifications',
				priority: 2,
				permission: '',
				headerTemplate: 'Scripts/common/notification/menuHeader.tpl.html',
				template: 'Scripts/common/notification/menu.tpl.html',
				action: function () { markAllAsRead(); },
				showHistory: function () { $state.go('notification') },
				children: [],
				newCount : 0
			};
			mainMenuService.addMenuItem(notifyMenu);
		}
		notifyMenu.incremented = false;
		notifications.query({ start: 0, count: 15 }, function (data, status, headers, config) {

			//timer = new Date().getUTCDate();
			notifyMenu.incremented = notifyMenu.newCount < data.newCount;
			notifyMenu.newCount = data.newCount;
			notifyMenu.progress = _.some(data.notifyEvents, function (x) { return x.status == notifyStatusEnum.running; });

			//clear all child
			notifyMenu.children.splice(0, notifyMenu.children.length);

			//Add all events
			angular.forEach(data.notifyEvents, function (x) {

				notificationTemplate = notificationTemplateResolver.resolve(x);

				var menuItem = {
					parent: notifyMenu,
					path: 'notification/events',
					icon: 'glyphicon glyphicon-comment',
					title: x.title,
					priority: 2,
					action: function (notify) { $state.go('notification', notify) },
					permission: '',
					template: notificationTemplate.template,
					notify: x
				};
				notifyMenu.children.push(menuItem);
			});

		});
	};

	var retVal = {
		run: function () {
			if (!this.running) {
				notificationRefresh();
				this.running = true;
				$interval(notificationRefresh, 10000);
			};
		},
		running: false,
		error: function (data) {
			return innerNotification({ notifyType: 'error', title: data.title, description: data.description, status: notifyStatusEnum.finished });
		},
		warning: function (data) {
			return innerNotification({ notifyType: 'warning', title: data.title, description: data.description, status: notifyStatusEnum.finished });
		},
		info: function (data) {
			return innerNotification({ notifyType: 'info', title: data.title, description: data.description, status: notifyStatusEnum.finished });
		},
		task: function (data) {
			return innerNotification({ notifyType: 'task', title: data.title, description: data.description });
		}
	};
	return retVal;

}]);
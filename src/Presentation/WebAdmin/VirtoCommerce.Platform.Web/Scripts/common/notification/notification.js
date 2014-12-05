angular.module('platformWebApp.notification', [
	'platformWebApp.mainMenu'
])
.config(
  ['$stateProvider', function ($stateProvider) {
  	$stateProvider
		.state('notification', {
			url: '/notification/:id',
			templateUrl: 'Scripts/common/notification/notification.tpl.html',
			controller: function ($stateParams, notificationService) {
				//$stateParams.id is id for event
				//todo: display history here
				
			}
		});
}])
.factory('notificationService', ['$http', '$interval', 'mainMenuService', function ($http, $interval, mainMenuService) {
	var serviceBase = 'api/notification/';
	
	//Enums
	var notifyTypeEnum =
    {
    	info: 0,
    	warning: 1,
    	error: 2,
    	task: 3
    };
	if (Object.freeze) Object.freeze(notifyTypeEnum);

	var notifyStatusEnum =
    {
    	running: 0,
    	aborted: 1,
    	finished: 2,
    	error: 3
    };

	if (Object.freeze) Object.freeze(notifyStatusEnum);


	function innerNotification(notification) {
		$http.post(serviceBase, notification)
			.success(function (data, status, headers, config) {
				notificationRefresh();
			})
			.error(function (data, status, headers, config) {
				//todo: Need a add notification to list anyway
			});
	};

	function markAllAsRead() {
		$http.get(serviceBase + 'markAllAsRead')
				.success(function (data, status, headers, config) {
					notificationRefresh();
				})
				.error(function (data, status, headers, config) {
				});
	};

	function notificationRefresh() {
		var notifyMenu = mainMenuService.findByPath('notification');
		if (!angular.isDefined(notifyMenu))
		{
			notifyMenu = {
				path: 'notification',
				icon: 'glyphicon glyphicon-comment',
				title: 'Notifications',
				priority: 2,
				permission: '',
				template: 'Scripts/common/notification/notifyMenu.tpl.html',
				customAction: function () { markAllAsRead(); },
				children: []
			};
			mainMenuService.addMenuItem(notifyMenu);
		}
		notifyMenu.incremented = false;
		$http.get(serviceBase + 'allRecent').success(function (data, status, headers, config) {
			
				notifyMenu.incremented = notifyMenu.newCount < data.newCount;
				notifyMenu.newCount = data.newCount;
				notifyMenu.progress = _.some(data.notifyEvents, function (x) { return x.status == notifyStatusEnum.running; });

				//clear all child
				notifyMenu.children.splice(0, notifyMenu.children.length)

				//Add all events
				angular.forEach(data.notifyEvents, function (x) {
				    var menuItem = {
				    	parent: notifyMenu,
						path: 'notification/events',
						icon: 'glyphicon glyphicon-comment',
						title: x.title,
						priority: 2,
						state: 'notification',
						stateParams: x,
						permission: '',
						template: 'Scripts/common/notification/notify.tpl.html',
						notify: x,
					};
					notifyMenu.children.push(menuItem);
				});
				
			}).
			error(function (data, status, headers, config) {
  				//todo: Need add error notification
			 });
	};

	var retVal = {
		run: function () {
			if (!this.running) {
				notificationRefresh();
				this.running = true;
				//$interval(notificationRefresh, 10000);
			};
		},
		running: false,
		error: function (data) {
			return innerNotification({ notifyType: notifyTypeEnum.error, title: data.title, description: data.description, status: notifyStatusEnum.finished });
		},
		warning: function (data) {
			return innerNotification({ notifyType: notifyTypeEnum.warning, title: data.title, description: data.description, status: notifyStatusEnum.finished });
		},
		info: function (data) {
			return innerNotification({ notifyType: notifyTypeEnum.info, title: data.title, description: data.description, status: notifyStatusEnum.finished });
		},
		task: function (data) {
		    return innerNotification({ notifyType: notifyTypeEnum.task, title: data.title, description: data.description });
		}	
	};
	return retVal;

}]);

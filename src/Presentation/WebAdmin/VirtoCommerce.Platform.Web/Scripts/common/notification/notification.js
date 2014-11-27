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
				notificationService.markAllAsRead();
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
		$http.get(serviceBase + 'allRecent').
			success(function (data, status, headers, config) {
				//Clear all previous notification from menu
				mainMenuService.clearByPath('notification');
			
				var menuItems = [];
				var menuItem = {
					path: 'notification',
					icon: 'glyphicon glyphicon-comment',
					title: 'Notifications',
					priority: 2,
					permission: '',
					template: 'Scripts/common/notification/notifyMenu.tpl.html',
					newCount: data.newCount,
					progress: _.some(data.notifyEvents, function (x) { return x.status == notifyStatusEnum.running; }),
					customAction: function() { markAllAsRead(); }
				};
				menuItems.push(menuItem);

				//Add all events
				angular.forEach(data.notifyEvents, function (x) {
					var menuItem = {
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
					menuItems.push(menuItem);
				
				});
				mainMenuService.addMenuItems(menuItems);
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
				//$interval(notificationRefresh, 10000)
			};
		},
		running: false,
		error: function (message) {
			return innerNotification({ notifyType: notifyTypeEnum.error, title: message });
		},
		warning: function (message) {
			return innerNotification({ notifyType: notifyTypeEnum.warning, title: message });
		},
		info: function (message) {
			return innerNotification({ notifyType: notifyTypeEnum.info, title: message });
		},
		task: function (message) {
			return innerNotification({ notifyType: notifyTypeEnum.task, title: message });
		}	
	};
	return retVal;

}]);

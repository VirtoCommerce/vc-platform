angular.module('platformWebApp')
.factory('platformWebApp.pushNotifications', ['$resource', function ($resource) {

	return $resource('api/pushnotifications/:id', { id: '@Id' }, {
		markAllAsRead: { method: 'GET', url: 'api/pushnotifications/markAllAsRead' },
		query: { method: 'GET', url: 'api/pushnotifications' },
		upsert: { method: 'POST', url: 'api/pushnotifications' }
	});
}]);


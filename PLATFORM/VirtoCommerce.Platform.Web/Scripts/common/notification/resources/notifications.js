angular.module('platformWebApp')
.factory('notifications', ['$resource', function ($resource) {

	return $resource('api/notifications/:id', { id: '@Id' }, {
		markAllAsRead: { method: 'GET', url: 'api/notifications/markAllAsRead' },
		query: { method: 'GET', url: 'api/notifications' },
		upsert: { method: 'POST', url: 'api/notifications' }
	});
}]);


angular.module('platformWebApp')
.factory('platformWebApp.pushNotifications', ['$resource', function ($resource) {

    return $resource('api/platform/pushnotifications/:id', { id: '@Id' }, {
        markAllAsRead: { method: 'POST', url: 'api/platform/pushnotifications/markAllAsRead' },
        query: { method: 'POST', url: 'api/platform/pushnotifications' }
	});
}]);

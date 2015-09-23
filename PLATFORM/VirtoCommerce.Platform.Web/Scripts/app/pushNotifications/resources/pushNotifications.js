angular.module('platformWebApp')
.factory('platformWebApp.pushNotifications', ['$resource', function ($resource) {

    return $resource('api/platform/pushnotifications/:id', { id: '@Id' }, {
        markAllAsRead: { method: 'GET', url: 'api/platform/pushnotifications/markAllAsRead' },
        query: { method: 'GET', url: 'api/platform/pushnotifications' }
       
	});
}]);


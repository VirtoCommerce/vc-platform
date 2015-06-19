angular.module('platformWebApp')
.factory('platformWebApp.newnotifications', ['$resource', function ($resource) {

	return $resource('api/notification/:id', { id: '@Id' }, {
		getNotificationList: { method: 'GET', url: 'api/notification', isArray: true },
		getTemplate: { method: 'GET', url: 'api/notification/template/:type/:objectId' },
		upadteTemplate: { method: 'POST', url: 'api/notification/template'}
	});
}]);
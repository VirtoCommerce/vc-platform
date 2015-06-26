angular.module('platformWebApp')
.factory('platformWebApp.newnotifications', ['$resource', function ($resource) {

	return $resource('api/notification/:id', { id: '@Id' }, {
		getNotificationList: { method: 'GET', url: 'api/notification', isArray: true },
		getTemplate: { method: 'GET', url: 'api/notification/template/:type/:objectId' },
		updateTemplate: { method: 'POST', url: 'api/notification/template' },
		prepareTestData: { method: 'GET', url: 'api/notification/template/:type/preparetestdata', isArray: true },
		resolveNotification: { method: 'POST', url: 'api/notification/template/:type/resolvenotification' },
		sendNotification: { method: 'POST', url: 'api/notification/template/:type/sendnotification' }
	});
}]);
angular.module('platformWebApp')
.factory('platformWebApp.newnotifications', ['$resource', function ($resource) {

	return $resource('api/notification/:id', { id: '@Id' }, {
		getNotificationList: { method: 'GET', url: 'api/notification', isArray: true },
		getTemplate: { method: 'GET', url: 'api/notification/template/:type/:objectId/:objectTypeId/:language' },
		getTemplates: { method: 'GET', url: 'api/notification/template/:type/:objectId/:objectTypeId', isArray: true },
		updateTemplate: { method: 'POST', url: 'api/notification/template' },
		deleteTemplate: { method: 'DELETE', url: 'api/notification/template/:id' },
		prepareTestData: { method: 'GET', url: 'api/notification/template/:type/preparetestdata', isArray: true },
		resolveNotification: { method: 'POST', url: 'api/notification/template/:type/:objectId/:objectTypeId/:language/resolvenotification' },
		sendNotification: { method: 'POST', url: 'api/notification/template/:type/:objectId/:objectTypeId/:language/sendnotification' },
		getNotificationJournalList: { method: 'GET', url: 'api/notification/journal/:objectId/:objectTypeId' },
		getNotificationJournalDetails: { method: 'GET', url: 'api/notification/notification/:id' },
		stopSendingNotifications: { method: 'POST', url: 'api/notification/stopnotifications' }
	});
}]);
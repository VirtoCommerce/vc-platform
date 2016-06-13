angular.module('platformWebApp')
.factory('platformWebApp.notifications', ['$resource', function ($resource) {

	return $resource('api/platform/notification/:id', { id: '@Id' }, {
		getNotificationList: { method: 'GET', url: 'api/platform/notification', isArray: true },
		getTemplateById: { method: 'GET', url: 'api/platform/notification/template/:id' },
		getTemplate: { method: 'GET', url: 'api/platform/notification/template' },
		getTemplates: { method: 'GET', url: 'api/platform/notification/templates', isArray: true },
		updateTemplate: { method: 'POST', url: 'api/platform/notification/template' },
		deleteTemplate: { method: 'DELETE', url: 'api/platform/notification/template/:id' },
		prepareTestData: { method: 'GET', url: 'api/platform/notification/template/:type/getTestingParameters', isArray: true },
		resolveNotification: { method: 'POST', url: 'api/platform/notification/template/rendernotificationcontent' },
		sendNotification: { method: 'POST', url: 'api/platform/notification/template/sendnotification' },
		getNotificationJournalList: { method: 'GET', url: 'api/platform/notification/journal/:objectId/:objectTypeId' },
		getNotificationJournalDetails: { method: 'GET', url: 'api/platform/notification/notification/:id' },
		stopSendingNotifications: { method: 'POST', url: 'api/platform/notification/stopnotifications' }
	});
}]);
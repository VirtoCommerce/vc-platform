angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeNotificationsLogWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	var blade = $scope.widget.blade;

	blade.showNotificationsLog = function () {
		var objectId = blade.currentEntity.id;
		var objectTypeId = 'Store';
		var newBlade = {
			id: 'storeNotificationLogWidgetChild',
			title: 'Store ' + blade.currentEntity.id + ' notification sending log',
			objectId: objectId,
			objectTypeId: objectTypeId,
			subtitle: 'Notifications log',
			controller: 'platformWebApp.notificationsJournalController',
			template: 'Scripts/app/newnotifications/blades/notifications-journal.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	};
}]);
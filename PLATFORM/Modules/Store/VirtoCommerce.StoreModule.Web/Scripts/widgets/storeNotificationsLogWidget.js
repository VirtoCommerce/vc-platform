angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeNotificationsLogWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	var blade = $scope.widget.blade;

	blade.showNotificationsLog = function () {
		var objectId = blade.currentEntity.id;
		var objectTypeId = 'Store';
		var newBlade = {
			id: 'storeNotificationLogWidgetChild',
			title: 'stores.widgets.storeNotificationsLogWidget.blade-title',
			titleValues: { id: blade.currentEntity.id },
			objectId: objectId,
			objectTypeId: objectTypeId,
			subtitle: 'stores.widgets.storeNotificationsLogWidget.blade-subtitle',
			controller: 'platformWebApp.notificationsJournalController',
			template: '$(Platform)/Scripts/app/notifications/blades/notifications-journal.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	};
}]);
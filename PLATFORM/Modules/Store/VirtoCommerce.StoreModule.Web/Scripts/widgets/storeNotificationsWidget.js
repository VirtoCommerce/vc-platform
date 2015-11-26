angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeNotificationsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	var blade = $scope.widget.blade;

	blade.showNotifications = function () {
		var objectId = blade.currentEntity.id;
		var objectTypeId = 'Store';
		var newBlade = {
			id: 'storeNotificationWidgetChild',
			title: 'stores.widgets.storeNotificationsWidget.blade-title',
			titleValues: { id: blade.currentEntity.id },
			objectId: objectId,
			objectTypeId: objectTypeId,
			subtitle: 'stores.widgets.storeNotificationsWidget.blade-subtitle',
			controller: 'platformWebApp.notificationsListController',
			template: '$(Platform)/Scripts/app/notifications/blades/notifications-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	};
}]);
angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeNotificationsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	var blade = $scope.widget.blade;

	blade.showNotifications = function () {
		var objectId = blade.currentEntity.id;
		var objectTypeId = 'Store';
		var newBlade = {
			id: 'storeNotificationWidgetChild',
			title: 'Store ' + blade.currentEntity.id + ' notification types list',
			objectId: objectId,
			objectTypeId: objectTypeId,
			subtitle: 'Notifications service',
			controller: 'platformWebApp.notificationsListController',
			template: 'Scripts/app/newnotifications/blades/notifications-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	};
}]);
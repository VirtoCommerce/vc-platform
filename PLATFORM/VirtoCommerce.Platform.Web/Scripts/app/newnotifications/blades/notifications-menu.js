angular.module('platformWebApp')
.controller('platformWebApp.notificationsMenuController', ['$scope', '$stateParams', 'platformWebApp.bladeNavigationService', function ($scope, $stateParams, bladeNavigationService) {
	var blade = $scope.blade;
	$scope.selectedNodeId = null;

	function initializeBlade() {
		var entities = [
            { id: '1', name: 'Notifications', templateName: 'notifications-list', controllerName: 'notificationsListController', icon: 'fa-list' },
            { id: '2', name: 'Journal of sending', templateName: 'notifications-journal', controllerName: 'notificationsJournalController', icon: 'fa-book' }];
		blade.currentEntities = entities;
		blade.isLoading = false;
	};

	blade.openBlade = function (data) {
		$scope.selectedNodeId = data.id;

		var objectId = (angular.isUndefined($stateParams.objectId) || $stateParams.objectId === null) ? 'Platform' : $stateParams.objectId;
		var objectTypeId = (angular.isUndefined($stateParams.objectTypeId) || $stateParams.objectTypeId === null) ? 'Platform' : $stateParams.objectTypeId;
		var newBlade = {
			id: 'marketingMainListChildren',
			title: data.name,
			objectId: objectId,
			objectTypeId: objectTypeId,
			subtitle: 'Marketing service',
			controller: 'platformWebApp.' + data.controllerName,
			template: 'Scripts/app/newnotifications/blades/' + data.templateName + '.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.headIcon = 'fa-envelope';

	initializeBlade();

}]);
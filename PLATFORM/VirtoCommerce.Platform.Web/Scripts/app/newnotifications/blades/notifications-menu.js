angular.module('platformWebApp')
.controller('platformWebApp.notificationsMenuController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($rootScope, $scope, bladeNavigationService, dialogService) {
	var blade = $scope.blade;
	$scope.selectedNodeId = null;

	function initializeBlade() {
		var entities = [
            { id: '1', name: 'Notifications', templateName: 'notifications-list', controllerName: 'notificationsListController', icon: 'fa-list' },
            { id: '2', name: 'Journal of sending', templateName: 'notification-journal', controllerName: 'notificationsJournalController', icon: 'fa-book' }];
		blade.currentEntities = entities;
		blade.isLoading = false;
	};

	blade.openBlade = function (data) {
		$scope.selectedNodeId = data.id;

		var newBlade = {
			id: 'marketingMainListChildren',
			title: data.name,
			subtitle: 'Marketing service',
			controller: 'platformWebApp.' + data.controllerName,
			template: 'Scripts/app/newnotifications/blades/' + data.templateName + '.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.headIcon = 'fa fa-flag';

	initializeBlade();

}]);
angular.module('platformWebApp')
.controller('platformWebApp.editTemplateController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.newnotifications', function ($rootScope, $scope, bladeNavigationService, dialogService, notifications) {
	$scope.selectedEntityId = null;
	var blade = $scope.blade;

	blade.initialize = function () {
		blade.isLoading = true;

		notifications.getTemplate({ type: currentEntityParent.type, objectId: currentEntityParent.objectId }, function (data) {
			blade.isLoading = false;
			blade.currentEntity = data;
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.updateTemplate = function () {
		blade.isLoading = false;
		notifications.updateTemplate({}, blade.currentEntity, function () {

		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.testResolve = function () {
		var 

		var newBlade = {
			id: 'testResolve',
			title: 'Edit notification template',
			currentEntity: data,
			controller: 'platformWebApp.editTemplateController',
			template: 'Scripts/app/newnotifications/blades/notifications-test-resolve.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.testSend = function () {

	}

	blade.initialize();
}]);
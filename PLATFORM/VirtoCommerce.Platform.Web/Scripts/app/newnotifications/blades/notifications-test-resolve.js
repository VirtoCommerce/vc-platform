angular.module('platformWebApp')
.controller('platformWebApp.testResolveController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.newnotifications', function ($rootScope, $scope, bladeNavigationService, dialogService, notifications) {
	var blade = $scope.blade;

	blade.initialize = function () {
		blade.isLoading = true;
		blade.isRender = false;

		notifications.prepareTestData({ type: blade.notificationType }, function (data) {
			blade.isLoading = false;

			blade.currentParams = data;
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.test = function () {
		blade.isLoading = true;
		blade.params = [];
		for (var i = 0; i < blade.currentParams.length; i++) {
			blade.params.push({ key: blade.currentParams[i], value: blade.obj[blade.currentParams[i]] });
		}

		notifications.resolveNotification({ type: blade.notificationType }, blade.params, function (notification) {
			blade.isLoading = false;

			var newBlade = {
				id: 'resolveResult',
				title: 'Result of resolving template',
				notification: notification,
				controller: 'platformWebApp.resolveResultController',
				template: 'Scripts/app/newnotifications/blades/resolve-result.tpl.html'
			};

			bladeNavigationService.showBlade(newBlade, blade);
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.headIcon = 'fa-play';

	blade.initialize();
}]);
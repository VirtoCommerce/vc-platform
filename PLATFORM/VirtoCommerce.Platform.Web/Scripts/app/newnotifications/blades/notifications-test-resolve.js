angular.module('platformWebApp')
.controller('platformWebApp.testResolveController', ['$rootScope', '$scope', '$localStorage', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.newnotifications', function ($rootScope, $scope, $localStorage, bladeNavigationService, dialogService, notifications) {
	var blade = $scope.blade;

	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

	blade.initialize = function () {
		blade.isLoading = true;
		blade.isRender = false;

		notifications.prepareTestData({ type: blade.notificationType }, function (data) {
			blade.isLoading = false;

			blade.currentParams = data;
			if (!angular.isUndefined($localStorage.notificationTestResolve) && $localStorage.notificationTestResolve.length > 0) {
				blade.obj = {};
				for (var i = 0; i < $localStorage.notificationTestResolve.length; i++) {
					blade.obj[$localStorage.notificationTestResolve[i].key] = $localStorage.notificationTestResolve[i].value;
				}
			}
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.test = function () {
		blade.isLoading = true;
		blade.params = [];
		for (var i = 0; i < blade.currentParams.length; i++) {
			blade.params.push({ key: blade.currentParams[i].parameterName, value: blade.obj[blade.currentParams[i].parameterName] });
		}

		$localStorage.notificationTestResolve = blade.params;

		notifications.resolveNotification({ type: blade.notificationType, objectId: blade.objectId, objectTypeId: blade.objectTypeId, language: blade.language }, blade.params, function (notification) {
			blade.isLoading = false;

			var newBlade = {
				id: 'resolveResult',
				title: 'Preview result',
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
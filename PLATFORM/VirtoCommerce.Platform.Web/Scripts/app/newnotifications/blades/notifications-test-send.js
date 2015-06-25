angular.module('platformWebApp')
.controller('platformWebApp.testSendController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.newnotifications', function ($rootScope, $scope, bladeNavigationService, dialogService, notifications) {
	var blade = $scope.blade;
	blade.sendingInfo = ['Sender', 'Recipient'];

	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

	blade.initialize = function () {
		blade.isLoading = true;

		notifications.prepareTestData({ type: blade.notificationType }, function (data) {
			blade.isLoading = false;

			blade.currentParams = data;
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.send = function () {
		blade.isLoading = true;
		blade.params = [];
		for (var i = 0; i < blade.currentParams.length; i++) {
			blade.params.push({ key: blade.currentParams[i], value: blade.obj[blade.currentParams[i]] });
		}
		for (var i = 0; i < blade.sendingInfo.length; i++) {
			blade.params.push({ key: blade.sendingInfo[i], value: blade.obj[blade.sendingInfo[i]] });
		}


		notifications.sendNotification({ type: blade.notificationType }, blade.params, function () {
			blade.isLoading = false;
			var dialog = {
				id: "successSend",
				title: "Sending success",
				message: "Email was send successfully!",
				callback: function (remove) {

				}
			}
			dialogService.showNotificationDialog(dialog);

		}, function (error) {
			blade.isLoading = false;
			var dialog = {
				id: "errorSend",
				title: "Error in sending",
				message: "Email wasn't send!",
				callback: function (remove) {

				}
			}
			dialogService.showNotificationDialog(dialog);
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	}

	blade.headIcon = 'fa-upload';

	blade.initialize();
}]);
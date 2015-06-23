angular.module('platformWebApp')
.controller('platformWebApp.testSendController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.newnotifications', function ($rootScope, $scope, bladeNavigationService, dialogService, notifications) {
	var blade = $scope.blade;

	blade.initialize = function () {
		blade.isLoading = true;

		notifications.prepareTestData({ type: blade.notificationType }, function (data) {
			blade.isLoading = false;

			blade.currentParams = data;
			blade.currentParams.push('Sender');
			blade.currentParams.push('Recipient');
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

		notifications.sendNotification({ type: blade.notificationType }, blade.params, function (errorMessage) {
			blade.isLoading = false;
			if (angular.isUndefined(errorMessage) || errorMessage === null || errorMessage === "") {
				var dialog = {
					id: "successSend",
					title: "Sending success",
					message: "Email was send successfully!",
					callback: function (remove) {

					}
				}
				dialogService.showNotificationDialog(dialog);
			}
			else {
				var dialog = {
					id: "errorSend",
					title: "Error in sending",
					message: errorMessage,
					callback: function (remove) {

					}
				}
				dialogService.showNotificationDialog(dialog);
				bladeNavigationService.setError('Error ' + error.status, blade);
			}
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
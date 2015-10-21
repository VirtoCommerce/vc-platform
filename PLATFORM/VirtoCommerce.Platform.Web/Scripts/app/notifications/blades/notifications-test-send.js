angular.module('platformWebApp')
.controller('platformWebApp.testSendController', ['$rootScope', '$scope', '$localStorage', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.notifications', function ($rootScope, $scope, $localStorage, bladeNavigationService, dialogService, notifications) {
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
		    if (!angular.isUndefined($localStorage.notificationTestSend)) {
		        blade.obj = { notificationParameters: {} };
		        blade.obj.notificationParameters['Sender'] = $localStorage.notificationTestSend['Sender'];
		        blade.obj.notificationParameters['Recipient'] = $localStorage.notificationTestSend['Recipient'];
		        for (var i = 0; i < blade.currentParams.length; i++) {
		            if (blade.currentParams[i].isDictionary) {
		                blade.obj.notificationParameters[blade.currentParams[i].parameterName] = [{ name: '', value: '' }];
		            }
		            else {
		                blade.obj.notificationParameters[blade.currentParams[i].parameterName] = $localStorage.notificationTestSend[blade.currentParams[i].parameterName];
		            }
		        }
		    }
		}, function (error) {
		    bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.send = function () {
		blade.isLoading = true;
		
		$localStorage.notificationTestSend = blade.obj.notificationParameters;
		blade.obj["Type"] = blade.notificationType;
		blade.obj["ObjectId"] = blade.objectId;
		blade.obj["ObjectTypeId"] = blade.objectTypeId;
		blade.obj["Language"] = blade.language;

	    //prepare params for sending
		var params = {};
		var preparedParams = {};

		for (var i = 0; i < blade.currentParams.length; i++) {
		    if (blade.currentParams[i].isDictionary) {
		        params = {};
		        preparedParams[blade.currentParams[i].parameterName] = blade.obj.notificationParameters[blade.currentParams[i].parameterName];
		        var notParam = blade.obj.notificationParameters[blade.currentParams[i].parameterName];
		        for (var j = 0; j < notParam.length; j++) {
		            params[notParam[j].name] = notParam[j].value;
		        }
		        blade.obj.notificationParameters[blade.currentParams[i].parameterName] = params;
		    }
		}

		notifications.sendNotification({}, blade.obj, function () {
			blade.isLoading = false;
			var dialog = {
				id: "successSend",
				title: "Sending success",
				message: "Email was send successfully!",
				callback: function (remove) {

				}
			}
			dialogService.showNotificationDialog(dialog);

		    //revert params
			blade.revertParams(preparedParams);

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

		    //revert params
			blade.revertParams(preparedParams);
		});
	}

	blade.headIcon = 'fa-upload';

	blade.initialize();

	blade.revertParams = function () {
	    for (var i = 0; i < blade.currentParams.length; i++) {
	        if (blade.currentParams[i].isDictionary) {
	            blade.obj.notificationParameters[blade.currentParams[i].parameterName] = preparedParams[blade.currentParams[i].parameterName];
	        }
	    }
	}

	blade.add = function (paramName) {
	    blade.obj.notificationParameters[paramName].push({ name: '', value: '' });
	}
}]);
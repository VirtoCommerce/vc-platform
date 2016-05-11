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

		    blade.obj = {
		        notificationParameters: [{
		            parameterName: "Recipient",
		            parameterDescription: "Recipient email",
		            isDictionary: false,
		            isArray: false,
		            type: "String"
		        },
		        {
		            parameterName: "Sender",
		            parameterDescription: "Sender email",
		            isDictionary: false,
		            isArray: false,
		            type: "String"
		        }]
		    };
		    for (var i = 0; i < blade.currentParams.length; i++) {
		        var property = blade.currentParams[i];
		        if (property.isDictionary) {
		            blade.currentParams[i].value = [{ name: '', value: '' }];
		            blade.obj.notificationParameters.push(blade.currentParams[i]);
		        }
		        else if (property.isArray) {
		            if (property.type === 'Decimal')
		                blade.currentParams[i].value = [{ key: '0.00' }];
		            else if (property.type === 'Integer')
		                blade.currentParams[i].value = [{ key: '0' }];
		            else if (property.type === 'DateTime')
		                blade.currentParams[i].value = [{ key: undefined }];
		            else if (property.type === 'Boolean')
		                blade.currentParams[i].value = [{ key: false }];
		            else if (property.type === 'String')
		                blade.currentParams[i].value = [{ key: '' }];
		            blade.obj.notificationParameters.push(blade.currentParams[i]);
		        }
		        else {
		            if (!_.isEmpty($localStorage.notificationTestSend)) {
		                var value = _.find($localStorage.notificationTestSend, function (element) { return blade.currentParams[i].parameterName === element.parameterName; });
		                if (value) {
		                    blade.currentParams[i].value = value.value;
		                }
		                else {
		                    blade.obj.notificationParameters.push(blade.currentParams[i]);
		                }
		            }
		            else {
		                blade.obj.notificationParameters.push(blade.currentParams[i]);
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
		var preparedParams = [];
		for (var i = 0; i < blade.obj.notificationParameters.length; i++) {
		    if (blade.obj.notificationParameters[i].isDictionary) {
		        var params = {};
		        preparedParams[i] = angular.copy(blade.obj.notificationParameters[i]);
		        var notParam = blade.obj.notificationParameters[i].value;
		        for (var j = 0; j < notParam.length; j++) {
		            params[notParam[j].name] = notParam[j].value;
		        }
		        blade.obj.notificationParameters[i].value = params;
		    }
		    else if (blade.obj.notificationParameters[i].isArray) {
		        var arrayParams = [];
		        preparedParams[i] = angular.copy(blade.obj.notificationParameters[i]);
		        var notParam = blade.obj.notificationParameters[i].value;
		        for (var j = 0; j < notParam.length; j++) {
		            arrayParams.push(notParam[j].key);
		        }
		        blade.obj.notificationParameters[i].value = arrayParams;
		    }
		}

		notifications.sendNotification({}, blade.obj, function () {
			blade.isLoading = false;
			var dialog = {
				id: "successSend",
				title: "platform.dialogs.email-sending-success.title",
				message: "platform.dialogs.email-sending-success.message",
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
				title: "platform.dialogs.email-send-error.title",
				message: "platform.dialogs.email-send-error.message",
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

	blade.revertParams = function (preparedParams) {
	    for (var i = 0; i < blade.obj.notificationParameters.length; i++) {
	        if (blade.obj.notificationParameters[i].isDictionary || blade.obj.notificationParameters[i].isArray) {
	            blade.obj.notificationParameters[i].value = preparedParams[i].value;
	        }
	    }
	}

	$scope.datepickers = {
	    endDate: false,
	    startDate: false,
	}
	$scope.today = new Date();

	$scope.open = function ($event, parameterName) {
	    $event.preventDefault();
	    $event.stopPropagation();

	    $scope.datepickers[parameterName] = true;
	};

	$scope.dateOptions = {
	    'year-format': "'yyyy'",
	    'starting-day': 1
	};

	$scope.formats = ['shortDate', 'dd-MMMM-yyyy', 'yyyy/MM/dd'];
	$scope.format = $scope.formats[0];

	blade.addDictionaryElement = function (paramName) {
	    var value = _.find(blade.obj.notificationParameters, function (element) { return paramName === element.parameterName; });
	    value.push({ name: '', value: '' });
	}

	blade.addArrayElement = function (parameter) {
	    var value = _.find(blade.obj.notificationParameters, function (element) { return parameter.parameterName === element.parameterName; });
	    if (value) {
	        if (value.type === 'Decimal')
	            value.value.push({ key: '0.00' });
	        else if (value.type === 'Integer')
	            value.value.push({ key: '0' });
	        else if (value.type === 'DateTime')
	            value.value.push({ key: undefined });
	        else if (value.type === 'Boolean')
	            value.value.push({ key: false });
	        else if (value.type === 'String')
	            value.value.push({ key: '' });
	    }
	}
}]);
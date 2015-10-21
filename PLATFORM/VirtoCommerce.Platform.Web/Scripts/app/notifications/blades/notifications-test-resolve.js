angular.module('platformWebApp')
.controller('platformWebApp.testResolveController', ['$rootScope', '$scope', '$localStorage', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.notifications', function ($rootScope, $scope, $localStorage, bladeNavigationService, dialogService, notifications) {
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
			if (!angular.isUndefined($localStorage.notificationTestResolve)) {
			    blade.obj = { notificationParameters: {} };
			    for (var i = 0; i < blade.currentParams.length; i++) {
			        if (blade.currentParams[i].isDictionary) {
			            blade.obj.notificationParameters[blade.currentParams[i].parameterName] = [ { name: '', value: '' } ];
			        }
			        else {
			            blade.obj.notificationParameters[blade.currentParams[i].parameterName] = $localStorage.notificationTestResolve[blade.currentParams[i].parameterName];
			        }
				}
			}
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.test = function () {
		blade.isLoading = true;

		$localStorage.notificationTestResolve = blade.obj.notificationParameters;
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

        //send params
	    notifications.resolveNotification({}, blade.obj, function (notification) {
			blade.isLoading = false;

			var newBlade = {
				id: 'resolveResult',
				title: 'Preview result',
				notification: notification,
				controller: 'platformWebApp.resolveResultController',
				template: '$(Platform)/Scripts/app/notifications/blades/resolve-result.tpl.html'
			};

			bladeNavigationService.showBlade(newBlade, blade);

            //revert params
			blade.revertParams(preparedParams);
		}, function (error) {
		    bladeNavigationService.setError('Error ' + error.status, blade);
		    blade.revertParams(preparedParams);
		});
	};

	blade.add = function (paramName) {
	    blade.obj.notificationParameters[paramName].push({ name: '', value: '' });
	}

	blade.headIcon = 'fa-play';

	blade.revertParams = function () {
	    for (var i = 0; i < blade.currentParams.length; i++) {
	        if (blade.currentParams[i].isDictionary) {
	            blade.obj.notificationParameters[blade.currentParams[i].parameterName] = preparedParams[blade.currentParams[i].parameterName];
	        }
	    }
	}

	blade.initialize();
}]);
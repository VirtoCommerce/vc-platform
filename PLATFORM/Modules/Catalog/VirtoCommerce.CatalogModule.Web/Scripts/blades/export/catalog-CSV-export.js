angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogCSVexportController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.export', 'platformWebApp.notifications', function ($scope, bladeNavigationService, exportResourse, notificationsResource) {
	var blade = $scope.blade;
	blade.isLoading = false;

    $scope.$on("new-notification-event", function (event, notification) {
    	if (blade.notification && notification.id == blade.notification.id)
    	{
    		angular.copy(notification, blade.notification);
    	}
    });

    $scope.startExport = function () {
     	exportResourse.run({ id: blade.catalog.id }, function (data) {
     		blade.notification = data;
     	}, function (error) {
     		bladeNavigationService.setError('Error ' + error.status, $scope.blade);
     	});
	}

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }
  
    $scope.bladeHeadIco = 'fa fa-file-archive-o';
}]);

angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogCSVimportController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.import', 'platformWebApp.notifications', function ($scope, bladeNavigationService, importResourse, notificationsResource) {
	var blade = $scope.blade;
	blade.isLoading = false;
	blade.title = 'Importing from csv';

    $scope.$on("new-notification-event", function (event, notification) {
    	if (blade.notification && notification.id == blade.notification.id)
    	{
    		angular.copy(notification, blade.notification);
    	}
    });

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }
  
    $scope.bladeHeadIco = 'fa fa-file-archive-o';

  
}]);

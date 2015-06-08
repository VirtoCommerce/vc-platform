angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogCSVexportController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.export', 'platformWebApp.notifications', function ($scope, bladeNavigationService, exportResourse, notificationsResource) {
	var blade = $scope.blade;
	blade.isLoading = false;
	blade.title = 'Catalog ' + (blade.catalog ? blade.catalog.name : '') + ' to csv export';

    $scope.$on("new-notification-event", function (event, notification) {
    	if (blade.notification && notification.id == blade.notification.id)
    	{
    		angular.copy(notification, blade.notification);
    	}
    });

    $scope.startExport = function () {
    	exportResourse.run({
    		catalogId: blade.catalog.id,
    		categoryIds: _.map(blade.selectedCategories, function (x) { return x.id }),
    		productIds: _.map(blade.selectedProducts, function (x) { return x.id })
    	}, function (data) { blade.notification = data; },
						   function (error) {
						   	bladeNavigationService.setError('Error ' + error.status, $scope.blade);
						   });
    }

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }
  
    $scope.blade.headIcon = 'fa fa-file-archive-o';
}]);

angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeExportController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.storeModule.export', function ($scope, bladeNavigationService, exportResource) {
    var blade = $scope.blade;
    blade.isLoading = false;
    blade.title = 'Export' + (blade.store ? blade.store.name : '');
    $scope.blade.headIcon = 'fa fa-file-archive-o';

    $scope.$on("new-notification-event", function (event, notification) {
        if (blade.notification && notification.id == blade.notification.id) {
            angular.copy(notification, blade.notification);
            if (notification.finished) {
                blade.isLoading = false;
            }
        }
    });

    $scope.startExport = function () {
        blade.isLoading = true;
        exportResource.run(
            { storeId: blade.store.id },
            function (data) { blade.notification = data; },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

}]);

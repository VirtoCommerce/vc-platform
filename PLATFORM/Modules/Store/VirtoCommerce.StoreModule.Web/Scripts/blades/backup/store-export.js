angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeExportController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.storeModule.export', function ($scope, bladeNavigationService, exportResource) {
    var blade = $scope.blade;
    blade.isLoading = false;
    blade.title = 'Export' + (blade.store ? blade.store.name : '');

    $scope.$on("new-notification-event", function (event, notification) {
        if (blade.notification && notification.id == blade.notification.id) {
            angular.copy(notification, blade.notification);
        }
    });

    $scope.startExport = function () {
        blade.isLoading = true;
        exportResource.run(
            { storeId: blade.store.id },
            function (data) { blade.isLoading = false; blade.notification = data; },
            function (error) { blade.isLoading = false; bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.blade.headIcon = 'fa fa-file-archive-o';
}]);

angular.module('platformWebApp')
.controller('platformWebApp.exportImport.exportMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.export', 'platformWebApp.notifications', function ($scope, bladeNavigationService, exportResourse, notifications) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-upload';
    blade.title = 'Data export';

    function initializeBlade() {
        //exportResourse.getExporters({ id: blade.currentEntityId }, function (data) {

        blade.currentEntities = [{ name: 'data 1', description: 'some data exporter description' },
            { name: 'data 1' },
            { name: 'data 1', description: 'some data exporter description' },
            { name: 'data 1', description: 'some data exporter description' },
            { name: 'data 1' },
            { name: 'data 1', description: 'some data exporter description' }
        ];
        blade.isLoading = false;
        //},
        //   function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    //$scope.setForm = function (form) {
    //    $scope.formScope = form;
    //}

    $scope.$on("new-notification-event", function (event, notification) {
        if (blade.notification && notification.id == blade.notification.id) {
            angular.copy(notification, blade.notification);
        }
    });

    $scope.startExport = function () {
        exportResourse.run({
            categoryIds: _.map(blade.selectedCategories, function (x) { return x.id })
        },
        function (data) { blade.notification = data; },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }


    initializeBlade();
}]);

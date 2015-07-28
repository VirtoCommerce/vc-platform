angular.module('platformWebApp')
.controller('platformWebApp.exportImport.exportMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.exportImport.resource', function ($scope, bladeNavigationService, exportImportResourse) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-upload';
    blade.title = 'Data export';

    function initializeBlade() {
        exportImportResourse.getExportersList(function (data) {
            blade.currentEntities = data;
            blade.isLoading = false;
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    //$scope.setForm = function (form) {
    //    $scope.formScope = form;
    //}

    $scope.$on("new-notification-event", function (event, notification) {
        if (blade.notification && notification.id == blade.notification.id) {
            angular.copy(notification, blade.notification);
        }
    });

    $scope.canStartProcess = function () {
        return blade.currentEntities && _.some(blade.currentEntities, function (x) { return x.isChecked; });
    }

    $scope.startExport = function () {
        blade.isLoading = true;
        var selection = _.where(blade.currentEntities, { isChecked: true });
        exportImportResourse.runExport({
            modules: _.pluck(selection, 'id')
        },
        function (data) { blade.notification = data; blade.isLoading = false; },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }


    initializeBlade();
}]);

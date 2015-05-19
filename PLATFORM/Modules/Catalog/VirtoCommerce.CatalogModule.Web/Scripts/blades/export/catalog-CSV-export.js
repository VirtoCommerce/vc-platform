angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogCSVexportController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    function initializeBlade() {
        blade.currentEntity = { catalogId: blade.catalogId };
        blade.isLoading = false;
    };

    $scope.startProcess = function () {
        console.log('Starting Catalog export: ' + angular.toJson(blade.currentEntity, true));
    }

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.bladeHeadIco = 'fa fa-file-archive-o';

    initializeBlade();
}]);

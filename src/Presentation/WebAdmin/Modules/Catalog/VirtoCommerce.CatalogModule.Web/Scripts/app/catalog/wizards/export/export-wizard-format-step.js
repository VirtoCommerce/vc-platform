angular.module('catalogModule.wizards.exportWizard')
.controller('exportFormatController', ['$scope', function ($scope) {
    $scope.blade.refresh = function () {
        $scope.selectedImporter = $scope.blade.parentBlade.item.entityImporter;
        $scope.blade.isLoading = false;
        $scope.importers = [
            "Csv"
        ];

    };

    $scope.setImporter = function (importer) {
        $scope.selectedImporter = importer;
        $scope.blade.parentBlade.item.entityImporter = importer;
        $scope.bladeClose();
    }

    $scope.blade.refresh();

}]);

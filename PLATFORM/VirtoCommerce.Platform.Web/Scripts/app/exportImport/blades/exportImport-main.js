angular.module('platformWebApp')
.controller('platformWebApp.exportImport.mainController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
   
    $scope.export = function () {
        $scope.selectedNodeId = 'export';

        var newBlade = {
            controller: 'platformWebApp.exportImport.exportMainController',
            template: 'Scripts/app/exportImport/blades/export-main.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.import = function () {
        $scope.selectedNodeId = 'import';

    };

    $scope.blade.headIcon = 'fa-database';
    $scope.blade.isLoading = false;
}]);

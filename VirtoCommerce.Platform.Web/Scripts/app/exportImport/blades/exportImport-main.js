angular.module('platformWebApp')
.controller('platformWebApp.exportImport.mainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.authService', function ($scope, bladeNavigationService, authService) {

    $scope.export = function () {
        $scope.selectedNodeId = 'export';

        var newBlade = {
            controller: 'platformWebApp.exportImport.exportMainController',
            template: '$(Platform)/Scripts/app/exportImport/blades/export-main.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.import = function () {
        if (authService.checkPermission('platform:exportImport:import')) {
            $scope.selectedNodeId = 'import';

            var newBlade = {
                controller: 'platformWebApp.exportImport.importMainController',
                template: '$(Platform)/Scripts/app/exportImport/blades/import-main.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade);
        }
    };

    $scope.blade.headIcon = 'fa-database';
    $scope.blade.isLoading = false;
}]);

angular.module('platformWebApp')
.controller('platformWebApp.changeLog.operationListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    
    $scope.blade.isLoading = false;
    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        $scope.gridOptions = gridOptions;
    };
}]);

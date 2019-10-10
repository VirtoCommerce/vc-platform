angular.module('platformWebApp').controller('platformWebApp.changeLog.operationListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.changeLogApi', function ($scope, bladeNavigationService, changeLogApi) {
    $scope.blade.isLoading = false;

    if (!$scope.blade.currentEntities) {
        $scope.blade.isLoading = true;
        var objectType = $scope.blade.tenantType ? $scope.blade.tenantType.split('.').pop() : '';
        changeLogApi.search({
            objectIds: [$scope.blade.tenantId],
            objectType: objectType,
            take: 50
        }, function (data) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = data;
        });
    }

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        $scope.gridOptions = gridOptions;
    };
}]);

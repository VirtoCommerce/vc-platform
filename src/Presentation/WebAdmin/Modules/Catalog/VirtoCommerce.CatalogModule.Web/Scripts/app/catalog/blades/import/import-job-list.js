angular.module('catalogModule.blades.import.importJobList', ['catalogModule.resources.import'])
.controller('importJobListController', ['$scope', 'bladeNavigationService', 'imports', function ($scope, bladeNavigationService, imports)
{
    $scope.selectedAll = false;
    $scope.selectedItem = null;

    $scope.blade.refresh = function ()
    {
        $scope.blade.isLoading = true;

        var result = imports.list({catalogId : $scope.blade.catalogId}, function (results)
        {
            $scope.blade.isLoading = false;
            $scope.items = results;

        });

    };

    $scope.checkAll = function (selected)
    {
        angular.forEach($scope.items, function (item)
        {
            item.selected = selected;
        });
    };

    $scope.blade.refresh();

}]);

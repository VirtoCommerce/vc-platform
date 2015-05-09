angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.importJobMappingEditController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService)
{
    function initializeBlade(data)
    {
        $scope.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    $scope.saveChanges = function ()
    {
        var idx = $scope.blade.parentBlade.item.propertiesMap.indexOf($scope.blade.origEntity);
        $scope.blade.parentBlade.item.propertiesMap.splice(idx, idx < 0 ? 0 : 1, $scope.currentEntity);

        $scope.bladeClose();
    };

    $scope.setForm = function (form)
    {
        $scope.formScope = form;
    }

    initializeBlade($scope.blade.item);

}]);



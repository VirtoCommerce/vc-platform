angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.importJobSettingsController', ['$scope', function ($scope)
{
    $scope.blade.isLoading = false;
    $scope.item = angular.copy($scope.blade.item);

    $scope.columnDelimiters = [
        { name: "Auto", value: "?" },
        { name: "Comma", value: "," },
        { name: "Semicolon", value: ";" },
        { name: "Tab", value: "\t" }
    ];

    $scope.saveChanges = function ()
    {
        $scope.blade.parentBlade.item.maxErrorsCount = $scope.item.maxErrorsCount;
        $scope.blade.parentBlade.item.importStep = $scope.item.importStep;
        $scope.blade.parentBlade.item.importCount = $scope.item.importCount;
        $scope.blade.parentBlade.item.startIndex = $scope.item.startIndex;
        $scope.blade.parentBlade.item.columnDelimiter = $scope.item.columnDelimiter;
        $scope.bladeClose();
    };

    $scope.setForm = function (form)
    {
        $scope.formScope = form;
    }

}]);



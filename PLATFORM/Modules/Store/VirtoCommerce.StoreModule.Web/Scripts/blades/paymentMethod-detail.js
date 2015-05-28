angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.paymentMethodDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, dialogService) {

    function initializeBlade(data) {
        $scope.blade.currentEntityId = data.id;
        $scope.blade.title = data.name;

        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.saveChanges = function () {
        angular.copy($scope.blade.currentEntity, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.blade.headIcon = 'fa fa-archive';
    $scope.blade.toolbarCustomTemplates = ['Modules/$(VirtoCommerce.Store)/Scripts/blades/toolbar-isActive.tpl.html'];

    $scope.blade.toolbarCommands = [
        {
            name: "Reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'store:manage'
        }
    ];


    initializeBlade($scope.blade.origEntity);
}]);
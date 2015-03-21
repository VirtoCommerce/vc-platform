angular.module('virtoCommerce.catalogModule')
.controller('associationGroupSelectController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.blade.refresh = function () {
        $scope.selectedId = $scope.blade.parentBlade.groupName;
        $scope.blade.isLoading = false;
    };

    $scope.openBladeNewGroup = function () {
        $scope.blade.isNewGroup = true;

        var newBlade = {
            id: "associationGroupNew",
            title: 'Create Association Group',
            controller: 'associationGroupNewController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/newAssociation/association-wizard-group-new.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.setSelected = function (data) {
        $scope.blade.parentBlade.groupName = data;
        $scope.bladeClose();
    }

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.blade.refresh();
}]);

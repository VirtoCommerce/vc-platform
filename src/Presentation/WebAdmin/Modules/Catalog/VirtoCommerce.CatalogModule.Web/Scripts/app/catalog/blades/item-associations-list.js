angular.module('catalogModule.blades.itemAssociationsList', [])
.controller('itemAssociationsListController', ['$rootScope', '$scope', 'bladeNavigationService', 'items', function ($rootScope, $scope, bladeNavigationService, items) {

    $scope.blade.refresh = function (parentRefresh) {
        if (parentRefresh) {
            $scope.blade.isLoading = true;
            $scope.blade.parentBlade.refresh().$promise.then(function (data) {
                initializeBlade(data.associations);
            });
        } else {
            initializeBlade($scope.blade.currentEntities);
        }
    }

    function initializeBlade(data) {
        $scope.blade.currentEntities = angular.copy(data);
        $scope.blade.origItem = data;
        $scope.blade.isLoading = false;
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    function openAddEntityWizard() {
        var newBlade = {
            id: "associationWizard",
            title: "New Associations",
            //subtitle: '',
            controller: 'associationWizardController',
            bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newAssociation/association-wizard-actions.tpl.html',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newAssociation/association-wizard.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Add", icon: 'icon-plus',
            executeMethod: function () {
                openAddEntityWizard();
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    $scope.blade.refresh(false);

    // open blade for new item 
    if (!_.some($scope.blade.currentEntities)) {
        openAddEntityWizard();
    }

}]);

angular.module('catalogModule.widget.itemAssociationsWidget', [])
.controller('itemAssociationsWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.openBlade = function () {
        if ($scope.currentBlade.item.associations) {
            var newBlade;
            if (_.any($scope.currentBlade.item.associations)) {
                newBlade = {
                    id: "associationsList",
                    currentEntityId: $scope.currentBlade.currentEntityId,
                    currentEntities: $scope.currentBlade.item.associations,
                    title: $scope.currentBlade.title,
                    subtitle: 'Associations',
                    controller: 'itemAssociationsListController',
                    template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-associations-list.tpl.html'
                };
            } else {
                var newBlade = {
                    id: "associationWizard",
                    currentEntities: $scope.currentBlade.item.associations,
                    title: "New Associations",
                    //subtitle: '',
                    controller: 'associationWizardController',
                    template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newAssociation/association-wizard.tpl.html'
                };
            }

            bladeNavigationService.showBlade(newBlade, $scope.currentBlade);
        }
    };
}]);

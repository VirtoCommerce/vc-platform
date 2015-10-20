angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemAssociationsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
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
                    controller: 'virtoCommerce.catalogModule.itemAssociationsListController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-associations-list.tpl.html'
                };
            } else {
                var newBlade = {
                    id: "associationWizard",
                    associations: $scope.currentBlade.item.associations,
                    controller: 'virtoCommerce.catalogModule.associationWizardController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/newAssociation/association-wizard.tpl.html'
                };
            }

            bladeNavigationService.showBlade(newBlade, $scope.currentBlade);
        }
    };
}]);

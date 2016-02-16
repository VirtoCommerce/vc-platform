angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemAssociationsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.openBlade = function () {
        if (blade.item.associations) {
            var newBlade;
            if (_.any(blade.item.associations) || !blade.hasUpdatePermission()) {
                newBlade = {
                    id: "associationsList",
                    currentEntityId: blade.currentEntityId,
                    currentEntities: blade.item.associations,
                    title: blade.title,
                    subtitle: 'catalog.widgets.itemAssociations.blade-subtitle',
                    controller: 'virtoCommerce.catalogModule.itemAssociationsListController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-associations-list.tpl.html'
                };
            } else {
                var newBlade = {
                    id: "associationWizard",
                    associations: blade.item.associations,
                    controller: 'virtoCommerce.catalogModule.associationWizardController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/newAssociation/association-wizard.tpl.html'
                };
            }

            bladeNavigationService.showBlade(newBlade, blade);
        }
    };
}]);

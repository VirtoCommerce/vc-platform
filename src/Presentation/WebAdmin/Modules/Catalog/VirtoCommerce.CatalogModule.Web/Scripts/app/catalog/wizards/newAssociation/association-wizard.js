angular.module('catalogModule.wizards.associationWizard', [
'catalogModule.wizards.associationWizard.associationGroup'
])
.controller('associationWizardController', ['$scope', 'bladeNavigationService', 'dialogService', 'items', function ($scope, bladeNavigationService, dialogService, items) {

    $scope.create = function () {
        $scope.blade.isLoading = true;
        var entriesCopy = $scope.blade.parentBlade.currentEntities.slice();

        _.each($scope.selection, function (item) {
            if (_.every(entriesCopy, function (x) { return x.productId != item.id; })) {
                var newEntry = {
                    name: $scope.blade.groupName,
                    productId: item.id
                };
                entriesCopy.push(newEntry);
            }
        });

        items.updateitem({ id: $scope.blade.parentBlade.currentEntityId, associations: entriesCopy }, function () {
            $scope.bladeClose();
            $scope.blade.parentBlade.refresh(true);
        });
    }

    $scope.openBlade = function (type) {
        $scope.blade.onClose(function () {
            var newBlade = null;
            switch (type) {
                case 'group':
                    newBlade = {
                        id: "associationGroup",
                        title: 'Association Group',
                        controller: 'associationGroupSelectController',
                        bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/common/wizard-ok-action.tpl.html',
                        template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newAssociation/association-wizard-group-step.tpl.html'
                    };
                    break;
                case 'products':
                    newBlade = {
                        id: 'selectCatalog',
                        title: 'Select Catalog',
                        subtitle: 'Adding Associations to product',
                        mode: 'newAssociation',
                        childTitle: 'Choose Items to associate',
                        controller: 'catalogsSelectController',
                        bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/common/wizard-ok-action.tpl.html',
                        template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/catalogs-select.tpl.html'
                    };

                    break;
            }

            if (newBlade != null) {
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            }
        });
    }

    $scope.blade.select = function (selection) {
        _.each(selection, function (item) {
            if (_.every($scope.selection, function (x) { return x.id != item.id; })) {
                $scope.selection.push(item);
            }
        });
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

    $scope.selection = [];
    $scope.blade.isLoading = false;
}]);



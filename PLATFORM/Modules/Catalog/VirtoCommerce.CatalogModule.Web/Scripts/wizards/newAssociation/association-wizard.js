angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.associationWizardController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'virtoCommerce.catalogModule.items', function ($scope, bladeNavigationService, settings, items) {
    var blade = $scope.blade;
    blade.title = "catalog.wizards.association.title";

    $scope.create = function () {
        blade.isLoading = true;
        var entriesCopy = blade.associations.slice();

        _.each(blade.selection, function (id) {
            if (_.every(entriesCopy, function (x) { return x.productId != id; })) {
                var newEntry = {
                    name: blade.groupName,
                    productId: id
                };
                entriesCopy.push(newEntry);
            }
        });

        items.update({ id: blade.parentBlade.currentEntityId, associations: entriesCopy }, function () {
            $scope.bladeClose();
            blade.parentBlade.refresh();
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    $scope.openBlade = function () {
        var selection = [];
        var options = {
            selectedItemIds: _.union(blade.selection, _.pluck(blade.associations, 'productId')),
            checkItemFn: function (listItem, isSelected) {
                if (isSelected) {
                    if (_.all(selection, function (x) { return x.id != listItem.id; })) {
                        selection.push(listItem);
                    }
                }
                else {
                    selection = _.reject(selection, function (x) { return x.id == listItem.id; });
                }
            }
        };
        var newBlade = {
            id: "CatalogItemsSelect",
            title: "catalog.blades.catalog-items-select.title-association",
            subtitle: 'catalog.blades.catalog-items-select.subtitle-association',
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            options: options,
            breadcrumbs: [],
            toolbarCommands: [
              {
                  name: "platform.commands.confirm", icon: 'fa fa-check',
                  executeMethod: function (pickingBlade) {
                      blade.selection = _.union(blade.selection, _.pluck(selection, 'id'));
                      bladeNavigationService.closeBlade(pickingBlade);
                  },
                  canExecuteMethod: function () {
                      return _.any(selection);
                  }
              }]
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }


    $scope.openDictionarySettingManagement = function () {
        var newBlade = {
            id: 'settingDetailChild',
            isApiSave: true,
            currentEntityId: 'Catalog.AssociationGroups',
            parentRefresh: function (data) { $scope.associationGroups = data; },
            controller: 'platformWebApp.settingDictionaryController',
            template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.associationGroups = settings.getValues({ id: 'Catalog.AssociationGroups' });

    blade.selection = [];
    blade.isLoading = false;
}]);
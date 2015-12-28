﻿angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.newCategoryWizardController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.catalogModule.categories', function ($scope, bladeNavigationService, dialogService, categories) {
    var blade = $scope.blade;

    $scope.create = function () {
        blade.isLoading = true;

        blade.currentEntity.$update(null, function (data) {
            $scope.bladeClose(function () {
                var categoryListBlade = blade.parentBlade;
                categoryListBlade.setSelectedItem(data);
                categoryListBlade.showCategoryBlade(data);
                categoryListBlade.refresh();
            });
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    $scope.openBlade = function (type) {
        blade.onClose(function () {
            var newBlade = null;
            switch (type) {
                case 'properties':
                    newBlade = {
                        id: "categoryPropertyDetail",
                        currentEntityId: blade.currentEntityId,
                        currentEntity: blade.currentEntity,
                        title: blade.title,
                        subtitle: 'catalog.blades.category-property-detail.title',
                        controller: 'virtoCommerce.catalogModule.categoryPropertyListController',
                        //isNew: true,
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/category-property-list.tpl.html'
                    };
                    break;
                case 'seo':
                    newBlade = {
                        id: "seoDetail",
                        seoUrlKeywordType: 0,
                        parentEntity: blade.currentEntity,
                        title: blade.title,
                        controller: 'virtoCommerce.catalogModule.seoDetailController',
                        //seoInfos: blade.item.seoInfos,
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/seo-detail.tpl.html'
                    };
                    break;
            }

            if (newBlade != null) {
                bladeNavigationService.showBlade(newBlade, blade);
            }
        });
    }

    $scope.codeValidator = function (value) {
        var pattern = /[$+;=%{}[\]|\\\/@ ~!^*&()?:'<>,]/;
        return !pattern.test(value);
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }


    blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    blade.isLoading = false;
}]);

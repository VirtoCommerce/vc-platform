﻿angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.newCategoryWizardController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.catalogModule.categories', function ($scope, bladeNavigationService, dialogService, categories) {
    $scope.create = function () {
        $scope.blade.currentEntity.$update(null, function (data) {
            $scope.bladeClose(function () {
                var categoryListBlade = $scope.blade.parentBlade;
                categoryListBlade.setSelectedItem(data);
                categoryListBlade.showCategoryBlade(data);
                categoryListBlade.refresh();
            });
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    $scope.openBlade = function (type) {
        $scope.blade.onClose(function () {
            var newBlade = null;
            switch (type) {
                case 'properties':
                    newBlade = {
                        id: "categoryPropertyDetail",
                        currentEntityId: $scope.blade.currentEntityId,
                        currentEntity: $scope.blade.currentEntity,
                        title: $scope.blade.title,
                        subtitle: 'catalog.blades.category-properties.title',
                        controller: 'virtoCommerce.catalogModule.categoryPropertyController',
                        //isNew: true,
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/category-property-detail.tpl.html'
                    };
                    break;
                case 'seo':
                    newBlade = {
                        id: "seoDetail",
                        seoUrlKeywordType: 0,
                        parentEntity: $scope.blade.currentEntity,
                        title: $scope.blade.title,
                        controller: 'virtoCommerce.catalogModule.seoDetailController',
                        //seoInfos: $scope.blade.item.seoInfos,
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/seo-detail.tpl.html'
                    };
                    break;
            }

            if (newBlade != null) {
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            }
        });
    }

    $scope.codeValidator = function (value) {
        var pattern = /[$+;=%{}[\]|\\\/@ ~#!^*&()?:'<>,]/;
        return !pattern.test(value);
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
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

    $scope.blade.isLoading = false;
}]);



﻿angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.seoDetailController', ['$scope', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', function ($scope, categories, items, dialogService, bladeNavigationService) {
    var blade = $scope.blade;
    
    function initializeBlade(parentEntity) {
        if (parentEntity) {
            if (blade.isNew) {
                blade.data = parentEntity;
            }
            var data = parentEntity.seoInfos.slice();

            // generate seo for missing languages
            _.each(parentEntity.catalog.languages, function (lang) {
                if (_.every(data, function (seoInfo) { return seoInfo.languageCode.toLowerCase().indexOf(lang.languageCode.toLowerCase()) < 0; })) {
                    data.push({ isNew: true, languageCode: lang.languageCode });
                }
            });

            $scope.seoInfos = angular.copy(data);
            blade.origItem = data;
            blade.parentEntityId = parentEntity.id;
            blade.isLoading = false;
        }
    };
    
    $scope.saveChanges = function () {
        var seoInfos = _.filter($scope.seoInfos, function (data) {
            return isValid(data);
        });

        if (blade.isNew) {
            blade.data.seoInfos = seoInfos;
            blade.origItem = $scope.seoInfos;
            $scope.bladeClose();
        } else {
            blade.isLoading = true;

            getCurrentResource().update({ id: blade.parentEntityId, seoInfos: seoInfos },
                blade.parentBlade.refresh, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, blade);
                });
        }
    }

    function getCurrentResource() {
        switch (blade.seoUrlKeywordType) {
            case 0:
                return categories;
            case 1:
                return items;
            default:
                return null;
        }
    }

    function isValid(data) {
        // check required and valid Url requirements
        return data.semanticUrl && $scope.semanticUrlValidator(data.semanticUrl);
    }

    $scope.semanticUrlValidator = function (value) {
        //var pattern = /^([a-zA-Z0-9\(\)_\-]+)*$/;
        var pattern = /[$+;=%{}[\]|\\\/@ ~#!^*&?:'<>,]/;
        return !pattern.test(value);
    }

    function isDirty() {
        return !angular.equals($scope.seoInfos, blade.origItem);
    };

    blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "catalog.dialogs.seo-save.title",
                message: "catalog.dialogs.seo-save.message"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    $scope.saveChanges();
                }
                closeCallback();
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    if (!blade.isNew) {
        blade.headIcon = 'fa-globe';
        blade.toolbarCommands = [
            {
                name: "platform.commands.save", icon: 'fa fa-save',
                executeMethod: function () {
                    $scope.saveChanges();
                },
                canExecuteMethod: function () {
                    return isDirty() && _.every(_.filter($scope.seoInfos, function (data) { return !data.isNew; }), isValid) && _.some($scope.seoInfos, isValid); // isValid formScope && formScope.$valid;
                },
                permission: 'catalog:update'
            },
            {
                name: "platform.commands.reset", icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(blade.origItem, $scope.seoInfos);
                },
                canExecuteMethod: function () {
                    return isDirty();
                },
                permission: 'catalog:update'
            }
        ];
    }

    blade.subtitle = 'catalog.blades.seo-detail.subtitle';

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade); // for category
    $scope.$watch('blade.parentBlade.item', initializeBlade);          // for item
}]);

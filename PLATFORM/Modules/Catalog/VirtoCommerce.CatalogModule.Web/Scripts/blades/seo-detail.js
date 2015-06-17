angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.seoDetailController', ['$scope', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', 'virtoCommerce.catalogModule.catalogs', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', function ($scope, categories, items, catalogs, dialogService, bladeNavigationService) {
    var blade = $scope.blade;

    function initializeBlade(parentEntity) {
        if (parentEntity) {
            if (blade.isNew) {
                blade.data = parentEntity;
            }
            var data = parentEntity.seoInfos || []; // temp workaround for missing property
            data = data.slice();

            // generate seo for missing languages
            _.each(getLanguages(parentEntity), function (lang) {
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

    function getLanguages(parentEntity) {
        if (blade.seoUrlKeywordType == 0 || blade.seoUrlKeywordType == 1) {
            return parentEntity.catalog.languages;
        } else {
            return parentEntity.languages;
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

            getUpdateFunction()({ id: blade.parentEntityId, seoInfos: seoInfos },
                blade.parentBlade.refresh, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, blade);
                });
        }
    }

    function getUpdateFunction() {
        switch (blade.seoUrlKeywordType) {
            case 0:
                return categories.update;
            case 1:
                return items.updateitem;
            case 3:
                return catalogs.update;
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
                title: "Save changes",
                message: "The SEO information has been modified. Do you want to save changes?"
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
        blade.toolbarCommands = [
            {
                name: "Save", icon: 'fa fa-save',
                executeMethod: function () {
                    $scope.saveChanges();
                },
                canExecuteMethod: function () {
                    return isDirty() && _.every(_.filter($scope.seoInfos, function (data) { return !data.isNew; }), isValid) && _.some($scope.seoInfos, isValid); // isValid formScope && formScope.$valid;
                },
                permission: 'catalog:items:manage'
            },
            {
                name: "Reset", icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(blade.origItem, $scope.seoInfos);
                },
                canExecuteMethod: function () {
                    return isDirty();
                },
                permission: 'catalog:items:manage'
            }
        ];
    }

    blade.subtitle = 'SEO information';

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);
    $scope.$watch('blade.parentBlade.item', initializeBlade);
}]);

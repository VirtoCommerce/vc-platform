angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.seoDetailController', ['$scope', 'virtoCommerce.storeModule.stores', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', function ($scope, stores, dialogService, bladeNavigationService) {
    var blade = $scope.blade;

    function initializeBlade(parentEntity) {
        if (parentEntity) {
            if (blade.isNew) {
                blade.data = parentEntity;
            }
            var data = parentEntity.seoInfos.slice();

            // generate seo for missing languages
            _.each(parentEntity.languages, function (languageCode) {
                if (_.every(data, function (seoInfo) { return seoInfo.languageCode.toLowerCase().indexOf(languageCode.toLowerCase()) < 0; })) {
                    data.push({ isNew: true, languageCode: languageCode });
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

            stores.update({ id: blade.parentEntityId, seoInfos: seoInfos },
                blade.parentBlade.refresh, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, blade);
                });
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
    };

    function isDirty() {
        return !angular.equals($scope.seoInfos, blade.origItem) && blade.hasUpdatePermission();
    }

    function canSave() {
        return isDirty() && _.every(_.filter($scope.seoInfos, function (data) { return !data.isNew; }), isValid) && _.some($scope.seoInfos, isValid); // isValid formScope && formScope.$valid;
    }

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "stores.dialogs.seo-save.title", "stores.dialogs.seo-save.message");
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    if (!blade.isNew) {
        blade.toolbarCommands = [
            {
                name: "platform.commands.save", icon: 'fa fa-save',
                executeMethod: $scope.saveChanges,
                canExecuteMethod: canSave,
                permission: blade.updatePermission
            },
            {
                name: "platform.commands.reset", icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(blade.origItem, $scope.seoInfos);
                },
                canExecuteMethod: isDirty,
                permission: blade.updatePermission
            }
        ];
    }

    blade.subtitle = 'stores.blades.seo-detail.subtitle';

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);
}]);

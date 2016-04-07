angular.module('virtoCommerce.coreModule.seo')
.controller('virtoCommerce.coreModule.seo.seoDetailController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    function initializeBlade() {
        blade.origEntity = blade.data;
        blade.currentEntity = angular.copy(blade.origEntity);
        blade.isLoading = false;
    };

    $scope.cancelChanges = function () {
        angular.copy(blade.origEntity, blade.currentEntity);
        $scope.bladeClose();
    };

    $scope.saveChanges = function () {
        if (blade.isNew) {
            blade.seoContainerObject.seoInfos.push(blade.currentEntity);
        }

        angular.copy(blade.currentEntity, blade.origEntity);
        $scope.bladeClose();

        if (blade.parentRefresh)
            blade.parentRefresh();
    }

    $scope.storeDuplicateValidator = function (value) {
        var data = blade.currentEntity;
        return _.all(blade.seoContainerObject.seoInfos, function (x) {
            return x === blade.origEntity ||
                   !x.isActive ||
                   x.storeId !== value ||
                   x.languageCode !== data.languageCode; // && x.semanticUrl !== data.semanticUrl
        });
    };

    $scope.languageDuplicateValidator = function (value) {
        var data = blade.currentEntity;
        return _.all(blade.seoContainerObject.seoInfos, function (x) {
            return x === blade.origEntity ||
                   !x.isActive ||
                   x.storeId !== data.storeId ||
                   x.languageCode !== value; // && x.semanticUrl !== data.semanticUrl
        });
    };

    function isValid(data) {
        // check required and valid Url requirements
        return data.semanticUrl &&
               $scope.semanticUrlValidator(data.semanticUrl) &&
               (!data.isActive ||
                 $scope.duplicateValidator(data.semanticUrl));
    }

    $scope.semanticUrlValidator = function (value) {
        //var pattern = /^([a-zA-Z0-9\(\)_\-]+)*$/;
        var pattern = /[$+;=%{}[\]|\\\/@ ~#!^*&?:'<>,]/;
        return !pattern.test(value);
    };

    $scope.duplicateValidator = function (value) {
        var data = blade.currentEntity;
        return _.all(blade.seoContainerObject.seoInfos, function (x) {
            return x === blade.origEntity ||
                   !x.isActive ||
                   x.storeId !== data.storeId ||
                   x.languageCode !== data.languageCode; // && x.semanticUrl !== data.semanticUrl
        });
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return isDirty() && isValid(blade.currentEntity); // isValid formScope && formScope.$valid;
    }

    $scope.isValid = canSave;

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "core.dialogs.seo-save.title", "core.dialogs.seo-save.message");
    };

    blade.toolbarCommands = [
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        }
    ];

    blade.headIcon = 'fa-globe';
    blade.title = blade.isNew ? 'core.blades.seo-detail.title-new' : blade.data.semanticUrl;
    blade.subtitle = 'core.blades.seo-detail.subtitle';

    initializeBlade();
}]);

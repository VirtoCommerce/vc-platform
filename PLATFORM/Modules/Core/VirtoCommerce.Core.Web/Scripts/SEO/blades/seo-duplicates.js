angular.module('virtoCommerce.coreModule.seo')
.controller('virtoCommerce.coreModule.seo.seoDuplicatesController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.coreModule.seoApi', function ($rootScope, $scope, bladeNavigationService, seoApi) {
    var blade = $scope.blade;
    var anyValidSeoFound;

    function initializeBlade() {
        blade.origEntity = blade.duplicates;
        blade.currentEntities = angular.copy(blade.origEntity);
        blade.isLoading = false;
    };

    function validate(data) {
        // check all restrictions
        var isUrlValid = data.semanticUrl && $scope.semanticUrlValidator(data.semanticUrl);
        var retVal = isUrlValid && $scope.duplicateValidator(data.semanticUrl, data);
        if (isUrlValid && !retVal) {
            blade.conflicts[data.id] = true;
        }
        anyValidSeoFound = anyValidSeoFound || retVal;
        return retVal;
    }

    $scope.semanticUrlValidator = function (value) {
        var pattern = /[$+;=%{}[\]|\\\/@ ~#!^*&?:'<>,]/;
        return !pattern.test(value);
    };

    $scope.duplicateValidator = function (value, seoInfo) {
        return _.all(blade.currentEntities, function (x) {
            return x === seoInfo ||
                x.storeId !== seoInfo.storeId ||
                x.semanticUrl !== value;
        });
    };

    $scope.saveChanges = function () {
        blade.isLoading = true;

        // generate data to save
        var dataToSave = [];
        _.each(blade.currentEntities, function (x) {
            if (validate(x) &&
                !angular.equals(x, _.findWhere(blade.origEntity, { id: x.id }))) {
                x = angular.copy(x);
                if (!x.storeId) {
                    x.id = undefined;
                    x.storeId = blade.defaultContainerId;
                }
                dataToSave.push(x);
            }
        });

        seoApi.batchUpdate(dataToSave, function () {
            angular.copy(blade.currentEntities, blade.origEntity);

            seoApi.query({ objectId: blade.seoContainerObject.id, objectType: blade.objectType }, function (results) {
                if (_.any(results)) { // conflicts still found
                    blade.duplicates = results;
                    initializeBlade();
                } else {
                    if (!blade.noClose) {
                        $scope.bladeClose();
                    }
                    $rootScope.$broadcast("refresh-entity-by-id", blade.seoContainerObject.id);
                }
            });
            if (blade.parentRefresh)
                blade.parentRefresh();
        }, function (error) { bladeNavigationService.setError('Error: ' + error.status, blade); });
    };

    function saveChanges_noClose() {
        blade.noClose = true;
        $scope.saveChanges();
    }

    function isDirty() {
        return !angular.equals(blade.currentEntities, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return isDirty() && anyValidSeoFound;
    }

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, saveChanges_noClose, closeCallback, "core.dialogs.seo-save.title", "core.dialogs.seo-save.message");
    };

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
                angular.copy(blade.origEntity, blade.currentEntities);
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        }
    ];

    blade.title = blade.parentBlade.title;
    blade.subtitle = 'core.blades.seo-duplicates.subtitle';

    initializeBlade();

    $scope.$watch('blade.currentEntities', function () {
        blade.conflicts = {};
        anyValidSeoFound = false;
        // must check ALL SEOs
        _.each(blade.currentEntities, validate);
    }, true);
}]);

angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.editorialReviewDetailWizardStepController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', function ($scope, bladeNavigationService, settings) {
    var blade = $scope.blade;
    var promise = settings.getValues({ id: 'Catalog.EditorialReviewTypes' }).$promise;

    function initializeBlade(data) {
        promise.then(function (promiseData) {
            $scope.types = promiseData;

            if (!data.reviewType) {
                data.reviewType = $scope.types[0];
            }

            $scope.currentEntity = angular.copy(data);
            blade.origEntity = data;
            blade.isLoading = false;
        });
    };

    $scope.saveChanges = function () {
        if (angular.isUndefined(blade.parentBlade.currentEntities)) {
            //If there is no list of reviews save directly to item reviews
            if (angular.isUndefined(blade.parentBlade.item.reviews)) {
                blade.parentBlade.item.reviews = [];
            }
            blade.parentBlade.item.reviews.push($scope.currentEntity);
        } else {
            var idx = blade.parentBlade.currentEntities.indexOf(blade.origEntity);
            blade.parentBlade.currentEntities.splice(idx, idx < 0 ? 0 : 1, $scope.currentEntity);
        }

        $scope.bladeClose();
    };

    blade.toolbarCommands = [
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                if (angular.isDefined(blade.parentBlade.currentEntities)) {
                    var idx = blade.parentBlade.currentEntities.indexOf(blade.origEntity);
                    blade.parentBlade.currentEntities.splice(idx, 1);
                    $scope.bladeClose();
                }
            },
            canExecuteMethod: function () {
                if (angular.isDefined(blade.parentBlade.currentEntities)) {
                    return blade.parentBlade.currentEntities.indexOf(blade.origEntity) >= 0;
                }
                return false;
            }
        }
    ];

    $scope.openDictionarySettingManagement = function () {
        var newBlade = {
            id: 'settingDetailChild',
            isApiSave: true,
            currentEntityId: 'Catalog.EditorialReviewTypes',
            parentRefresh: function (data) { $scope.types = data; },
            controller: 'platformWebApp.settingDictionaryController',
            template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    // on load
    initializeBlade(blade.currentEntity);
}]);

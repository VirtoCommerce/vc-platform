angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addPublishingPlaceholdersStepController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.contentPlaces', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, marketing_dynamicContents_res_contentPlaces, bladeNavigationService) {
    var blade = $scope.blade;
    blade.chosenFolder = 'ContentPlace';
    blade.currentEntity = {};

    function refresh() {
        marketing_dynamicContents_res_search.search({ folderId: blade.chosenFolder, responseGroup: '20' }, function (data) {
            blade.currentEntity.childrenFolders = data.contentFolders;
            blade.currentEntity.placeholders = data.contentPlaces;
            setBreadcrumbs();
            blade.isLoading = false;
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    blade.initialize = function () {
        refresh();

        blade.entity.contentPlaces.forEach(function (el) {
            marketing_dynamicContents_res_contentPlaces.get({ id: el.id }, function (data) {
                var orEl = _.find(blade.parentBlade.originalEntity.contentPlaces, function (contentPlace) { return contentPlace.id === el.id });
                if (!angular.isUndefined(orEl)) {
                    orEl.path = data.path;
                    orEl.outline = data.outline;
                }
                el.path = data.path;
                el.outline = data.outline;
            }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        });
    }

    blade.addPlaceholder = function (placeholder) {
        blade.entity.contentPlaces.push(placeholder);
    }

    blade.folderClick = function (placeholderFolder) {
        if (angular.isUndefined(blade.chosenFolder) || !angular.equals(blade.chosenFolder, placeholderFolder.id)) {
            blade.isLoading = true;
            blade.chosenFolder = placeholderFolder.id;
            blade.currentEntity = placeholderFolder;
            refresh();
        }
        else {
            blade.chosenFolder = placeholderFolder.parentFolderId;
            blade.currentEntity = undefined;
        }
    }

    blade.deleteAllPlaceholder = function () {
        blade.entity.contentPlaces = [];
    }

    blade.deletePlaceholder = function (data) {
        blade.entity.contentPlaces = _.filter(blade.entity.contentPlaces, function (place) { return !angular.equals(data.id, place.id); });;
    }

    blade.checkPlaceholder = function (data) {
        return _.filter(blade.entity.contentPlaces, function (ci) { return angular.equals(ci.id, data.id); }).length == 0;
    }

    function setBreadcrumbs() {
        if (blade.breadcrumbs) {
            var breadcrumbs;
            var index = _.findLastIndex(blade.breadcrumbs, { id: blade.chosenFolder });
            if (index > -1) {
                //Clone array (angular.copy leaves the same reference)
                breadcrumbs = blade.breadcrumbs.slice(0, index + 1);
            }
            else {
                breadcrumbs = blade.breadcrumbs.slice(0);
                breadcrumbs.push(generateBreadcrumb(blade.currentEntity));
            }
            blade.breadcrumbs = breadcrumbs;
        } else {
            blade.breadcrumbs = [(generateBreadcrumb({ id: 'ContentPlace', name: 'Placeholders' }))];
        }
    }

    function generateBreadcrumb(node) {
        return {
            id: node.id,
            name: node.name,
            navigate: function () {
                blade.folderClick(node);
            }
        }
    }

    blade.headIcon = 'fa-paperclip';

    blade.initialize();
}]);
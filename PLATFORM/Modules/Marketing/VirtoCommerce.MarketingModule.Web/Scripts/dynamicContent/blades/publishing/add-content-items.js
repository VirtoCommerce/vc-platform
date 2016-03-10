angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addPublishingContentItemsStepController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.contentItems', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_search, marketing_dynamicContents_res_contentItems, bladeNavigationService) {
    var blade = $scope.blade;
    blade.chosenFolder = 'ContentItem';
    blade.currentEntity = {};

    function refresh() {
        marketing_dynamicContents_res_search.search({ folderId: blade.chosenFolder, responseGroup: '18' }, function (data) {
            blade.currentEntity.childrenFolders = data.contentFolders;
            blade.currentEntity.items = data.contentItems;
            setBreadcrumbs();
            blade.isLoading = false;
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    blade.initialize = function () {
        refresh();

        blade.entity.contentItems.forEach(function (el) {
            marketing_dynamicContents_res_contentItems.get({ id: el.id }, function (data) {
                var orEl = _.find(blade.parentBlade.originalEntity.contentItems, function (contentItem) { return contentItem.id === el.id });
                if (!angular.isUndefined(orEl)) {
                    orEl.path = data.path;
                    orEl.outline = data.outline;
                    orEl.dynamicProperties = data.dynamicProperties;
                    orEl.objectType = data.objectType;
                }
                el.path = data.path;
                el.outline = data.outline;
                el.dynamicProperties = data.dynamicProperties;
                el.objectType = data.objectType;
            }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        });
    }

    blade.addContentItem = function (contentItem) {
        blade.entity.contentItems.push(contentItem);
    }

    blade.deleteAllContentItems = function () {
        blade.entity.contentItems = [];
    }

    blade.deleteContentItem = function (data) {
        blade.entity.contentItems = _.filter(blade.entity.contentItems, function (place) { return !angular.equals(data.id, place.id); });;
    }

    blade.checkContentItem = function (data) {
        return _.filter(blade.entity.contentItems, function (ci) { return angular.equals(ci.id, data.id); }).length == 0;
    }

    blade.folderClick = function (contentItem) {
        if (angular.isUndefined(blade.chosenFolder) || !angular.equals(blade.chosenFolder, contentItem.id)) {
            blade.isLoading = true;
            blade.chosenFolder = contentItem.id;
            blade.currentEntity = contentItem;
            refresh();
        }
    }

    blade.headIcon = 'fa-paperclip';

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
            blade.breadcrumbs = [(generateBreadcrumb({ id: 'ContentItem', name: 'Items' }))];
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

    blade.initialize();
}]);
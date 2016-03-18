﻿angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.themeAssetListController', ['$scope', 'virtoCommerce.contentModule.themes', 'virtoCommerce.contentModule.stores', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, themes, themesStores, bladeNavigationService, dialogService) {
    var blade = $scope.blade;

    blade.selectedAssetId = undefined;

    blade.initialize = function () {
        blade.isLoading = true;
        themes.getAssets({ storeId: blade.chosenStoreId, themeId: blade.chosenThemeId }, function (data) {
            blade.currentEntities = data;
            themesStores.get({ id: blade.chosenStoreId }, function (data) {
                blade.store = data;
                blade.isLoading = false;
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    blade.folderClick = function (data) {
        var folder = _.find(blade.folders, function (folder) { return data.folderName === folder.name; });
        if (folder.isOpen) {
            folder.isOpen = false;
        }
        else {
            folder.isOpen = true;
        }
    }

    blade.checkFolder = function (data) {
        var folder = _.find(blade.folders, function (folder) { return data.folderName === folder.name; });

        if (angular.isUndefined(folder)) {
            return false;
        }

        return folder.isOpen;
    }

    blade.checkAsset = function (data) {
        return blade.selectedAssetId === data.id;
    }

    blade.getOneItemName = function (data) {
        var folder = _.find(blade.folders, function (folder) { return data.folderName === folder.name; });

        if (angular.isUndefined(folder)) {
            return false;
        }

        return folder.oneItemName;
    }

    blade.assetClass = function (asset) {
        switch (asset.contentType) {
            case 'text/html':
            case 'application/json':
            case 'application/javascript':
                return 'fa-file-text';

            case 'image/png':
            case 'image/jpeg':
            case 'image/bmp':
            case 'image/gif':
                return 'fa-image';

            default:
                return 'fa-file';
        }
    }

    blade.openBlade = function (asset, data) {
        blade.selectedAssetId = asset.id;

        if (asset.contentType === 'text/html' || asset.contentType === 'application/json' || asset.contentType === 'application/javascript') {
            var newBlade = {
                id: 'editAssetBlade',
                chosenStoreId: blade.chosenStoreId,
                chosenThemeId: blade.chosenThemeId,
                chosenAssetId: asset.id,
                chosenFolder: data.folderName,
                newAsset: false,
                title: asset.id,
                subtitle: 'content.blades.edit-asset.subtitle',
                subtitleValues: { name: asset.name },
                controller: 'virtoCommerce.contentModule.editAssetController',
                template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/edit-asset.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        }
        else {
            var newBlade = {
                id: 'editImageAssetBlade',
                chosenStoreId: blade.chosenStoreId,
                chosenThemeId: blade.chosenThemeId,
                chosenAssetId: asset.id,
                chosenFolder: data.folderName,
                newAsset: false,
                title: asset.id,
                subtitle: 'content.blades.edit-image-asset.subtitle',
                subtitleValues: { name: asset.name },
                controller: 'virtoCommerce.contentModule.editImageAssetController',
                template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/edit-image-asset.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        }
    }

    blade.folderSorting = function (entity) {
        if (entity.folderName == "layout")
            return 0;
        else if (entity.folderName == "templates")
            return 1;
        else if (entity.folderName == "snippets")
            return 2;
        else if (entity.folderName == "assets")
            return 3;
        else if (entity.folderName == "config")
            return 4;
        else if (entity.folderName == "locales")
            return 5;

        return 10;
    }

    blade.openBladeNew = function (data) {
        var folder = blade.getFolder(data);
        var contentType = folder.defaultContentType;
        var name = folder.defaultItemName;

        if (contentType === 'text/html') {
            var newBlade = {
                id: 'addAsset',
                chosenStoreId: blade.chosenStoreId,
                chosenThemeId: blade.chosenThemeId,
                chosenFolder: data.folderName,
                newAsset: true,
                currentEntity: { id: undefined, content: undefined, contentType: contentType, assetUrl: undefined, name: name },
                titleValues: { name: folder.oneItemName },
                subtitle: { name: folder.oneItemName },
                title: 'content.blades.edit-asset.title-new',
                subtitle: 'content.blades.edit-asset.subtitle-new',
                controller: 'virtoCommerce.contentModule.editAssetController',
                template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/edit-asset.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        }
        else {
            var newBlade = {
                id: 'addImageAsset',
                chosenStoreId: blade.chosenStoreId,
                chosenThemeId: blade.chosenThemeId,
                chosenFolder: data.folderName,
                newAsset: true,
                currentEntity: { id: undefined, content: undefined, contentType: undefined, assetUrl: undefined, name: undefined },
                titleValues: { name: folder.oneItemName },
                subtitle: { name: folder.oneItemName },
                title: 'content.blades.edit-image-asset.title-new',
                subtitle: 'content.blades.edit-image-asset.subtitle-new',
                controller: 'virtoCommerce.contentModule.editImageAssetController',
                template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/edit-image-asset.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        }
    }

    blade.getFolder = function (data) {
        var folder = _.find(blade.folders, function (folder) { return folder.name === data.folderName });

        if (folder !== undefined) {
            return folder;
        }

        return null;
    }

    blade.isFolder = function (data) {
        var folder = _.find(blade.folders, function (folder) { return folder.name === data.folderName });

        if (folder !== undefined) {
            return true;
        }

        return false;
    }

    $scope.blade.headIcon = 'fa-archive';

    blade.folders = [
		{ name: 'assets', oneItemName: 'asset', defaultItemName: undefined, defaultContentType: null, isOpen: false },
		{ name: 'layout', oneItemName: 'layout', defaultItemName: 'new_layout.liquid', defaultContentType: 'text/html', isOpen: false },
		{ name: 'config', oneItemName: 'config', defaultItemName: 'new_config.json', defaultContentType: 'application/json', isOpen: false },
		{ name: 'locales', oneItemName: 'locale', defaultItemName: 'new_locale.json', defaultContentType: 'application/json', isOpen: false },
		{ name: 'snippets', oneItemName: 'snippet', defaultItemName: 'new_snippet.liquid', defaultContentType: 'text/html', isOpen: false },
		{ name: 'templates', oneItemName: 'template', defaultItemName: 'new_template.liquid', defaultContentType: 'text/html', isOpen: false }
    ];

    blade.initialize();
}]);

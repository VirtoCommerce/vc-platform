angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.contentMainController', ['$scope', '$state', '$stateParams', 'virtoCommerce.contentModule.menus', 'virtoCommerce.contentModule.contentApi', 'virtoCommerce.storeModule.stores', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.widgetService',
	function ($scope, $state, $stateParams, menus, contentApi, stores, bladeNavigationService, dialogService, widgetService) {
	    var blade = $scope.blade;

	    blade.initialize = function () {
	        blade.isLoading = true;
	        blade.currentEntities = [];

	        if ($stateParams.storeId) {
	            stores.get({ id: $stateParams.storeId }, blade.openThemes);
	        };

	        stores.query(null, function (storesResult) {
	            var loadCounter = storesResult.length * 2;

	            var finnalyFunction = function () {
	                blade.isLoading = --loadCounter;
	            };

	            blade.isLoading = _.any(storesResult);

	            _.each(storesResult, function (x) {
	                blade.currentEntities.push({
	                    storeId: x.id,
	                    store: x,
	                    themesCount: '...',
	                    pagesCount: '...',
	                    blogsCount: '...',
	                    listLinksCount: '...'
	                });

	                blade.refresh(x.id, 'stats').finally(finnalyFunction);
	                blade.refresh(x.id, 'menus').finally(finnalyFunction);
	            });
	        });

	        $scope.thereIsWidgetToShow = _.any(widgetService.widgetsMap['contentMainListItem'], function (w) { return !angular.isFunction(w.isVisible) || w.isVisible(blade); });
	    };


	    blade.refresh = function (storeId, requestType, data) {
	        var entity = _.findWhere(blade.currentEntities, { storeId: storeId });

	        switch (requestType) {
	            case 'menus':
	                return menus.get({ storeId: storeId }, function (data) {
	                    entity.listLinksCount = data.length;
	                }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); }).$promise;
	                break;
	            case 'stats':
	                return contentApi.getStatistics({ storeId: storeId }, function (data) {
	                    angular.extend(entity, data);
	                }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); }).$promise;
	                break;
	            case 'defaultTheme':
	                entity.activeThemeName = data;
	            default:
	                break;
	        }
	    };

	    $scope.$on("cms-menus-changed", function (event, storeId) {
	        blade.refresh(storeId, 'menus');
	    });

	    $scope.$on("cms-statistics-changed", function (event, storeId) {
	        blade.refresh(storeId, 'stats');
	    });

	    blade.openThemes = function (store) {
	        var newBlade = {
	            id: "themesListBlade",
	            storeId: store.id,
	            baseThemes: getBaseThemes(store),
	            title: 'content.blades.themes-list.title',
	            titleValues: { name: store.name },
	            subtitle: 'content.blades.themes-list.subtitle',
	            controller: 'virtoCommerce.contentModule.themesListController',
	            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/themes-list.tpl.html',
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.openPages = function (data) {
	        var newBlade = {
	            id: "pagesList",
	            contentType: 'pages',
	            storeId: data.storeId,
	            languages: data.store.languages,
	            currentEntity: data,
	            title: data.store.name,
	            subtitle: 'content.blades.pages-list.subtitle-pages',
	            controller: 'virtoCommerce.contentModule.pagesListController',
	            template: '$(Platform)/Scripts/app/assets/blades/asset-list.tpl.html'
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.openLinkLists = function (data) {
	        var newBlade = {
	            id: "linkListBlade",
	            storeId: data.storeId,
	            title: 'content.blades.link-lists.title',
	            titleValues: { name: data.store.name },
	            subtitle: 'content.blades.link-lists.subtitle',
	            controller: 'virtoCommerce.contentModule.linkListsController',
	            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/menu/link-lists.tpl.html',
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.openBlogs = function (data) {
	        var newBlade = {
	            id: "blogsListBlade",
	            contentType: 'blogs',
	            storeId: data.storeId,
	            languages: data.store.languages,
	            currentEntity: data,
	            title: data.store.name,
	            subtitle: 'content.blades.pages-list.subtitle-blogs',
	            controller: 'virtoCommerce.contentModule.pagesListController',
	            template: '$(Platform)/Scripts/app/assets/blades/asset-list.tpl.html'
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.addNewTheme = function (data) {
	        var newBlade = {
	            id: 'addTheme',
	            isNew: true,
	            isActivateAfterSave: !data.themesCount,
	            store: data.store,
	            storeId: data.storeId,
	            baseThemes: getBaseThemes(data.store),
	            controller: 'virtoCommerce.contentModule.themeDetailController',
	            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/theme-detail.tpl.html',
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.addNewPage = function (data) {
	        var newBlade = {
	            id: 'addPage',
	            contentType: 'pages',
	            storeId: data.storeId,
	            languages: data.store.languages,
	            currentEntity: {},
	            isNew: true,
	            title: 'content.blades.edit-page.title-new',
	            subtitle: 'content.blades.edit-page.subtitle-new',
	            controller: 'virtoCommerce.contentModule.pageDetailController',
	            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/page-detail.tpl.html',
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.addNewLinkList = function (data) {
	        var newBlade = {
	            id: 'addMenuLinkListBlade',
	            chosenStoreId: data.storeId,
	            newList: true,
	            title: 'content.blades.menu-link-list.title-new',
	            subtitle: 'content.blades.menu-link-list.subtitle-new',
	            controller: 'virtoCommerce.contentModule.menuLinkListController',
	            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/menu/menu-link-list.tpl.html',
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.addBlog = function (data) {
	        var newBlade = {
	            id: 'newBlog',
	            contentType: 'blogs',
	            storeId: data.storeId,
	            currentEntity: {},
	            isNew: true,
	            title: 'content.blades.edit-blog.title-new',
	            subtitle: 'content.blades.edit-blog.subtitle-new',
	            controller: 'virtoCommerce.contentModule.editBlogController',
	            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-blog.tpl.html',
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.openTheme = function (data) {
	        var newBlade = {
	            id: 'themeAssetListBlade',
	            contentType: 'themes',
	            storeId: data.storeId,
	            currentEntity: { name: data.activeThemeName, url: data.activeThemeURL },
	            subtitle: 'content.blades.asset-list.subtitle',
	            controller: 'virtoCommerce.contentModule.assetListController',
	            template: '$(Platform)/Scripts/app/assets/blades/asset-list.tpl.html'
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.previewTheme = function (data) {
	        if (data.store.url) {
	            window.open(data.store.url + '?previewtheme=' + data.activeThemeName, '_blank');
	        }
	        else {
	            var dialog = {
	                id: "noUrlInStore",
	                title: "content.dialogs.set-store-url.title",
	                message: "content.dialogs.set-store-url.message"
	            }
	            dialogService.showNotificationDialog(dialog);
	        }
	    }

	    $scope.openStoresModule = function () {
	        $state.go('workspace.storeModule');
	    };

	    function getBaseThemes(store) {
	        var setting;
	        if (setting = _.findWhere(store.settings, { name: 'VirtoCommerce.Content.BaseThemes' })) {
	            return setting.arrayValues;
	        }
	        return null;
	    }

	    blade.headIcon = 'fa-code';

	    blade.initialize();
	}]);

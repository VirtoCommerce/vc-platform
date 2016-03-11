angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.contentMainController', ['$scope', '$state', '$stateParams', 'virtoCommerce.contentModule.menus', 'virtoCommerce.contentModule.pages', 'virtoCommerce.contentModule.themes', 'virtoCommerce.contentModule.stores', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.authService',
	function ($scope, $state, $stateParams, menus, pages, themes, stores, bladeNavigationService, dialogService, authService) {
	    var blade = $scope.blade;

	    blade.initialize = function () {
	        blade.isLoading = true;
	        blade.currentEntities = [];

	        if (!angular.isUndefined($stateParams.storeId)) {
	            stores.get({ id: $stateParams.storeId }, function (data) {
	                blade.openThemes($stateParams.storeId, data.name);
	            });
	        };

	        stores.query({}, function (data) {
	            var loadCounter = data.length;

	            var finnalyFunction = function () {
	                blade.isLoading = --loadCounter;
	            };

	            blade.isLoading = _.any(data);

	            for (var i = 0; i < data.length; i++) {
	                stores.get({ id: data[i].id }, function (data) {
	                    var entity = {};
	                    entity.store = data;
	                    entity.listLinksCount = '...';
	                    entity.pagesCount = '...';
	                    entity.themesCount = '...';
	                    entity.blogsCount = '...';
	                    entity.defaultThemeName = undefined;
	                    entity.defaultTheme = undefined;
	                    entity.themes = [];

	                    menus.get({ storeId: entity.store.id }, function (data) {
	                        entity.listLinksCount = data.length;
	                    });

	                    pages.get({ storeId: entity.store.id }, function (data) {
	                        entity.pagesCount = _.reject(data, function (x) { return x.id.startsWith("blogs/"); }).length;
	                        pages.getFolders({ storeId: entity.store.id }, function (data) {
	                            var blogs = _.find(data.folders, function (x) { return x.folderName === "blogs" });
	                            if (blogs && blogs.folders) {
	                                entity.blogsCount = blogs.folders.length;
	                            }
	                            else {
	                                entity.blogsCount = 0;
	                            }
	                        });
	                    });

	                    themes.get({ storeId: entity.store.id }, function (data) {
	                        entity.themesCount = data.length;
	                        entity.themes = data;

	                        var defaultThemeNameProperty = _.find(entity.store.dynamicProperties, function (property) { return property.name === 'DefaultThemeName'; });
	                        if (defaultThemeNameProperty !== undefined && defaultThemeNameProperty.values !== undefined && defaultThemeNameProperty.values.length > 0) {
	                            entity.defaultThemeName = defaultThemeNameProperty.values[0].value;
	                            if (_.find(entity.themes, function (theme) { return theme.name === entity.defaultThemeName; }) !== undefined) {
	                                entity.defaultTheme = _.find(entity.themes, function (theme) { return theme.name === entity.defaultThemeName; });
	                            }
	                            else {
	                                entity.defaultThemeName = undefined;
	                            }
	                        }
	                        blade.currentEntities.push(entity);
	                    });

	                }).$promise.finally(finnalyFunction);
	            }
	        });
	    };


	    blade.refresh = function (storeId, requestType) {
	        var entity = _.find(blade.currentEntities, function (entity) { return entity.store.id === storeId });

	        switch (requestType) {
	            case 'menus':
	                menus.get({ storeId: storeId }, function (data) {
	                    entity.listLinksCount = data.length;
	                }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
	                break;

	            case 'pages':
	                pages.get({ storeId: storeId }, function (data) {
	                    entity.pagesCount = _.reject(data, function (page) { return page.id.startsWith("blogs/"); }).length;
	                }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
	                break;

	            case 'themes':
	                themes.get({ storeId: storeId, cacheKill: new Date().getTime() }, function (data) {
	                    entity.themesCount = data.length;
	                    entity.themes = data;
	                }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
	                break;

	            case 'blogs':
	                pages.getFolders({ storeId: storeId }, function (data) {
	                    var blogsFolder = _.find(data.folders, function (folder) { return folder.folderName === "blogs" });
	                    if (blogsFolder)
	                        entity.blogsCount = blogsFolder.folders.length;
	                }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
	                break;

	            case 'defaultTheme':
	                stores.get({ id: storeId }, function (data) {
	                    themes.get({ storeId: storeId, cacheKill: new Date().getTime() }, function (themesList) {
	                        entity.themesCount = themesList.length;
	                        entity.themes = themesList;

	                        var defaultThemeNameProperty = _.find(data.dynamicProperties, function (property) { return property.name === 'DefaultThemeName'; });
	                        if (defaultThemeNameProperty !== undefined && defaultThemeNameProperty.values !== undefined && defaultThemeNameProperty.values.length > 0) {
	                            entity.defaultThemeName = defaultThemeNameProperty.values[0].value;
	                            var defaultTheme = _.find(entity.themes, function (theme) { return theme.name === entity.defaultThemeName; });
	                            if (defaultTheme !== undefined) {
	                                entity.defaultTheme = defaultTheme;
	                            }
	                            else {
	                                entity.defaultThemeName = undefined;
	                                if (_.where(data.dynamicProperties, { name: "DefaultThemeName" }).length > 0) {
	                                    angular.forEach(data.dynamicProperties, function (value, key) {
	                                        if (value.name === "DefaultThemeName") {
	                                            value.values[0] = { value: '' };
	                                        }
	                                    });
	                                }

	                                stores.update({ storeId: storeId }, data, function (data) {
	                                    entity.defaultThemeName = undefined;
	                                },
									function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
	                            }
	                        }
	                    }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
	                });

	            default:
	                break;
	        }
	    };

	    blade.openThemes = function (storeId, storeName) {
	        var newBlade = {
	            id: "themesListBlade",
	            storeId: storeId,
	            title: 'content.blades.themes-list.subtitle',
	            titleValues: { name: storeName },
	            subtitle: 'content.blades.themes-list.subtitle',
	            controller: 'virtoCommerce.contentModule.themesListController',
	            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/themes-list.tpl.html',
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.openPages = function (data) {
	        var newBlade = {
	            id: "pagesListBlade",
	            storeId: data.store.id,
	            title: 'content.blades.pages-list.title-pages',
	            titleValues: { name: data.store.name },
	            subtitle: 'content.blades.pages-list.subtitle-pages',
	            type: 'pages',
	            controller: 'virtoCommerce.contentModule.pagesListController',
	            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/pages-list.tpl.html',
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.openLinkLists = function (data) {
	        var newBlade = {
	            id: "linkListBlade",
	            storeId: data.store.id,
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
	            storeId: data.store.id,
	            isBlogsBlade: true,
	            title: 'content.blades.pages-list.title-blogs',
	            titleValues: { name: data.store.name },
	            subtitle: 'content.blades.pages-list.subtitle-blogs',
	            type: 'blogs',
	            controller: 'virtoCommerce.contentModule.pagesListController',
	            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/pages-list.tpl.html',
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.addNewTheme = function (data) {
	        var newBlade = {
	            id: 'addTheme',
	            chosenStoreId: data.store.id,
	            currentEntity: {},
	            title: 'content.blades.add-theme.title',
	            subtitle: 'content.blades.add-theme.subtitle',
	            controller: 'virtoCommerce.contentModule.addThemeController',
	            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/add-theme.tpl.html',
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.addNewPage = function (data) {
	        var newBlade = {
	            id: 'addPageBlade',
	            chosenStoreId: data.store.id,
	            currentEntity: { name: null, content: null },
	            newPage: true,
	            title: 'content.blades.edit-page.title-new',
	            subtitle: 'content.blades.edit-page.subtitle-new',
	            controller: 'virtoCommerce.contentModule.editPageController',
	            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-page.tpl.html',
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.addNewLinkList = function (data) {
	        var newBlade = {
	            id: 'addMenuLinkListBlade',
	            chosenStoreId: data.store.id,
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
	            id: 'openBlogNew',
	            chosenStoreId: data.store.id,
	            isNew: true,
	            entity: { name: undefined },
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
	            chosenThemeId: data.defaultTheme.name,
	            chosenStoreId: data.store.id,
	            chosenTheme: data.defaultTheme,
	            title: 'content.blades.theme-asset-list.subtitle',
	            titleValues: { path: data.defaultTheme.path },
	            subtitle: 'content.blades.theme-asset-list.subtitle',
	            controller: 'virtoCommerce.contentModule.themeAssetListController',
	            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/theme-asset-list.tpl.html',
	        };
	        bladeNavigationService.showBlade(newBlade, blade);
	    };

	    blade.previewTheme = function (data) {
	        if (data.store.url !== undefined) {
	            window.open(data.store.url + '?previewtheme=' + data.defaultTheme.name, '_blank');
	        }
	        else {
	            var dialog = {
	                id: "noUrlInStore",
	                title: "content.dialogs.set-store-url.title",
	                message: "content.dialogs.set-store-url.message",
	                callback: function (remove) {

	                }
	            }
	            dialogService.showNotificationDialog(dialog);
	        }
	    }

	    $scope.openStoresModule = function () {
	        $state.go('workspace.storeModule');
	    };

	    blade.headIcon = 'fa-code';

	    blade.initialize();
	}]);

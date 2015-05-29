angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.contentMainController', ['$scope', 'virtoCommerce.contentModule.menus', 'virtoCommerce.contentModule.pages', 'virtoCommerce.contentModule.themes', 'virtoCommerce.contentModule.stores', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, menus, pages, themes, stores, bladeNavigationService, dialogService) {
	$scope.selectedNodeId = null;

	var blade = $scope.blade;
	blade.currentEntities = [];

	blade.initialize = function () {
		blade.isLoading = true;
		blade.currentEntities = [];

		stores.query({}, function (data) {
			for (var i = 0; i < data.length; i++) {

				stores.get({ id: data[i].id }, function (data) {
					
					var entity = {};
					entity.store = data;
					entity.listLinksCount = '...';
					entity.pagesCount = '...';
					entity.themesCount = '...';
					entity.defaultThemeName = undefined;
					entity.defaultTheme = undefined;
					entity.themes = [];

					menus.get({ storeId: entity.store.id }, function (data) {
						entity.listLinksCount = data.length;

						pages.get({ storeId: entity.store.id }, function (data) {
							entity.pagesCount = data.length;

							themes.get({ storeId: entity.store.id }, function (data) {
								entity.themesCount = data.length;
								entity.themes = data;

								if (_.find(entity.store.settings, function (setting) { return setting.name === 'DefaultThemeName'; }) !== undefined) {
									entity.defaultThemeName = _.find(entity.store.settings, function (setting) { return setting.name === 'DefaultThemeName'; }).value;
									if (_.find(entity.themes, function (theme) { return theme.name === entity.defaultThemeName; }) !== undefined) {
										entity.defaultTheme = _.find(entity.themes, function (theme) { return theme.name === entity.defaultThemeName; });
									}
									else {
										entity.defaultThemeName = undefined;
									}
								}
							});

							blade.currentEntities.push(entity);
							blade.isLoading = false;
						});
					});

				});
			}
		});
	}

	blade.onClose = function (closeCallback) {
		closeChildrenBlades();
		closeCallback();
	};



	function closeChildrenBlades() {
		angular.forEach(blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}


	blade.openThemes = function (data) {
		closeChildrenBlades();

		var newBlade = {
			id: "themesListBlade",
			storeId: data.store.id,
			title: data.store.name + ' themes list',
			subtitle: 'Themes List',
			controller: 'virtoCommerce.contentModule.themesListController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/themes-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.openPages = function (data) {
		closeChildrenBlades();

		var newBlade = {
			id: "pagesListBlade",
			storeId: data.store.id,
			title: data.store.name + ' pages list',
			subtitle: 'Pages List',
			controller: 'virtoCommerce.contentModule.pagesListController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/pages-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.openLinkLists = function (data) {
		closeChildrenBlades();

		var newBlade = {
			id: "linkListBlade",
			storeId: data.store.id,
			title: data.store.name + ' link Lists',
			subtitle: 'Link Lists',
			controller: 'virtoCommerce.contentModule.linkListsController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/menu/link-lists.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.addNewTheme = function (data) {
		closeChildrenBlades();

		var newBlade = {
			id: 'addTheme',
			choosenStoreId: data.store.id,
			currentEntity: {},
			title: 'New theme asset',
			subtitle: 'Create new theme',
			controller: 'virtoCommerce.contentModule.addThemeController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/add-theme.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.addNewPage = function (data) {
		closeChildrenBlades();

		var newBlade = {
			id: 'addPageBlade',
			choosenStoreId: data.store.id,
			currentEntity: { name: null, content: null },
			newPage: true,
			title: 'Add new page',
			subtitle: 'Create new page',
			controller: 'virtoCommerce.contentModule.editPageController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-page.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.addNewLinkList = function (data) {
		closeChildrenBlades();

		var newBlade = {
			id: 'addMenuLinkListBlade',
			choosenStoreId: data.store.id,
			newList: true,
			title: 'Add new list',
			subtitle: 'Create new list',
			controller: 'virtoCommerce.contentModule.menuLinkListController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/menu/menu-link-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.openTheme = function (data) {
		closeChildrenBlades();

		var newBlade = {
			id: 'themeAssetListBlade',
			choosenThemeId: data.defaultTheme.name,
			choosenStoreId: data.store.id,
			choosenTheme: data.defaultTheme,
			title: 'Edit ' + data.defaultTheme.path,
			subtitle: 'Theme asset list',
			controller: 'virtoCommerce.contentModule.themeAssetListController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/themes/theme-asset-list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.previewTheme = function (data) {
		if (data.store.url !== undefined) {
			window.open(data.store.url + '?previewtheme=' + data.defaultTheme.name, '_blank');
		}
		else {
			var dialog = {
				id: "noUrlInStore",
				title: "Url is not set for store",
				message: "Please, set store url, before preview theme!",
				callback: function (remove) {

				}
			}
			dialogService.showNotificationDialog(dialog);
		}
	}

	$scope.blade.headIcon = 'fa fa-code';

	blade.initialize();
}]);

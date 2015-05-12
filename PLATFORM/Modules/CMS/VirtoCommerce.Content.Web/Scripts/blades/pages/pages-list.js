angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.pagesListController', ['$scope', 'virtoCommerce.contentModule.pages', 'virtoCommerce.contentModule.stores', 'platformWebApp.bladeNavigationService', function ($scope, pages, pagesStores, bladeNavigationService) {
	$scope.selectedNodeId = null;

	var blade = $scope.blade;
	blade.steps = ['Pages'];
	blade.selectedStep = 0;

	blade.initialize = function () {
		blade.isLoading = true;
		pages.getFolders({ storeId: blade.storeId }, function (data) {
			blade.isLoading = false;
			blade.pagesCatalog = data;
			blade.currentPageCatalog = data;

			for (var i = 1; i < blade.steps.length; i++) {
				blade.currentPageCatalog = _.find(blade.currentPageCatalog.folders, function (folder) { return folder.folderName === blade.steps[i] });
			}

			blade.parentBlade.initialize();
		})
	}

	blade.openBlade = function (data) {
		$scope.selectedNodeId = data.pageName;
		closeChildrenBlades();

		var add = '';
		if (blade.steps.length > 1) {
			add = _.last(blade.steps) + '/';
		}

		var newBlade = {
			id: 'editPageBlade',
			choosenStoreId: blade.storeId,
			choosenPageName: add + data.name,
			choosenPageLanguage: data.language,
			newPage: false,
			title: 'Edit ' + data.name,
			subtitle: 'Page edit',
			controller: 'virtoCommerce.contentModule.editPageController',
			template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-page.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}
	
	blade.openBladeNew = function(isBytes) {
		$scope.selectedNodeId = null;
		closeChildrenBlades();

		var path = '';
		if (blade.steps.length > 1) {
			path = _.last(blade.steps) + '/';
		}

		if (!isBytes) {
			var newBlade = {
				id: 'addPageBlade',
				choosenStoreId: blade.storeId,
				currentEntity: { name: path + 'new_page.md', content: null, contentType: 'text/html', language: null, storeId: blade.storeId },
				newPage: true,
				title: 'Add new page',
				subtitle: 'Create new page',
				controller: 'virtoCommerce.contentModule.editPageController',
				template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-page.tpl.html'
			};
			bladeNavigationService.showBlade(newBlade, $scope.blade);
		}
		else {
			var newBlade = {
				id: 'addPageBlade',
				choosenStoreId: blade.storeId,
				path: path,
				currentEntity: { name: path + 'new_file', content: null, contentType: null, language: null, storeId: blade.storeId },
				newPage: true,
				title: 'Add new file',
				subtitle: 'Create new file',
				controller: 'virtoCommerce.contentModule.editPageController',
				template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-page.tpl.html'
			};
			bladeNavigationService.showBlade(newBlade, $scope.blade);
		}
	}

	$scope.blade.onClose = function (closeCallback) {
		closeChildrenBlades();
		closeCallback();
	};

	function closeChildrenBlades() {
		angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}

	function deletePage(page) {
		var dialog = {
			id: "confirmDelete",
			title: "Delete confirmation",
			message: "Are you sure you want to delete this page?",
			callback: function (remove) {
				if (remove) {
					blade.isLoading = true;

					pages.delete({ storeId: blade.choosenStoreId, pageNames: blade.page.name }, function () {
						blade.initialize();
					});
				}
			}
		}
		dialogService.showConfirmationDialog(dialog);
	}

	blade.folderClick = function (data) {
		blade.steps.push(data.folderName);

		blade.currentPageCatalog = data;
	}

	blade.stepsClick = function () {
		blade.currentPageCatalog = blade.pagesCatalog;
		var index = blade.selectedStep + 1;

		blade.steps.splice(index);

		for (var i = 1; i < index; i++) {
			blade.currentPageCatalog = _.find(blade.currentPageCatalog.folders, function (folder) { return folder.folderName === blade.steps[i] });
		}
	}

	blade.showDivider = function (index) {
		if (index === blade.steps.length - 1) {
			return false;
		}

		return true;
	}

	$scope.bladeHeadIco = 'fa fa-archive';

	blade.getFlag = function (lang) {
		switch (lang) {
			case 'ru-RU':
				return 'ru';

			case 'en-US':
				return 'us';

			case 'fr-FR':
				return 'fr';

			case 'zh-CN':
				return 'ch';

			case 'ru-RU':
				return 'ru';

			case 'ja-JP':
				return 'jp';

			case 'de-DE':
				return 'de';
		}
	}

	$scope.bladeToolbarCommands = [
        {
        	name: "Add page", icon: 'fa fa-plus',
        	executeMethod: function () {
        		blade.openBladeNew(false);
        	},
        	canExecuteMethod: function () {
        		return true;
        	},
        	permission: 'content:manage'
        },
		{
			name: "Add file", icon: 'fa fa-plus',
			executeMethod: function () {
				blade.openBladeNew(true);
			},
			canExecuteMethod: function () {
				return true;
			},
			permission: 'content:manage'
		},
	];

	blade.initialize();
}]);

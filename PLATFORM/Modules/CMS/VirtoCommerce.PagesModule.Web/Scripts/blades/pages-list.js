angular.module('virtoCommerce.content.pagesModule.blades.pagesList', [
    'virtoCommerce.content.pagesModule.resources.pages',
	'virtoCommerce.content.pagesModule.blades.editPage',
	'virtoCommerce.content.pagesModule.resources.pagesStores'
])
.controller('pagesListController', ['$scope', 'pages', 'pagesStores', 'bladeNavigationService', function ($scope, pages, pagesStores, bladeNavigationService) {
	$scope.selectedNodeId = null;

	var blade = $scope.blade;

	blade.refresh = function () {
		blade.isLoading = true;
		pages.get({ storeId: blade.storeId }, function (data) {
			blade.isLoading = false;
			blade.currentEntities = data;
			blade.parentWidget.refresh();
		});
	}

	blade.openBlade = function (data) {
		$scope.selectedNodeId = data.name;
		closeChildrenBlades();

		var newBlade = {
			id: 'editPageBlade',
			choosenStoreId: blade.storeId,
			choosenPageName: data.name,
			choosenPageLanguage: data.language,
			newPage: false,
			title: 'Edit ' + data.name,
			subtitle: 'Page edit',
			controller: 'editPageController',
			template: 'Modules/$(VirtoCommerce.Pages)/Scripts/blades/edit-page.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	function openBladeNew() {
		$scope.selectedNodeId = null;
		closeChildrenBlades();

		var newBlade = {
			id: 'addPageBlade',
			choosenStoreId: blade.storeId,
			currentEntity: { name: null, content: null },
			newPage: true,
			title: 'Add new page',
			subtitle: 'Create new theme',
			controller: 'editPageController',
			template: 'Modules/$(VirtoCommerce.Pages)/Scripts/blades/edit-page.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
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
						blade.refresh();
					});
				}
			}
		}
		dialogService.showConfirmationDialog(dialog);
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
				return 'ja';
		}
	}

	$scope.bladeToolbarCommands = [
        {
        	name: "Add page", icon: 'fa fa-plus',
        	executeMethod: function () {
        		openBladeNew();
        	},
        	canExecuteMethod: function () {
        		return true;
        	},
        }
	];

	blade.refresh();
}]);

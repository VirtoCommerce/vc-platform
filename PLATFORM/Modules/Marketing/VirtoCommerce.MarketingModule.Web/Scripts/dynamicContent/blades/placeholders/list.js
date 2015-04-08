angular.module('virtoCommerce.marketingModule')
.controller('placeholdersDynamicContentListController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	var blade = $scope.blade

	$scope.selectedNodeId = null;

	function initializeBlade() {
		blade.isLoading = false;
	};

	blade.addNew = function () {
		closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'New placeholders element',
			subtitle: 'Add new placeholders element',
			isNew: true,
			controller: 'addPlaceholderController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/add.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.addNewFolder = function () {
		closeChildrenBlades();

		var newBlade = {
			id: 'listItemChild',
			title: 'New placeholders element',
			subtitle: 'Add new placeholders element',
			isNew: true,
			controller: 'addPlaceholderController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/add.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	$scope.bladeToolbarCommands = [
        {
        	name: "Refresh", icon: 'fa fa-refresh',
        	executeMethod: function () {
        		$scope.blade.refresh();
        	},
        	canExecuteMethod: function () {
        		return true;
        	}
        },
        {
        	name: "Add", icon: 'fa fa-plus',
        	executeMethod: function () {
        		blade.addNew();
        	},
        	canExecuteMethod: function () {
        		return true;
        	}
        },
	];

	$scope.bladeHeadIco = 'fa fa-flag';

	initializeBlade();
}]);

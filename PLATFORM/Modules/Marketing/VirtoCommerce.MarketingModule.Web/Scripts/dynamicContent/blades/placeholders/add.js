angular.module('virtoCommerce.marketingModule')
.controller('pla', ['$scope', 'bladeNavigationService', 'categories', 'items', function ($scope, bladeNavigationService, categories, items) {
	var balde = $scope.blade;
	var pb = $scope.blade.parentBlade;

	$scope.addCategory = function () {
		categories.newCategory({ catalogId: pb.catalogId, parentCategoryId: pb.categoryId },
            function (data) {
            	var newBlade = {
            		id: "newCategoryWizard",
            		currentEntity: data,
            		title: "New category",
            		subtitle: 'Fill category information',
            		controller: 'newCategoryWizardController',
            		template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/newCategory/category-wizard.tpl.html'
            	};
            	bladeNavigationService.showBlade(newBlade, pb);

            	$scope.bladeClose();
            });
	};

	$scope.addLinkedCategory = function () {
		$scope.bladeClose();

		var newBlade = {
			id: 'selectCatalog',
			title: 'Select Catalog',
			subtitle: 'Creating a Link inside virtual catalog',
			controller: 'catalogsSelectController',
			template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalogs-select.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
	};

	$scope.blade.isLoading = false;
}]);

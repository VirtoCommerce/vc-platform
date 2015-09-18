angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quoteItemsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.openBlade = function () {
		var newBlade = {
			id: 'quoteItems',
			title: $scope.blade.title + ' line items',
			subtitle: 'Edit line items',
			recalculateFn: $scope.blade.recalculate,
			currentEntity: $scope.blade.currentEntity,
			controller: 'virtoCommerce.quoteModule.quoteItemsController',
			template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quote-items.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};
}])
;
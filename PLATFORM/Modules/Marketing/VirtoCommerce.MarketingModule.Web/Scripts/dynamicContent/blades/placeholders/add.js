angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addPlaceholderElementController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', function ($scope, bladeNavigationService, categories, items) {
	var blade = $scope.blade;

	blade.addFolder = function () {
		var data = { outline: '', name: '', description: '', parentFolderId: blade.choosenFolder};
		blade.parentBlade.addNewFolder(data);
	};

	blade.addPlaceholder = function () {
		var data = { name: '', description: '', imageUrl: 'http://virtotest.blob.core.windows.net/catalog/9f0113a5-fa34-4d83-bad8-2c9d6fdc763d.png', folderId: blade.choosenFolder };
		blade.parentBlade.addNewPlaceholder(data);
	};

	$scope.blade.isLoading = false;
}]);

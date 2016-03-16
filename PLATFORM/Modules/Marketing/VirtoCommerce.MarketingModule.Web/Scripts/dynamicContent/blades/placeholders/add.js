angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addPlaceholderElementController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', function ($scope, bladeNavigationService, categories, items) {
	var blade = $scope.blade;

	blade.addFolder = function () {
		var data = { outline: '', name: '', description: '', parentFolderId: blade.chosenFolder};
		blade.parentBlade.addNewFolder(data);
	};

	blade.addPlaceholder = function () {
	    var data = { name: '', description: '', imageUrl: 'Modules/$(VirtoCommerce.Marketing)/Content/images/noimage.png', folderId: blade.chosenFolder };
		blade.parentBlade.addNewPlaceholder(data);
	};

	$scope.blade.isLoading = false;
}]);

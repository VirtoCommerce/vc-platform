angular.module('virtoCommerce.marketingModule')
.controller('addPlaceholderElementController', ['$scope', 'bladeNavigationService', 'categories', 'items', function ($scope, bladeNavigationService, categories, items) {
	var blade = $scope.blade;

	blade.addFolder = function () {
		var data = { id: Math.floor((Math.random() * 1000000000) + 1).toString(), name: '', description: '', parentId: blade.choosenFolder, placeholders: [], childrenFolders: [] };
		blade.parentBlade.addNewFolder(data);
	};

	blade.addPlaceholder = function () {
		var data = { id: Math.floor((Math.random() * 1000000000) + 1).toString(), name: '', description: '', descriptionImageUrl: 'http://mini.s-shot.ru/1024x768/JPEG/1024/Z100/?kitmall.ru', parentId: blade.choosenFolder };
		blade.parentBlade.addNewPlaceholder(data);
	};

	$scope.blade.isLoading = false;
}]);

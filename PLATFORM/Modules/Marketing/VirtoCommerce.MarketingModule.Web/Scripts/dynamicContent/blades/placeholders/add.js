angular.module('virtoCommerce.marketingModule')
.controller('addPlaceholderElementController', ['$scope', 'bladeNavigationService', 'categories', 'items', function ($scope, bladeNavigationService, categories, items) {
	var blade = $scope.blade;

	blade.addFolder = function () {
		var data = { id: '78-77', name: '', description: '', parentId: blade.choosenFolder, placeholders: [] };
		blade.parentBlade.addNewFolder(data);
	};

	blade.addPlaceholder = function () {
		var data = { id: '77-78', name: '', description: '', descriptionImageUrl: 'http://mini.s-shot.ru/1024x768/JPEG/1024/Z100/?kitmall.ru', parentId: blade.choosenFolder };
		blade.parentBlade.addNewPlaceholder(data);
	};

	$scope.blade.isLoading = false;
}]);

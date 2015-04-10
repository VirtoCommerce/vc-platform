angular.module('virtoCommerce.marketingModule')
.controller('addContentItemsElementController', ['$scope', 'bladeNavigationService', 'categories', 'items', function ($scope, bladeNavigationService, categories, items) {
	var blade = $scope.blade;

	blade.addFolder = function () {
		var data = { id: Math.floor((Math.random() * 1000000000) + 1).toString(), name: '', description: '', parentId: blade.choosenFolder, items: [], childrenFolders: [] };
		blade.parentBlade.addNewFolder(data);
	};

	blade.addContentItem = function () {
		var data = { id: Math.floor((Math.random() * 1000000000) + 1).toString(), name: '', description: '', contentType: 'CategoryWithImages', categoryId: '', imageUrl: '', externalImageUrl: '', message: '', categoryCode: '', title: '', sortField: '', itemCount: 1, newItems: false, flashFilePath: '', link1Url: '', link2Url: '', link3Url: '', rawHtml: '', razorHtml: '', alternativeText: '', targetUrl: '', productCode: '', parentId: blade.choosenFolder };
		blade.parentBlade.addNewContentItem(data);
	};

	$scope.blade.isLoading = false;
}]);

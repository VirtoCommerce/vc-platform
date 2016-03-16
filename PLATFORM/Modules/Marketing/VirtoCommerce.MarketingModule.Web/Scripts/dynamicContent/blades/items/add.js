angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addContentItemsElementController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', 'virtoCommerce.marketing.dynamicContent.dynamicProperties', function ($scope, bladeNavigationService, categories, items, dynamicProperties) {
	var blade = $scope.blade;

	blade.addFolder = function () {
		var data = { name: '', description: '', parentFolderId: blade.chosenFolder, items: [], childrenFolders: [] };
		blade.parentBlade.addNewFolder(data);
	};

	blade.addContentItem = function () {
	    dynamicProperties.getDynamicProperties({ typeName: 'VirtoCommerce.Domain.Marketing.Model.DynamicContentItem' }, function (data) {
	        angular.forEach(data, function (item){
	        	item.values = [];
	        	item.displayNames = [];
	        });
	        var contentItem = { name: '', description: '', folderId: blade.chosenFolder, dynamicProperties: data };
	        blade.parentBlade.addNewContentItem(contentItem);
	    });
	};

	$scope.blade.isLoading = false;
}]);

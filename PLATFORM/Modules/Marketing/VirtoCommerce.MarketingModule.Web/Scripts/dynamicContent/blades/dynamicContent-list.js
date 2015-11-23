angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.dynamicContentListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

	$scope.selectedNodeId = null;

	function initializeBlade() {
		var entities = [
            { id: 'DC_1', name: 'marketing.blades.items.list.title', subname: 'marketing.blades.items.list.subtitle', title: 'marketing.blades.dynamicContent-list.menu.items.title', description: 'marketing.blades.dynamicContent-list.menu.items.description', icon: 'fa-inbox', entityName: 'items' },
            { id: 'DC_2', name: 'marketing.blades.placeholders.list.title', subname: 'marketing.blades.placeholders.list.subtitle', title: 'marketing.blades.dynamicContent-list.menu.placeholders.title', description: 'marketing.blades.dynamicContent-list.menu.placeholders.description', icon: 'fa-location-arrow', entityName: 'placeholders' },
            { id: 'DC_3', name: 'marketing.blades.publishing.list.title', subname: 'marketing.blades.publishing.list.subtitle', title: 'marketing.blades.dynamicContent-list.menu.publishing.title', description: 'marketing.blades.dynamicContent-list.menu.publishing.description', icon: 'fa-paperclip', entityName: 'publishing' }];
		blade.currentEntities = entities;
		blade.isLoading = false;
	};

	blade.openBlade = function (data) {
		$scope.selectedNodeId = data.id;

		var newBlade = {
			id: 'dynamicContentList',
			title: data.name,
			subtitle: data.subname,
			controller: 'virtoCommerce.marketingModule.' + data.entityName + 'DynamicContentListController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/' + data.entityName + '/list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	$scope.blade.headIcon = 'fa-calendar-o';

	initializeBlade();
}]);

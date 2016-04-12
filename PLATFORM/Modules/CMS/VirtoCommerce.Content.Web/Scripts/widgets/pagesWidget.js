angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.pagesWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;

    //$scope.openBlade = function () {
    //	var newBlade = {
    //		id: "pagesListBlade",
    //		storeId: blade.currentEntityId,
    //		parentWidget: $scope.widget,
    //		title: blade.title,
    //		subtitle: 'content.widgets.pades.blade-subtitle',
    //		controller: 'virtoCommerce.contentModule.pagesListController',
    //		template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/pages-list.tpl.html'
    //	};
    //	bladeNavigationService.showBlade(newBlade, blade);
    //};

}]);
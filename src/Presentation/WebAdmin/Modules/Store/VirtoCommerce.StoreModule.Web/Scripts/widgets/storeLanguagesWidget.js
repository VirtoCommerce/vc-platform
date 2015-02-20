angular.module('virtoCommerce.storeModule.widgets', [])
.controller('storeLanguagesWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    // var blade = $scope.widget.blade;
    
    $scope.openBlade = function () {
        //if ($scope.elementCount !== '...') {
        //    var newBlade = {
        //        id: "storeLanguages",
        //        itemId: blade.itemId,
        //        title: blade.title,
        //        subtitle: 'Manage languages',
        //        controller: 'storeLanguagesListController',
        //        template: 'Modules/Store/VirtoCommerce.StoreModule.Web/Scripts/blades/stores-languages-list.tpl.html'
        //    };
        //    bladeNavigationService.showBlade(newBlade, blade);
        //}
    };
}]);
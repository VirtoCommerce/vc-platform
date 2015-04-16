angular.module('virtoCommerce.gshoppingModule')
.controller('gshoppingWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;
    $scope.showWidget = blade.currentEntity.id == 'GoogleShopping.Merchant';
}]);
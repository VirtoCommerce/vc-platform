angular.module('virtoCommerce.coreModule.fulfillment')
.controller('fulfillmentCenterContactWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "fulfillmentDetailChild",
            data: blade.currentEntity,
            title: blade.title,
            subtitle: 'Contact information',
            controller: 'fulfillmentCenterContactController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/blades/fulfillment-center-contact.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);
angular.module('virtoCommerce.coreModule.fulfillment')
.controller('virtoCommerce.coreModule.fulfillment.fulfillmentCenterContactWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "fulfillmentDetailChild",
            data: blade.currentEntity,
            title: blade.title,
            subtitle: 'Contact information',
            controller: 'virtoCommerce.coreModule.fulfillment.fulfillmentCenterContactController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/blades/fulfillment-center-contact.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);
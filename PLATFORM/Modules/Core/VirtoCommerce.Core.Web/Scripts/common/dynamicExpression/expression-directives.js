angular.module('virtoCommerce.coreModule')
// .controller('expressionBlockController', )
    .directive('vaExpressionBlock', function () {
        return {
            restrict: 'E',
            templateUrl: 'Modules/$(VirtoCommerce.Core)/Scripts/common/dynamicExpression/expression-block.tpl.html'
        };
    })
    .directive('vaExpressionElement', function () {
        return {
            restrict: 'E',
            scope: {
                element: '='
            },
            template: '<ng-include src="getTemplateUrl()"/>',
            //templateUrl: unfortunately has no access to $scope
            link: function ($scope) {
                //function used on the ng-include to resolve the template
                $scope.getTemplateUrl = function() {
                    // return 'Modules/$(VirtoCommerce.Core)/Scripts/common/dynamicExpression/expression-' + $scope.element.type + '.tpl.html';
                    return 'expression-' + $scope.element.type + '.html';
                };
            }
        };
    });
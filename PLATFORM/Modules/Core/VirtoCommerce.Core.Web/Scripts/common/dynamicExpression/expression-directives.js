angular.module('virtoCommerce.coreModule')
.factory('vaDynamicExpressionService', function () {
    var retVal = {
        expressions: [],
        registerExpression: function (expression) {
            if (!expression.templateURL) {
                expression.templateURL = 'expression-' + expression.id + '.html';
            }

            this.expressions[expression.id] = expression;
        }
    };
    return retVal;
})
.directive('vaDynamicExpressionTree', function () {
    return {
        restrict: 'E',
        //scope: {
        //    source: '='
        //},
        link: function ($scope, $element, $attrs) {
            $scope.addChild = function (chosenMenuElement, parentBlock) {
                if (!parentBlock.children) {
                    parentBlock.children = [];
                }
                parentBlock.children.push(angular.copy(chosenMenuElement));
            };
            $scope.deleteChild = function (child, parentList) {
                parentList.splice(parentList.indexOf(child), 1);
            }

            $scope.$watch($attrs.source, function (newVal) {
                $scope.source = newVal;
            });
        },
        templateUrl: 'Modules/$(VirtoCommerce.Core)/Scripts/common/dynamicExpression/expression-tree.tpl.html'
    };
})
//.directive('vaTemplatedExpression', ['$compile', function ($compile) {
//    return {
//        restrict: 'A',
//        //scope: true,
//        //scope: {
//        //    data: '='
//        //},
//        terminal: true,
//        priority: 1000,
//        link: function (scope, element, attr) {
//            //element.attr('ng-controller', scope.data.controller);
//            //element.attr('ng-model', 'data');
//            element.removeAttr("va-templated-expression");
//            $compile(element)(scope);
//        }
//    }
//}])
;
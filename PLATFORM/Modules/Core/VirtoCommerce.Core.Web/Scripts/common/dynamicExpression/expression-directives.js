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
//.directive('vaExpressionBlock', function () {
//    return {
//        restrict: 'E',
//        scope: {
//            data: '='
//        },
//        link: function ($scope) {
//            $scope.addChild = function (chosenMenuElement, parentList) {
//                if (!parentList.children) {
//                    parentList.children = [];
//                }
//                parentList.children.push(angular.copy(chosenMenuElement));
//            };
//            $scope.deleteChild = function (child, parentList) {
//                parentList.splice(parentList.indexOf(child), 1);
//            }
//        },
//        templateUrl: 'Modules/$(VirtoCommerce.Core)/Scripts/common/dynamicExpression/expression-block.tpl.html'
//    };
//})
//.directive('vaExpressionElement', function () {
//    return {
//        restrict: 'E',
//        scope: {
//            element: '='
//        },
//        template: '<ng-include src="getTemplateUrl()"/>',
//        //templateUrl: unfortunately has no access to $scope
//        link: function ($scope) {
//            //function used on the ng-include to resolve the template
//            $scope.getTemplateUrl = function () {
//                // return 'Modules/$(VirtoCommerce.Core)/Scripts/common/dynamicExpression/expression-' + $scope.element.type + '.tpl.html';
//                return 'expression-' + $scope.element.type + '.html';
//            };
//        }
//    };
//})
;
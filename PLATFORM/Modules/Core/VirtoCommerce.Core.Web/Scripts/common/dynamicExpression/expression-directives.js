angular.module('virtoCommerce.coreModule')
// .controller('expressionBlockController', )
    .directive('vaExpressionBlock', function () {
        return {
            restrict: 'E',
            scope: {
                data: '=',
                expressionData: '='
            },
            link: function ($scope) {
                //function used on the ng-include to resolve the template
                $scope.addChild = $scope.expressionData.addChild;
                $scope.deleteChild = function (child, parentList) {
                    parentList.splice(parentList.indexOf(child), 1);
                }
            },
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
                $scope.getTemplateUrl = function () {
                    // return 'Modules/$(VirtoCommerce.Core)/Scripts/common/dynamicExpression/expression-' + $scope.element.type + '.tpl.html';
                    return 'expression-' + $scope.element.type + '.html';
                };
            }
        };
    })
    .directive('vaContextClick', function () {
        return {
            restrict: 'A',
            scope: '@&',
            compile: function compile(tElement, tAttrs, transclude) {
                return {
                    post: function postLink(scope, iElement, iAttrs, controller) {
                        var ul = $('#' + iAttrs.vaContextClick),
                            last = null;

                        ul.css({ 'display': 'none' });

                        $(iElement).click(function (event) {
                            ul.css({
                                position: "fixed",
                                display: "block",
                                left: event.clientX + 'px',
                                top: event.clientY + 'px'
                            });
                            last = event.timeStamp;
                        });

                        $(iElement).mouseout(function (event) {
                            if (ul.has(event.relatedTarget).length == 0) {
                                ul.css({
                                    'display': 'none'
                                });
                            }
                        });
                        //$(document).click(function (event) {
                        //    var target = $(event.target);
                        //    if (!target.is(".popover") && !target.parents().is(".popover")) {
                        //        if (last === event.timeStamp)
                        //            return;
                        //        ul.css({
                        //            'display': 'none'
                        //        });
                        //    }
                        //});
                    }
                };
            }
        };
    });
angular.module('platformWebApp')
    .directive('vaBreadcrumb', [
        'platformWebApp.breadcrumbHistoryService', function (breadcrumbHistoryService) {
            return {
                restrict: 'E',
                require: 'ngModel',
                replace: true,
                scope: {
                    bladeId: '='
                },
        templateUrl: '$(Platform)/Scripts/app/navigation/breadcrumbs/breadcrumbs.tpl.html',
                link: function (scope, element, attr, ngModelController) {
                    scope.breadcrumbs = [];
                    ngModelController.$render = function () {
                        scope.breadcrumbs = ngModelController.$modelValue;
                    };

                    scope.innerNavigate = function (breadcrumb) {
                        breadcrumb.navigate(breadcrumb);
                    };

                    scope.canNavigateBack = function () {
                        return breadcrumbHistoryService.check(scope.bladeId);
                    };

                    scope.navigateBack = function () {
                        if (scope.canNavigateBack()) {
                            var breadcrumb = breadcrumbHistoryService.pop(scope.bladeId);
                            breadcrumb.navigate(breadcrumb);
                        }
                    };
                    scope.$watchCollection('breadcrumbs', function (newItems) {
                        breadcrumbHistoryService.push(newItems, scope.bladeId);
                    });
                }
            }
        }
    ])
    .factory('platformWebApp.breadcrumbHistoryService', function () {
        var map = {};

        function breadcrumbsEqual(x,y) {
            return x && y && x.id === y.id && x.name === y.name;
        }

        return {
            push: function (breadcrumbs, id) {
                var history = map[id];
                if (!history) {
                    map[id] = history = {
                        ignoreNextAction: false,
                        records: []
                    };
                }

                var currentBreadcrumb = _.last(breadcrumbs);

                if (history.ignoreNextAction) {
                    history.ignoreNextAction = false;
                } else if (history.currentBreadcrumb &&
                            !breadcrumbsEqual(history.currentBreadcrumb, currentBreadcrumb) &&
                            !breadcrumbsEqual(history.currentBreadcrumb, _.last(history.records))) {
                    history.records.push(history.currentBreadcrumb);
                }

                if (currentBreadcrumb) {
                    history.currentBreadcrumb = currentBreadcrumb;
                }
            },

            check: function (id) {
                return map[id] && _.any(map[id].records);
            },

            pop: function (id) {
                var retVal = undefined;
                var history = map[id];
                if (_.any(history.records)) {
                    retVal = history.records.pop();
                    history.ignoreNextAction = true;
                }

                return retVal;
            }
        };
    });
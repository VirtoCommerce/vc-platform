angular.module('platformWebApp')
    .directive('vaBreadcrumb', [
        'platformWebApp.breadcrumbHistoryService', '$translate', '$timeout', function (breadcrumbHistoryService, $translate, $timeout) {
            return {
                restrict: 'E',
                require: 'ngModel',
                replace: true,
                scope: {
                    bladeId: '=',
                    bladeMaximized: '='
                },
                templateUrl: '$(Platform)/Scripts/app/navigation/breadcrumbs/breadcrumbs.tpl.html',
                link: function (scope, element, attr, ngModelController) {
                    var availableWidth = element.width();
                    var originalWidth = availableWidth;

                    $timeout(() => {
                        element.find(".menu.__inline").css('max-width', availableWidth);
                    });

                    scope.breadcrumbs = [];
                    ngModelController.$render = function () {
                        scope.breadcrumbs = ngModelController.$modelValue;
                    };

                    scope.innerNavigate = function (breadcrumb) {
                        breadcrumb.navigate(breadcrumb);
                    };

                    scope.$watch('bladeMaximized', (newVal, oldVal) => {
                        if (newVal === oldVal && newVal === undefined) return;

                        if (newVal) {
                            originalWidth = availableWidth;
                            availableWidth = element.width();
                        } else {
                            availableWidth = originalWidth;
                        }

                        recalculateItemVisibility(scope.breadcrumbs);

                        element.find(".menu.__inline").css('max-width', availableWidth);
                    });

                    scope.$watchCollection('breadcrumbs', (newItems) => {
                        delete scope.expanded;
                        recalculateItemVisibility(newItems);
                        breadcrumbHistoryService.push(newItems, scope.bladeId);
                    });

                    function recalculateItemVisibility(breadcrumbs) {
                        if (scope.expanded || !_.some(breadcrumbs))
                            return;

                        availableWidth = Math.min(availableWidth, element.width());

                        const expanderWidth = 43;
                        var wasLastItemVisible = true;
                        breadcrumbs[0].name = $translate.instant(breadcrumbs[0].name);
                        var widthOfItems = calculateWordWidth(breadcrumbs[0].name);
                        var items = _.rest(breadcrumbs).reverse();

                        for (var i = 0; i < items.length; i++) {
                            var x = items[i];
                            if (wasLastItemVisible) {
                                var wordWidth = calculateWordWidth(x.name);
                                if (widthOfItems + wordWidth < availableWidth) {
                                    widthOfItems += wordWidth;
                                } else {
                                    wasLastItemVisible = false;
                                    widthOfItems += expanderWidth;
                                    if (widthOfItems > availableWidth) {
                                        // hide 1 more because expander took the space
                                        items[i - 1].isVisible = false;
                                    }
                                }
                            }

                            x.isVisible = wasLastItemVisible;
                        }

                        //console.log("Calc. breadcrumbs width: " + widthOfItems + ", avail: " + availableWidth);
                    }

                    function calculateWordWidth(word) {
                        var wordWidth = _.reduce(word, (memo, letter) => memo + (letter === letter.toUpperCase() ? 9 : 5.8), 0);
                        const maxWidthForMenuLinkText = 123;
                        const paddingAndBorders = 32;
                        var result = Math.min(maxWidthForMenuLinkText, wordWidth) + paddingAndBorders;
                        //console.log("[calc. width] " + word + "\t=> " + (result - 6));
                        return result;
                    }

                    scope.expand = () => {
                        for (var x of scope.breadcrumbs) {
                            x.isVisible = true;
                        }

                        scope.expanded = true;

                        $timeout(() => {
                            var el = element;
                            var bladeStatic = el.parent();
                            var requiredStaticTopHeight = bladeStatic[0].offsetHeight + el.height() - 39;
                            bladeStatic.height(requiredStaticTopHeight + "px");

                            var staticBottom = bladeStatic.parent().find(".blade-static.__bottom")[0]
                            var staticBottomHeight = staticBottom ? staticBottom.offsetHeight : 0;
                            bladeStatic.parent().find(".blade-content").height('calc(100% - ' + (requiredStaticTopHeight + staticBottomHeight) + 'px)');
                        });
                    };

                    scope.canExpand = () => {
                        return !scope.expanded && scope.breadcrumbs && _.some(_.rest(scope.breadcrumbs), (breadcrumb) => !breadcrumb.isVisible);
                    };
                }
            }
        }
    ])

    .factory('platformWebApp.breadcrumbHistoryService', function () {
        var map = {};

        function breadcrumbsEqual(x, y) {
            return x && y && x.id === y.id && x.name === y.name;
        }

        var checkInternal = (id) => map[id] && _.any(map[id].records);

        var popInternal = (id) => {
            var retVal = undefined;
            var history = map[id];
            if (_.any(history.records)) {
                retVal = history.records.pop();
                history.ignoreNextAction = true;
            }

            return retVal;
        };

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

            check: checkInternal,

            pop: popInternal,

            getBackButtonInstance: () => angular.copy({
                name: "platform.navigation.back", icon: 'fas fa-arrow-left',
                showSeparator: true,
                executeMethod: function (blade) {
                    var breadcrumb = popInternal(blade.id);
                    breadcrumb.navigate(breadcrumb);
                },
                canExecuteMethod: (blade) => checkInternal(blade.id),
                index: 0
            })
        };
    });

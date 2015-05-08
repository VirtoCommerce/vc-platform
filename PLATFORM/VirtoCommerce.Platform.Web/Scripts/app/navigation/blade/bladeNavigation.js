angular.module('platformWebApp')
.directive('vaBladeContainer', ['$compile', 'bladeNavigationService', function ($compile, bladeNavigationService) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: 'Scripts/app/navigation/blade/bladeContainer.tpl.html',
        link: function (scope) {
            scope.blades = bladeNavigationService.stateBlades();
        }
    }
}])
.directive('vaBlade', ['$compile', 'bladeNavigationService', '$timeout', function ($compile, bladeNavigationService, $timeout) {
    return {
        terminal: true,
        priority: 100,
        link: function (scope, element) {
            element.attr('ng-controller', scope.blade.controller);
            element.attr('id', scope.blade.id);
            element.attr('ng-model', "blade");
            element.removeAttr("va-blade");
            $compile(element)(scope);

            var speed = (window.navigator.platform == 'MacIntel' ? .5 : 40);

            var mainContent = $('.cnt');
            var blade = $(element).parent('.blade');
            var offset = parseInt(blade.offset().left + mainContent.scrollLeft() + blade.width() + 125 - mainContent[0].clientWidth);

            if (!scope.blade.disableOpenAnimation) {
                blade.css('margin-left', '-' + blade.width() + 'px').addClass('__animate');

                setTimeout(function () {
                    blade.animate({ 'margin-left': 0 }, 250, function () {
                        blade.removeAttr('style').removeClass('__animate');
                    });
                }, 0);
            }

            $timeout(function () {

                if (offset > mainContent.scrollLeft()) {
                    mainContent.animate({ scrollLeft: offset + 'px' }, 500);
                }


            }, 0, false);


            //Somehow vertical scrollbar does not work initially so need to turn it on
            // $(element).find('.blade-content').off('mousewheel').on('mousewheel', function (event, delta)
            // {
            //     this.scrollTop -= (delta * speed);
            //     event.preventDefault();
            // });

            $(element).find('.blade-container').on('mouseenter', function (e) {
                var blade = $(this),
                    bladeI = blade.find('.blade-inner'),
                    bladeH = bladeI.height(),
                    bladeIh = blade.find('.inner-block').height() + 5;

                if (blade.length) {
                    if (bladeH <= bladeIh) {
                        horizontalScroll('off');
                    }
                    else {
                        horizontalScroll('on');
                    }
                }
            }).on('mouseleave', function () {
                horizontalScroll('on');
            });

            $('.dashboard-area').on('mouseenter', function (event) {
                var dashboardA = $(this),
                    dashboardH = dashboardA.height(),
                    dashboardIh = dashboardA.find('.dashboard-inner').height();

                if (dashboardA.length) {
                    if (dashboardH <= dashboardIh) {
                        horizontalScroll('off');
                    }
                    else {
                        horizontalScroll('on');
                    }
                }
            }).on('mouseleave', function () {
                horizontalScroll('on');
            });

            function horizontalScroll(flag) {
                if (flag != 'off') {
                    $('.cnt').off('mousewheel').on('mousewheel', function (event, delta) {
                        this.scrollLeft -= (delta * speed);
                        event.preventDefault();
                    });
                }
                else {
                    $('.cnt').unmousewheel();
                }
            }

            scope.bladeMaximize = function () {
                scope.maximized = true;

                var blade = $(element);
                blade.attr('data-width', blade.width());
                var leftMenu = $('.nav-bar');
                var offset = parseInt((blade.offset().left + $('.cnt').scrollLeft()) - leftMenu.width());
                var contentblock = blade.find(".blade-content");
                $(contentblock).animate({ width: (parseInt(window.innerWidth - leftMenu.width()) + 'px') }, 100);
                $(contentblock).find('.inner-block').animate({ width: parseInt(window.innerWidth - leftMenu.width() - 40) + 'px' }, 100);
                $('.cnt').animate({ scrollLeft: offset + 'px' }, 250);
            };

            scope.bladeRestore = function () {
                scope.maximized = false;

                var blade = $(element);
                var blockWidth = blade.data('width');
                var leftMenu = $('.nav-bar');
                blade.removeAttr('data-width');
                var offset = parseInt(blade.offset().left + $('.cnt').scrollLeft() - leftMenu.width());
                var contentblock = blade.find(".blade-content");
                $(contentblock).animate({ width: blockWidth }, 100);
                $(contentblock).find('.inner-block').animate({ width: blockWidth - 40 }, 100);
                $('.cnt').animate({ scrollLeft: offset + 'px' }, 250);
            };

            scope.bladeClose = function (onAfterClose) {
                bladeNavigationService.closeBlade(scope.blade, onAfterClose, function (callback) {
                    blade.addClass('__animate').animate({ 'margin-left': '-' + blade.width() + 'px' }, 125, function () {
                        blade.remove();
                        callback();
                    });
                });
            };

            scope.executeCommand = function (toolbarCommand) {
                if (toolbarCommand.canExecuteMethod())
                    toolbarCommand.executeMethod(scope.blade);
            };
        }
    };
}])
.factory('bladeNavigationService', ['$rootScope', '$timeout', '$state', function ($rootScope, $timeout, $state) {
    var service = {
        blades: [],
        currentBlade: undefined,
        closeBlade: function (blade, callback, onBeforeClosing) {
            //Need in case a copy was passed
            blade = service.findBlade(blade.id);

            // close all children
            closeChildren(blade);

            var idx = service.stateBlades().indexOf(blade);

            var doCloseBlade = function () {
                if (angular.isFunction(onBeforeClosing)) {
                    onBeforeClosing(doCloseBladeFinal);
                } else {
                    doCloseBladeFinal();
                }
            }

            var doCloseBladeFinal = function () {
                service.stateBlades().splice(idx, 1);
                //remove blade from children collection
                if (angular.isDefined(blade.parentBlade)) {
                    var childIdx = blade.parentBlade.childrenBlades.indexOf(blade);
                    if (childIdx >= 0) {
                        blade.parentBlade.childrenBlades.splice(childIdx, 1);
                    }
                }
                if (angular.isFunction(callback)) {
                    $timeout(callback);
                };
            };

            if (idx >= 0) {
                if (angular.isFunction(blade.onClose)) {
                    blade.onClose(doCloseBlade);
                }
                else {
                    doCloseBlade();
                }
            }
        },
        hasBlade: function (id) {
            return service.findBlade(id) !== undefined;
        },
        stateBlades: function (stateName) {
            if (angular.isUndefined(stateName)) {
                stateName = $state.current.name;
            }

            if (angular.isUndefined(service.blades[stateName])) {
                service.blades[stateName] = [];
            }

            return service.blades[stateName];
        },
        findBlade: function (id) {

            var found;
            angular.forEach(service.stateBlades(), function (blade) {
                if (blade.id == id) {
                    found = blade;
                }
            });

            return found;
        },
        showBlade: function (blade, parentBlade) {
            blade.isLoading = true;
            blade.parentBlade = parentBlade;
            blade.childrenBlades = [];

            var existingBlade = service.findBlade(blade.id);

            //Show blade in previous location
            if (existingBlade != undefined) {
                //store prev blade x-index
                blade.xindex = existingBlade.xindex;
            } else if (!angular.isDefined(blade.xindex)) {
                //Show blade as last one by default
                blade.xindex = service.stateBlades().length;
            }

            if (angular.isDefined(parentBlade)) {
                blade.xindex = service.stateBlades().indexOf(parentBlade) + 1;
                closeChildren(parentBlade);
                parentBlade.childrenBlades.push(blade);
            }

            var showBlade = function () {
                //show blade in same place where it been
                service.stateBlades().splice(Math.min(blade.xindex, service.stateBlades().length), 0, blade);
                service.currentBlade = blade;
            };

            if (angular.isDefined(existingBlade) && angular.isUndefined(parentBlade)) {
                service.closeBlade(existingBlade, showBlade);
            } else {
                showBlade();
            }
        },
        setError: function (msg, blade) {
            blade.isLoading = false;
            blade.error = msg;
        }
    };

    function closeChildren(blade) {
        if (blade && blade.childrenBlades) {
            angular.forEach(blade.childrenBlades.slice(), function (child) {
                service.closeBlade(child);
            });
        }
    }

    return service;
}]);
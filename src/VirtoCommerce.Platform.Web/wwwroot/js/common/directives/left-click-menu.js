// leftClickMenu based on: ng-context-menu - v1.0.2 - An AngularJS directive to display a context menu when a right-click event is triggered
angular
    .module('platformWebApp')
    .directive("leftClickMenu", ["$document", "ContextMenuService", function ($document, ContextMenuService) {
        return {
            restrict: 'A',
            scope: {
                'callback': '&leftClickMenu',
                'disabled': '&contextMenuDisabled',
                'closeCallback': '&contextMenuClose'
            },
            link: function ($scope, $element, $attrs) {
                var opened = false;

                function open(event, menuElement) {
                    menuElement.addClass('open');

                    var doc = $document[0].documentElement;
                    var docLeft = (window.pageXOffset || doc.scrollLeft) - (doc.clientLeft || 0);
                    var docTop = (window.pageYOffset || doc.scrollTop) - (doc.clientTop || 0);
                    var elementWidth = menuElement[0].scrollWidth;
                    var elementHeight = menuElement[0].scrollHeight;

                    var docWidth = doc.clientWidth + docLeft;
                    var docHeight = doc.clientHeight + docTop;
                    var totalWidth = elementWidth + event.pageX;
                    var left = Math.max(event.pageX - docLeft, 0);
                    var top = Math.max(event.pageY - docTop, 0);

                    if (totalWidth > docWidth) {
                        left = left - (totalWidth - docWidth);
                    }

                    var maxTopOffset = 0;
                    // get max bottom position of children
                    $.map(menuElement.children(), function (item) {
                        var height = $(item).find('ul').height();
                        var topPosition = $(item).position().top;
                        var bottomPosition = height + topPosition;
                        if (bottomPosition > maxTopOffset)
                            maxTopOffset = bottomPosition;
                    });

                    // get max bottom position of all context menu
                    if (elementHeight > maxTopOffset)
                        maxTopOffset = elementHeight;

                    var scrollbarHeight = 20;
                    if (event.pageY + maxTopOffset > docHeight) {
                        top = top - (event.pageY - docHeight + maxTopOffset + scrollbarHeight);
                    }

                    menuElement.css('top', top + 'px');
                    menuElement.css('left', left + 'px');
                    opened = true;
                }

                function close(menuElement) {
                    menuElement.removeClass('open');

                    if (opened) {
                        $scope.closeCallback();
                    }

                    opened = false;
                }

                $element.bind('click', function (event) {
                    if (!$scope.disabled()) {
                        if (ContextMenuService.menuElement !== null) {
                            close(ContextMenuService.menuElement);
                        }
                        ContextMenuService.menuElement = angular.element(
                            document.getElementById($attrs.target)
                        );
                        ContextMenuService.element = event.target;

                        event.preventDefault();
                        event.stopPropagation();
                        $scope.$apply(function () {
                            $scope.callback({ $event: event });
                        });
                        $scope.$apply(function () {
                            open(event, ContextMenuService.menuElement);
                        });
                    }
                });

                function handleKeyUpEvent(event) {
                    if (!$scope.disabled() && opened && event.keyCode === 27) {
                        $scope.$apply(function () {
                            close(ContextMenuService.menuElement);
                        });
                    }
                }

                function handleClickEvent(event) {
                    if (!$scope.disabled() &&
                        opened &&
                        (event.button !== 2 ||
                            event.target !== ContextMenuService.element)) {
                        $scope.$apply(function () {
                            close(ContextMenuService.menuElement);
                        });
                    }
                }

                $document.bind('keyup', handleKeyUpEvent);
                // Firefox treats a right-click as a click and a contextmenu event
                // while other browsers just treat it as a contextmenu event
                $document.bind('click', handleClickEvent);
                $document.bind('contextmenu', handleClickEvent);

                $scope.$on('$destroy', function () {
                    $document.unbind('keyup', handleKeyUpEvent);
                    $document.unbind('click', handleClickEvent);
                    $document.unbind('contextmenu', handleClickEvent);
                });
            }
        };
    }]);

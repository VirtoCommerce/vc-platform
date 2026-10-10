angular.module('platformWebApp')
    .factory('platformWebApp.toolbarService', function () {
        var toolbarCommandsMap = [];
        return {
            register: function (toolbarItem, toolbarController) {
                var map = toolbarCommandsMap;
                if (!map[toolbarController]) {
                    map[toolbarController] = [];
                }

                map[toolbarController].push(toolbarItem);
                map[toolbarController].sort(function (a, b) {
                    return a.index - b.index;
                });
            },
            tryRegister: function (toolbarItem, toolbarController) {
                var map = toolbarCommandsMap;
                if (!map[toolbarController] || !_.findWhere(map[toolbarController], { name: toolbarItem.name })) {
                    this.register(toolbarItem, toolbarController);
                }
            },
            resolve: function (bladeCommands, toolbarController) {
                var externalCommands = toolbarCommandsMap[toolbarController];
                if (externalCommands) {
                    bladeCommands = angular.copy(bladeCommands || []);

                    _.each(externalCommands, function (newCommand) {
                        var overrideIndex = _.findIndex(bladeCommands, function (bladeCommand) {
                            return bladeCommand.name === newCommand.name;
                        });
                        var deleteCount = overrideIndex >= 0 ? 1 : 0;
                        overrideIndex = overrideIndex >= 0 ? overrideIndex : newCommand.index;

                        bladeCommands.splice(overrideIndex, deleteCount, newCommand);
                    });
                }

                return bladeCommands;
            }
        };
    })
    .directive('vaBladeContainer', ['platformWebApp.bladeNavigationService', function (bladeNavigationService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '$(Platform)/Scripts/app/navigation/blade/bladeContainer.tpl.html',
            link: function (scope) {
                scope.blades = bladeNavigationService.stateBlades();

                // Book mode: a parent blade folded to its spine opens again on click
                scope.openSpine = function (blade) {
                    bladeNavigationService.focusBlade(blade);
                };
            }
        }
    }])
    .directive('vaBlade', ['$compile', 'platformWebApp.bladeNavigationService', 'platformWebApp.toolbarService', '$timeout', '$document', 'platformWebApp.dialogService', function ($compile, bladeNavigationService, toolbarService, $timeout, $document, dialogService) {
        return {
            terminal: true,
            priority: 100,
            link: function (scope, element) {
                element.attr('ng-controller', scope.blade.controller);
                element.attr('id', scope.blade.id);
                element.attr('ng-model', "blade");
                element.removeAttr("va-blade");
                $compile(element)(scope);
                scope.blade.$scope = scope;

                var mainContent = $('.cnt');
                var currentBlade = $(element).parent();
                var parentBlade = currentBlade.prev();
                // book mode measures blades through this element
                scope.blade.$bladeElement = currentBlade;

                if (!scope.blade.disableOpenAnimation) {
                    scope.blade.animated = true;
                    $timeout(function () {
                        scope.blade.animated = false;
                    }, 250);
                }

                function scrollContent(scrollToBlade, scrollToElement) {
                    if (bladeNavigationService.bookMode) {
                        // book mode folds parent blades instead of scrolling them out of view
                        bladeNavigationService.layoutBook();
                        return;
                    }
                    if (!scrollToBlade) {
                        scrollToBlade = scope.blade;
                    }
                    if (!scrollToElement) {
                        scrollToElement = currentBlade;
                    }

                    // we can't just get current blade position (because of animation) or calculate it
                    // via parent position + parent width (because we may open parent and child blade at the same time)
                    // instead, we need to use sum of width of all blades
                    var previousBlades = scrollToElement.prevAll();
                    var previousBladesWidthSum = 0;
                    previousBlades.each(function () {
                        previousBladesWidthSum += $(this).outerWidth();
                    });
                    var scrollLeft = previousBladesWidthSum + scrollToElement.outerWidth(!(scrollToBlade.isExpanded || scrollToBlade.isMaximized)) - mainContent.width();
                    mainContent.animate({ scrollLeft: (scrollLeft > 0 ? scrollLeft : 0) }, 500);
                }

                var updateSize = function () {
                    var contentBlock = currentBlade.find(".blade-content");
                    var containerBlock = currentBlade.find(".blade-container");

                    var bladeWidth = "";
                    var bladeMinWidth = "";

                    if ((scope.blade.isExpandable && scope.blade.isExpanded) || (!scope.blade.isExpandable && scope.blade.isMaximized)) {
                        // minimal required width + container padding
                        bladeMinWidth = 'calc(' + contentBlock.css("min-width") + ' + ' + parseInt(containerBlock.outerWidth() - containerBlock.width()) + 'px)';
                    }

                    if (scope.blade.isExpandable && scope.blade.isExpanded) {
                        var offset = parentBlade.length > 0 ? parentBlade.width() : 0;
                        // free space of view - parent blade size (if exist)
                        bladeWidth = 'calc(100% - ' + offset + 'px)';
                    } else if (!scope.blade.isExpandable && scope.blade.isMaximized) {
                        currentBlade.attr('data-width', currentBlade.outerWidth());
                        bladeWidth = '100%';
                    }

                    currentBlade.width(bladeWidth);
                    currentBlade.css('min-width', bladeMinWidth);

                    setVisibleToolsLimit();
                }

                scope.$watch('blade.isExpanded', function () {
                    // we must recalculate position only at next digest cycle,
                    // because at this time blade UI is not fully (re)initialized
                    // for example, ng-class set classes after this watch called
                    $timeout(function () {
                        updateSize();
                        if (bladeNavigationService.bookMode) {
                            // the blade has its natural width again
                            bladeNavigationService.layoutBook();
                        }
                    }, 0, false);
                });

                scope.$on('$includeContentLoaded', function (event, src) {
                    if (src === scope.blade.template) {
                        // see above
                        $timeout(function () {
                            updateSize();
                            scrollContent();
                        }, 0, false);
                    }
                });

                scope.bladeMaximize = function () {
                    scope.blade.isMaximized = true;
                    updateSize();
                    scrollContent();
                };

                scope.bladeMinimize = function () {
                    scope.blade.isMaximized = false;
                    updateSize();
                    if (bladeNavigationService.bookMode) {
                        bladeNavigationService.layoutBook();
                    }
                };

                scope.bladeClose = function (onAfterClose) {
                    bladeNavigationService.closeBlade(scope.blade, onAfterClose, function (callback) {
                        scope.blade.animated = true;
                        scope.blade.closing = true;
                        $timeout(function () {
                            currentBlade.remove();
                            scrollContent(scope.blade.parentBlade, parentBlade);
                            callback();
                        }, 125, false);
                    });
                };

                scope.moreToolsOpen = false;
                scope.overflowCommands = [];

                // The modern look drops the empty toolbar row of blades without commands (the classic look keeps it)
                scope.hasNoToolbar = function () {
                    return scope.blade.hideToolbar ||
                        (bladeNavigationService.bookMode && !(scope.resolvedToolbarCommands && scope.resolvedToolbarCommands.length));
                };

                scope.$watch('blade.toolbarCommands', function (toolbarCommands) {

                    scope.resolvedToolbarCommands = toolbarService.resolve(toolbarCommands, scope.blade.controller);
                    scope.moreToolsOpen = false;
                    setVisibleToolsLimit();
                }, true);

                var pendingToolsTimeout = null;
                function setVisibleToolsLimit() {
                    scope.moreToolsOpen = false;
                    scope.toolsPerLineCount = scope.resolvedToolbarCommands ? scope.resolvedToolbarCommands.length : 1;

                    // Cancel any pending timeout to avoid race conditions with repeated calls
                    if (pendingToolsTimeout) {
                        $timeout.cancel(pendingToolsTimeout);
                    }

                    pendingToolsTimeout = $timeout(function () {
                        pendingToolsTimeout = null;
                        // Look up toolbar element fresh each time to avoid stale references from ng-if
                        var toolbar = currentBlade.find(".blade-toolbar .menu.__inline");
                        var lis = toolbar.find("li");

                        // Width a toolbar item takes, including the horizontal margins between items (hidden items take none)
                        var itemWidth = function (li) {
                            if (!li.getClientRects().length) {
                                return 0;
                            }
                            var style = window.getComputedStyle(li);
                            return li.getBoundingClientRect().width + parseFloat(style.marginLeft) + parseFloat(style.marginRight);
                        };

                        if (lis.length > 1) {
                            var totalToolsWidth = 0;
                            for (var j = 0; j < lis.length; j++) {
                                totalToolsWidth += itemWidth(lis[j]);
                            }
                            var availableWidth = toolbar.width();

                            if (totalToolsWidth > availableWidth) {
                                var maxToolbarWidth = availableWidth - 46; // the 'more' button is 46px wide
                                var toolsWidth = 0;
                                var i = 0;
                                while (i < lis.length && toolsWidth + itemWidth(lis[i]) <= maxToolbarWidth) {
                                    toolsWidth += itemWidth(lis[i]);
                                    i++;
                                }
                                scope.toolsPerLineCount = Math.max(i, 1);

                            }
                        }
                        updateOverflowCommands();
                    }, 220);
                }

                function updateOverflowCommands() {
                    if (scope.resolvedToolbarCommands && scope.toolsPerLineCount < scope.resolvedToolbarCommands.length) {
                        scope.overflowCommands = scope.resolvedToolbarCommands.slice(scope.toolsPerLineCount);
                    } else {
                        scope.overflowCommands = [];
                    }
                }

                function handleClickEvent() {
                    scope.$apply(function () {
                        scope.moreToolsOpen = false;
                    });
                    $document.unbind('click', handleClickEvent);
                }

                scope.dropdownStyle = {};
                scope.showMoreTools = function (event) {
                    scope.moreToolsOpen = !scope.moreToolsOpen;
                    event.stopPropagation();
                    if (scope.moreToolsOpen) {
                        // Calculate fixed position for dropdown relative to the toolbar bottom
                        var toolbar = currentBlade.find(".blade-toolbar")[0];
                        if (toolbar) {
                            var rect = toolbar.getBoundingClientRect();
                            scope.dropdownStyle = {
                                top: Math.round(rect.bottom) + 'px',
                                right: Math.round(document.documentElement.clientWidth - rect.right) + 'px'
                            };
                        }
                        $document.bind('click', handleClickEvent);
                    } else {
                        $document.unbind('click', handleClickEvent);
                    }
                };

                // Re-fit the toolbar whenever the blade changes width (folded/unfolded in book mode, small screens)
                var bladeResizeObserver;
                if (typeof ResizeObserver !== 'undefined') {
                    var lastBladeWidth = 0;
                    bladeResizeObserver = new ResizeObserver(function (entries) {
                        var width = Math.round(entries[0].contentRect.width);
                        if (width !== lastBladeWidth) {
                            lastBladeWidth = width;
                            // inside a digest, so all commands are rendered again before they are measured
                            scope.$applyAsync(setVisibleToolsLimit);
                        }
                    });
                    bladeResizeObserver.observe(currentBlade[0]);
                }

                scope.$on('$destroy', function () {
                    $document.unbind('click', handleClickEvent);
                    if (bladeResizeObserver) {
                        bladeResizeObserver.disconnect();
                    }
                });

                scope.showErrorDetails = function () {
                    var dialog = {
                        id: "errorDetails",
                        title: 'platform.dialogs.error-details.title'
                    };
                    if (scope.blade.errorBody != undefined)
                        dialog.message = scope.blade.errorBody;
                    dialogService.showErrorDialog(dialog);
                };

                scope.clearError = function () {
                    bladeNavigationService.clearError(scope.blade);
                };
            }
        }
    }])
    .factory('platformWebApp.bladeNavigationService', ['platformWebApp.authService', '$rootScope', '$timeout', '$state', '$translate', 'platformWebApp.dialogService', function (authService, $rootScope, $timeout, $state, $translate, dialogService) {
        function showConfirmationIfNeeded(showConfirmation, canSave, blade, saveChangesCallback, closeCallback, saveTitle, saveMessage) {
            if (showConfirmation) {
                var dialog = { id: "confirmCurrentBladeClose" };

                if (canSave) {
                    dialog.title = saveTitle;
                    dialog.message = saveMessage;
                } else {
                    dialog.title = "Warning";
                    dialog.message = "Validation failed for this object. Will you continue editing and save later?";
                }

                dialog.callback = function (userChoseYes) {
                    if (canSave) {
                        if (userChoseYes) {
                            saveChangesCallback();
                        }
                        closeCallback();
                    } else if (!userChoseYes) {
                        closeCallback();
                    }
                };

                dialogService.showConfirmationDialog(dialog);
            }
            else {
                closeCallback();
            }
        }

        // Extracts the error details from a response body. Returns undefined when the body carries no details
        // (e.g. a ProblemDetails object without 'errors'), so the caller can fall back to the generic error.
        function getErrorBody(data) {
            if (angular.isArray(data)) {
                // Bare list of validation errors, e.g. [{ propertyName: '...', errorMessage: '...' }]
                var messages = _.filter(_.map(data, function (item) {
                    return angular.isString(item) ? item : item && item.errorMessage;
                }), function (message) { return !!message; });
                return messages.length ? messages.join('<br>') : undefined;
            }

            if (data.exceptionMessage || data.message) {
                return data.exceptionMessage || data.message;
            }

            return angular.isArray(data.errors) ? data.errors.join('<br>') : undefined;
        }

        function clearError(blade) {
            if (blade) {
                blade.isLoading = false;
                blade.error = undefined;
                blade.errorBody = "";
            }
        }

        // Book mode: when the open blades do not fit the workspace, the blades farthest from the focused
        // one fold to a narrow spine instead of being scrolled out of view. A folded blade keeps its DOM,
        // scope and state (it is only clipped), so module blades and grids behave exactly as before.
        // Keep in sync with $bladeSpineWidth + $bladeGap in _modern.sass.
        var bookBladeGap = 12;
        // Keep in sync with $mobileBreakpoint in _modern-mobile.sass
        var bookMobileBreakpoint = 768;        var bookSpineOuterWidth = 56 + bookBladeGap;
        var bookLayoutPromise;
        var bookResizeObserver;

        function isFullWidth(blade) {
            return blade.isMaximized || (blade.isExpandable && blade.isExpanded);
        }

        function scrollToBookBlade(blade) {
            var mainContent = $('.cnt');
            var bladeElement = blade && blade.$bladeElement;
            if (!mainContent.length || !bladeElement || !bladeElement.length) {
                return;
            }

            var previousWidth = 0;
            bladeElement.prevAll('.blade').each(function () {
                previousWidth += $(this).outerWidth(true);
            });
            var scrollLeft = previousWidth + bladeElement.outerWidth(true) - mainContent.width();
            mainContent.stop(true).animate({ scrollLeft: Math.max(scrollLeft, 0) }, 300);
        }

        function observeWorkspaceResize() {
            if (bookResizeObserver || typeof ResizeObserver === 'undefined') {
                return;
            }

            var workspace = document.querySelector('.cnt');
            if (workspace) {
                bookResizeObserver = new ResizeObserver(function () {
                    service.layoutBook();
                });
                bookResizeObserver.observe(workspace);
            }
        }

        function doLayoutBook() {
            var blades = service.stateBlades();
            var focusIndex = blades.indexOf(service.focusedBlade);
            if (focusIndex < 0) {
                focusIndex = blades.length - 1;
            }
            var focusBlade = blades[focusIndex];

            _.each(blades, function (blade) {
                blade.isHiddenSpine = false;
            });

            if (service.bookMode && blades.length > 1 && window.innerWidth < bookMobileBreakpoint) {
                // Small screens: only the focused blade is open; its direct neighbours stay reachable as spines
                observeWorkspaceResize();
                _.each(blades, function (blade, index) {
                    blade.isSpine = index !== focusIndex;
                    blade.isHiddenSpine = Math.abs(index - focusIndex) > 1;
                });
                return;
            }

            if (!service.bookMode || blades.length < 2 || isFullWidth(focusBlade)) {
                _.each(blades, function (blade) {
                    blade.isSpine = false;
                });
                return;
            }

            observeWorkspaceResize();

            var workspaceInner = $('.cnt-inner');
            // widths include each blade's right margin; the last one may run past the edge
            var available = workspaceInner.width() + bookBladeGap;

            // Measure the natural width of each blade. Expanded/maximized blades stretch to the workspace, so
            // their size-class minimum (min-width of .blade-content) is used instead; a folded blade keeps the
            // width it had when it was last measured.
            var widths = _.map(blades, function (blade) {
                var element = blade.$bladeElement;
                if (!blade.isSpine && element && element.length) {
                    var margin = element.outerWidth(true) - element.outerWidth();
                    var content = element.find('.blade-content').first();
                    var minWidth = content.length ? parseFloat(content.css('min-width')) : 0;
                    var stretched = blade.isMaximized || blade.isExpanded || element.hasClass('__expanded') || element.hasClass('__maximized');
                    blade.$bookWidth = stretched && minWidth > 0 ? minWidth + margin : element.outerWidth(true);
                }
                return blade.$bookWidth || 0;
            });

            var isOpen = {};
            isOpen[focusIndex] = true;
            var used = widths[focusIndex] + (blades.length - 1) * bookSpineOuterWidth;

            var tryOpen = function (index) {
                var width = used - bookSpineOuterWidth + widths[index];
                if (width <= available) {
                    isOpen[index] = true;
                    used = width;
                    return true;
                }
                return false;
            };

            // Children of the focused blade first, then its parents, as long as they fit
            for (var i = focusIndex + 1; i < blades.length && tryOpen(i); i++) { }
            for (var j = focusIndex - 1; j >= 0 && tryOpen(j); j--) { }

            _.each(blades, function (blade, index) {
                blade.isSpine = !isOpen[index];
            });

            // scroll after the digest has applied the folded widths
            $timeout(function () {
                scrollToBookBlade(focusBlade);
            }, 0, false);
        }

        var service = {
            blades: [],
            currentBlade: undefined,
            bookMode: false,
            focusedBlade: undefined,
            setBookMode: function (enabled) {
                service.bookMode = !!enabled;
                service.layoutBook();
            },
            focusBlade: function (blade) {
                service.focusedBlade = blade;
                service.layoutBook();
            },
            layoutBook: function () {
                if (bookLayoutPromise) {
                    $timeout.cancel(bookLayoutPromise);
                }
                bookLayoutPromise = $timeout(function () {
                    bookLayoutPromise = null;
                    doLayoutBook();
                }, 0);
            },
            showConfirmationIfNeeded: showConfirmationIfNeeded,
            closeBlade: function (blade, callback, onBeforeClosing) {
                //Need in case a copy was passed
                blade = service.findBlade(blade.id, blade.navigationGroup) || blade;

                // close all children
                service.closeChildrenBlades(blade, function () {
                    var doCloseBlade = function () {
                        if (angular.isFunction(onBeforeClosing)) {
                            onBeforeClosing(doCloseBladeFinal);
                        } else {
                            doCloseBladeFinal();
                        }
                    }

                    var doCloseBladeFinal = function () {
                        var idx = service.stateBlades().indexOf(blade);
                        if (idx >= 0) service.stateBlades().splice(idx, 1);

                        //remove blade from children collection and set current blade
                        if (angular.isDefined(blade.parentBlade)) {
                            var childIdx = blade.parentBlade.childrenBlades.indexOf(blade);
                            if (childIdx >= 0) {
                                blade.parentBlade.childrenBlades.splice(childIdx, 1);
                            }
                            service.currentBlade = blade.parentBlade;
                        }
                        if (service.focusedBlade === blade) {
                            service.focusedBlade = blade.parentBlade;
                        }
                        service.layoutBook();
                        if (angular.isFunction(callback)) {
                            callback();
                        }
                    };

                    if (angular.isFunction(blade.onClose)) {
                        blade.onClose(doCloseBlade);
                    }
                    else {
                        doCloseBlade();
                    }
                });

                if (blade.parentBlade && blade.parentBlade.isExpandable) {
                    blade.parentBlade.isExpanded = true;
                    if (angular.isFunction(blade.parentBlade.onExpand)) {
                        blade.parentBlade.onExpand();
                    }
                }
            },
            closeChildrenBlades: function (blade, callback) {
                if (blade && _.any(blade.childrenBlades)) {
                    angular.forEach(blade.childrenBlades.slice(), function (child) {
                        service.closeBlade(child, function () {
                            // show only when all children were closed
                            if (blade.childrenBlades.length == 0 && angular.isFunction(callback)) {
                                callback();
                            }
                        });
                    });
                } else if (angular.isFunction(callback)) {
                    callback();
                }
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
            findBlade: function (id, navigationGroup) {
                var found;
                angular.forEach(service.stateBlades(), function (blade) {
                    if (blade.id == id && blade.navigationGroup == navigationGroup) {
                        found = blade;
                    }
                });

                return found;
            },
            showBlade: function (blade, parentBlade) {
                //If it is first blade for state try to open saved blades
                //var firstStateBlade = service.stateBlades($state.current.name)[0];
                //if (angular.isDefined(firstStateBlade) && firstStateBlade.id == blade.id) {
                //    service.currentBlade = firstStateBlade;
                //    return;
                //}
                blade.errorBody = "";
                blade.isLoading = true;
                blade.childrenBlades = [];
                if (parentBlade) {
                    blade.headIcon = blade.headIcon || parentBlade.headIcon;
                    blade.updatePermission = blade.updatePermission || parentBlade.updatePermission;
                    blade.navigationGroup = blade.navigationGroup || parentBlade.navigationGroup;
                }
                //copy securityscopes from parent blade
                if (parentBlade != null && parentBlade.securityScopes) {
                    //need merge scopes
                    if (angular.isArray(blade.securityScopes) && angular.isArray(parentBlade.securityScopes)) {
                        blade.securityScopes = parentBlade.securityScopes.concat(blade.securityScopes);
                    }
                    else {
                        blade.securityScopes = parentBlade.securityScopes;
                    }
                }

                var existingBlade = service.findBlade(blade.id, blade.navigationGroup);

                //Show blade in previous location
                if (existingBlade != undefined) {
                    parentBlade = existingBlade.parentBlade;
                    //store prev blade x-index
                    blade.xindex = existingBlade.xindex;
                } else if (!angular.isDefined(blade.xindex)) {
                    //Show blade as last one by default
                    blade.xindex = service.stateBlades().length;
                }

                blade.parentBlade = parentBlade;

                var showBlade = function () {
                    if (angular.isDefined(parentBlade)) {
                        blade.xindex = service.stateBlades().indexOf(parentBlade) + 1;
                        parentBlade.childrenBlades.push(blade);
                    }
                    //show blade in same place where it was
                    service.stateBlades().splice(Math.min(blade.xindex, service.stateBlades().length), 0, blade);
                    service.currentBlade = blade;
                    service.focusedBlade = blade;
                };

                if (angular.isDefined(parentBlade) && parentBlade.childrenBlades.length > 0) {
                    service.closeChildrenBlades(parentBlade, showBlade);
                }
                else if (angular.isDefined(existingBlade)) {
                    service.closeBlade(existingBlade, showBlade);
                }
                else {
                    $timeout(function () {
                        showBlade();
                    });
                }

                if (parentBlade && parentBlade.isExpandable && parentBlade.isExpanded) {
                    parentBlade.isExpanded = false;
                    if (angular.isFunction(parentBlade.onCollapse)) {
                        parentBlade.onCollapse();
                    }
                }

                if (blade.isExpandable) {
                    blade.isExpanded = true;
                    if (angular.isFunction(blade.onExpand)) {
                        blade.onExpand();
                    }
                }

                blade.hasUpdatePermission = function () {
                    return authService.checkPermission(blade.updatePermission, blade.securityScopes);
                };
            },
            checkPermission: authService.checkPermission,
            setError: function (response, blade) {
                if (blade) {
                    blade.isLoading = false;
                    if (response) {
                        response.statusText = service.getStatusText(response);
                        blade.error = response.status && response.statusText ? response.status + ': ' + response.statusText : response;
                        var errorBody = response.data ? getErrorBody(response.data) : undefined;
                        blade.errorBody = errorBody !== undefined ? errorBody : blade.errorBody || blade.error;
                    }
                    else {
                        clearError(blade);
                    }
                }
            },
            clearError: clearError,
            // Drops every blade stack, for every state, without running onClose/confirmation
            // handlers: the session those blades belonged to is already gone, so there is nothing
            // left to save and nobody to prompt.
            clearBlades: function () {
                angular.forEach(Object.keys(service.blades), function (stateName) {
                    // Empty in place rather than rebinding to a new array: vaBladeContainer
                    // captures this array by reference in its link function and ng-repeat renders
                    // that reference. Replacing it would leave the mounted blades on screen with
                    // live scopes, and send every later showBlade into an array no view is bound to.
                    service.blades[stateName].length = 0;
                });
                service.currentBlade = undefined;
            },
            getStatusText: function (response) {
                if (response.statusText === "") {
                    var errorKey = 'platform.errors.' + response.status.toString();
                    var result = $translate.instant(errorKey);
                    if (errorKey === result) {
                        result = $translate.instant('platform.errors.generic-error');
                    }

                    return result;
                }
                return response.statusText;
            }
        };

        // Blade stacks are keyed by state name and are meant to outlive state changes, so switching
        // workspaces and coming back restores what you had open. Nothing tore them down on sign-out,
        // though: signing out only opens loginDialog over the current state, so the blades - and
        // their scopes, watches and $rootScope listeners - stayed live. Signing in again in the same
        // tab then left two generations of the same blade mounted at once, including two ui-grids
        // sharing one 'gridState:<template>' $localStorage key; ngStorage deep-watches that object on
        // $rootScope, so each grid's saveState() woke the other and the digest never settled.
        $rootScope.$on('loginStatusChanged', function (event, authContext) {
            if (!authContext.isAuthenticated) {
                service.clearBlades();
            }
        });

        return service;
    }]);

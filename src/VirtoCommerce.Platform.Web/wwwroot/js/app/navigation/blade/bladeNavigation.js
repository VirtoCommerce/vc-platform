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

                if (!scope.blade.disableOpenAnimation) {
                    scope.blade.animated = true;
                    $timeout(function () {
                        scope.blade.animated = false;
                    }, 250);
                }

                function scrollContent(scrollToBlade, scrollToElement) {
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
                    $timeout(updateSize, 0, false);
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

                scope.$watch('blade.toolbarCommands', function (toolbarCommands) {
                    scope.resolvedToolbarCommands = toolbarService.resolve(toolbarCommands, scope.blade.controller);

                    setVisibleToolsLimit();
                }, true);

                var toolbar = currentBlade.find(".blade-toolbar .menu.__inline");

                function setVisibleToolsLimit() {
                    scope.toolsPerLineCount = scope.resolvedToolbarCommands ? scope.resolvedToolbarCommands.length : 1;

                    $timeout(function () {
                        if (toolbar.height() > 55 && scope.toolsPerLineCount > 1) {
                            var maxToolbarWidth = toolbar.width() - 46; // the 'more' button is 46px wide
                            //console.log(toolbar.width() + 'maxToolbarWidth: ' + maxToolbarWidth);
                            var toolsWidth = 0;
                            var lis = toolbar.find("li");
                            var i = 0;
                            while (maxToolbarWidth > toolsWidth && lis.length > i) {
                                toolsWidth += lis[i].clientWidth;
                                i++;
                            }
                            scope.toolsPerLineCount = i - 1;
                        }
                    }, 220);
                }

                function handleClickEvent() {
                    setVisibleToolsLimit();
                    $document.unbind('click', handleClickEvent);
                }

                scope.showMoreTools = function (event) {
                    scope.toolsPerLineCount = scope.resolvedToolbarCommands.length;

                    event.stopPropagation();
                    $document.bind('click', handleClickEvent);
                };

                scope.showErrorDetails = function () {
                    var dialog = { id: "errorDetails" };
                    if (scope.blade.errorBody != undefined)
                        dialog.message = scope.blade.errorBody;
                    dialogService.showDialog(dialog, '$(Platform)/Scripts/app/modularity/dialogs/errorDetails-dialog.tpl.html', 'platformWebApp.confirmDialogController');
                };
            }
        }
    }])
    .factory('platformWebApp.bladeNavigationService', ['platformWebApp.authService', '$timeout', '$state', 'platformWebApp.dialogService', function (authService, $timeout, $state, dialogService) {
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

        var service = {
            blades: [],
            currentBlade: undefined,
            showConfirmationIfNeeded: showConfirmationIfNeeded,
            closeBlade: function (blade, callback, onBeforeClosing) {
                //Need in case a copy was passed
                blade = service.findBlade(blade.id) || blade;

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

                        //remove blade from children collection
                        if (angular.isDefined(blade.parentBlade)) {
                            var childIdx = blade.parentBlade.childrenBlades.indexOf(blade);
                            if (childIdx >= 0) {
                                blade.parentBlade.childrenBlades.splice(childIdx, 1);
                            }
                        }
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
                //If it is first blade for state try to open saved blades
                //var firstStateBlade = service.stateBlades($state.current.name)[0];
                //if (angular.isDefined(firstStateBlade) && firstStateBlade.id == blade.id) {
                //    service.currentBlade = firstStateBlade;
                //    return;
                //}
                blade.errorBody = "";
                blade.isLoading = true;
                blade.parentBlade = parentBlade;
                blade.childrenBlades = [];
                if (parentBlade) {
                    blade.headIcon = blade.headIcon || parentBlade.headIcon;
                    blade.updatePermission = blade.updatePermission || parentBlade.updatePermission;
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

                var existingBlade = service.findBlade(blade.id);

                //Show blade in previous location
                if (existingBlade != undefined) {
                    //store prev blade x-index
                    blade.xindex = existingBlade.xindex;
                } else if (!angular.isDefined(blade.xindex)) {
                    //Show blade as last one by default
                    blade.xindex = service.stateBlades().length;
                }

                var showBlade = function () {
                    if (angular.isDefined(parentBlade)) {
                        blade.xindex = service.stateBlades().indexOf(parentBlade) + 1;
                        parentBlade.childrenBlades.push(blade);
                    }
                    //show blade in same place where it was
                    service.stateBlades().splice(Math.min(blade.xindex, service.stateBlades().length), 0, blade);
                    service.currentBlade = blade;
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
                if (blade && response) {
                    blade.isLoading = false;
                    blade.error = response.status && response.statusText ? response.status + ': ' + response.statusText : response;
                    blade.errorBody = response.data ? response.data.exceptionMessage || response.data.message || response.data.errors.join('<br>') : blade.errorBody || blade.error;
                }
            }
        };

        return service;
    }]);

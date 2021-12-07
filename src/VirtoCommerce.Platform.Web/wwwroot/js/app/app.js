angular.module('platformWebApp', AppDependencies).controller('platformWebApp.appCtrl', ['$rootScope', '$scope', 'platformWebApp.mainMenuService',
    'platformWebApp.i18n', 'platformWebApp.modules', '$state', 'platformWebApp.bladeNavigationService', 'platformWebApp.userProfile', 'platformWebApp.settings', 'platformWebApp.common',
    function ($rootScope, $scope, mainMenuService,
        i18n, modules, $state, bladeNavigationService, userProfile, settings, common) {

        $scope.closeError = function () {
            $scope.platformError = undefined;
        };

        $scope.$on('httpError', function (event, error) {
            if (!event.defaultPrevented) {
                if (bladeNavigationService.currentBlade) {
                    bladeNavigationService.setError(error, bladeNavigationService.currentBlade);
                }
            }
        });

        $scope.$on('httpRequestSuccess', function (event, data) {
            // clear error on blade cap
            if (bladeNavigationService.currentBlade) {
                bladeNavigationService.currentBlade.error = undefined;
            }
        });

        $scope.$on('loginStatusChanged', function (event, authContext) {
            $scope.isAuthenticated = authContext.isAuthenticated;
        });

        $scope.$on('loginStatusChanged', function (event, authContext) {
            //reset menu to default state
            angular.forEach(mainMenuService.menuItems, function (menuItem) { mainMenuService.resetMenuItemDefaults(menuItem); });

            if (authContext.isAuthenticated) {
                modules.query().$promise.then(function (results) {
                    var modulesWithErrors = _.filter(results, function (x) { return _.any(x.validationErrors) && x.isInstalled; });
                    if (_.any(modulesWithErrors)) {
                        $scope.platformError = {
                            title: modulesWithErrors.length + " modules are loaded with errors and require your attention.",
                            detail: ''
                        };
                        _.each(modulesWithErrors, function (x) {
                            var moduleErrors = "<br/><br/><b>" + x.id + "</b> " + x.version + "<br/>" + x.validationErrors.join("<br/>");
                            $scope.platformError.detail += moduleErrors;
                        });
                        $state.go('workspace.modularity');
                    }
                });

                userProfile.load().then(function () {
                    i18n.changeLanguage(userProfile.language);
                    i18n.changeRegionalFormat(userProfile.regionalFormat);
                    i18n.changeTimeZone(userProfile.timeZone);
                    i18n.changeTimeAgoSettings(userProfile.timeAgoSettings);
                    initializeMainMenu(userProfile);
                });
            }
        });

        // TODO: Fix me! we need to detect scripts, not languages + we use to letter language codes only
        $rootScope.$on('$translateChangeSuccess', function () {
            var rtlLanguages = ['ar', 'arc', 'bcc', 'bqi', 'ckb', 'dv', 'fa', 'glk', 'he', 'lrc', 'mzn', 'pnb', 'ps', 'sd', 'ug', 'ur', 'yi'];
            $rootScope.isRTL = rtlLanguages.indexOf(i18n.getLanguage()) >= 0;
        });

        $scope.mainMenu = {};
        $scope.mainMenu.items = mainMenuService.menuItems;

        $scope.onMainMenuChanged = function (mainMenu) {
            if ($scope.isAuthenticated) {
                saveMainMenuState(mainMenu, userProfile);
            }
        };

        function initializeMainMenu(profile) {
            if (profile.mainMenuState) {
                $scope.mainMenu.isCollapsed = profile.mainMenuState.isCollapsed;
                angular.forEach(profile.mainMenuState.items, function (x) {
                    var existItem = mainMenuService.findByPath(x.path);
                    if (existItem) {
                        angular.extend(existItem, x);
                    }
                });
            }
        }

        function saveMainMenuState(mainMenu, profile) {
            if (mainMenu && profile.$resolved) {
                profile.mainMenuState = {
                    isCollapsed: mainMenu.isCollapsed,
                    items: _.map(_.filter(mainMenu.items,
                        function (x) { return !x.isAlwaysOnBar; }),
                        function (x) { return { path: x.path, isCollapsed: x.isCollapsed, isFavorite: x.isFavorite, order: x.order }; })
                };
                profile.save();
            }
        }

        settings.getUiCustomizationSetting(function (uiCustomizationSetting) {

            $rootScope.uiCustomization = {};

            //Set top panel logo icons
            if (uiCustomizationSetting.value) {
                $rootScope.uiCustomization = angular.fromJson(uiCustomizationSetting.value);
                if (!$rootScope.uiCustomization.topPanelLogo_default) {
                    $rootScope.uiCustomization.topPanelLogo_default = {
                        url: '/images/logo.svg',
                    }
                }
                if (!$rootScope.uiCustomization.topPanelLogo_mini) {
                    $rootScope.uiCustomization.topPanelLogo_mini = {
                        url: '/images/logo-small.svg',
                    }
                }
            }

            common.getLoginPageUIOptions(function (loginPageUIOptions) {

                if (!$rootScope.uiCustomization.background && !$rootScope.uiCustomization.pattern) {
                    $rootScope.uiCustomization.background = {
                        url: loginPageUIOptions.backgroundUrl
                    };
                    $rootScope.uiCustomization.pattern = {
                        url: loginPageUIOptions.patternUrl
                    };
                }
            })
        });

        // DO NOT CHANGE THE FUNCTION BELOW: COPYRIGHT VIOLATION
        $scope.initExpiration = function (x) {
            if (x && x.expirationDate) {
                x.hasExpired = new Date(x.expirationDate) < new Date();
            }
            return x;
        };

        $scope.showVersionInfo = function () {
            $state.go('workspace.systemInfo');
        };

    }])
    .factory('platformWebApp.httpErrorInterceptor', ['$q', '$rootScope', '$injector', 'platformWebApp.authDataStorage', function ($q, $rootScope, $injector, authDataStorage) {
        var httpErrorInterceptor = {};

        httpErrorInterceptor.request = function (config) {
            // Need to pass localization request despite on the auth state
            if (config.url == 'api/platform/localization') {
                return config;
            }
            config.headers = config.headers || {};
            return extractAuthData()
                .then(function (authData) {
                    if (authData) {
                        config.headers.Authorization = 'Bearer ' + authData.token;
                    }
                    return config;
                }).finally(function () {
                    // do something on success
                    if (!config.cache) {
                        $rootScope.$broadcast('httpRequestSuccess', config);
                    }
                });
        };

        function extractAuthData() {
            var authData = authDataStorage.getStoredData();
            if (!authData) {
                return $q.resolve();
            }
            if (Date.now() < authData.expiresAt) {
                return $q.resolve(authData);
            }
            var authService = $injector.get('platformWebApp.authService');
            return authService.refreshToken();
        }

        httpErrorInterceptor.responseError = function (rejection) {
            if (rejection.status === 401) {
                $rootScope.$broadcast('unauthorized', rejection);
            }
            else {
                $rootScope.$broadcast('httpError', rejection);
            }
            return $q.reject(rejection);
        };
        httpErrorInterceptor.requestError = function (rejection) {
            $rootScope.$broadcast('httpError', rejection);
            return $q.reject(rejection);
        };

        return httpErrorInterceptor;
    }])
    .factory('translateLoaderErrorHandler', function ($q, $log) {
        return function (part, lang) {
            $log.error('Localization "' + part + '" for "' + lang + '" was not loaded.');

            //todo add notification.

            //2) You have to either resolve the promise with a translation table for the given part and language or reject it 3) The partial loader will use the given translation table like it was successfully fetched from the server 4) If you reject the promise, then the loader will reject the whole loading process
            return $q.when({});
        };
    })
    .factory('fileUploaderOptions', function () {
        // do not add dynamic headers here, as the service is initialized once and no updates will be made
        return {
            url: '/',
            alias: 'file',
            queue: [],
            progress: 0,
            autoUpload: false,
            removeAfterUpload: false,
            method: 'POST',
            filters: [],
            formData: [],
            queueLimit: Number.MAX_VALUE,
            withCredentials: false
        };
    })
    .factory('platformWebApp.WaitForRestart', ['$window', '$timeout', function ($window, $timeout) {
        return function waitForRestart(delay) {
            if (delay > 60000) // if we waited for a minute, there is something wrong, so let user see
            {
                $window.location.reload();
            }

            var url = "images/logo.png?now=" + Math.random();
            var img = new Image();
            img.src = url;

            img.onload = function () {
                // If the server is up, do this.
                $window.location.reload();
            }

            img.onerror = function () {
                delay += 1000;
                // If the server is down, do that.
                return $timeout(function () { }, delay).then(function () {
                    return waitForRestart(delay);
                });
            }
        }
    }])
    .config(['$provide', function ($provide) {
        $provide.decorator('FileUploader', ['$delegate', 'platformWebApp.authDataStorage', function (FileUploader, authDataStorage) {
            // inject auth header for all FileUploader instance
            FileUploader.prototype._onAfterAddingFile = function (item) {
                var authData = authDataStorage.getStoredData();
                var authHeaders = authData ? { Authorization: 'Bearer ' + authData.token } : {};
                item.headers = angular.extend({}, item.headers, authHeaders);
                this.onAfterAddingFile(item);
            };
            return FileUploader;
        }]);
    }])
    .config(['$provide', function ($provide) {
        $provide.decorator('GridOptions', ['$delegate', 'uiGridConstants', function (GridOptions, uiGridConstants) {
            // create GridOptions object for each grid from base GridOptions object
            var gridOptions = angular.copy(GridOptions);
            gridOptions.initialize = function (options) {
                // initialize it with default values
                var initialOptions = GridOptions.initialize(options);

                // override default values here
                initialOptions.enableVerticalScrollbar = uiGridConstants.scrollbars.WHEN_NEEDED;
                initialOptions.enableHorizontalScrollbar = uiGridConstants.scrollbars.WHEN_NEEDED;

                return initialOptions;
            };
            return gridOptions;
        }]);
    }])
    .config(['$stateProvider', '$httpProvider', 'uiSelectConfig', 'datepickerConfig', 'datepickerPopupConfig', 'tagsInputConfigProvider', '$compileProvider',
        function ($stateProvider, $httpProvider, uiSelectConfig, datepickerConfig, datepickerPopupConfig, tagsInputConfigProvider, $compileProvider) {

            RegExp.escape = function (str) {
                return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
            };

            $stateProvider.state('workspace', {
                url: '/workspace',
                templateUrl: '$(Platform)/Scripts/app/workspace.tpl.html'
            });

            //Add interceptor
            $httpProvider.interceptors.push('platformWebApp.httpErrorInterceptor');
            //ui-select set selectize as default theme
            uiSelectConfig.theme = 'select2';

            datepickerConfig.showWeeks = false;
            datepickerPopupConfig.datepickerPopup = "mediumDate";

            tagsInputConfigProvider.setDefaults('tagsInput', {
                addOnEnter: true,
                addOnSpace: false,
                addOnComma: false,
                addOnBlur: true,
                addOnParse: true,
                replaceSpacesWithDashes: false,
                pasteSplitPattern: ";"
            });

            // Disable Debug Data in DOM ("significant performance boost").
            // Comment the following line while debugging or execute this in browser console: angular.reloadWithDebugInfo();
            $compileProvider.debugInfoEnabled(false);
        }])
    .run(['$location', '$rootScope', '$state', '$stateParams', 'platformWebApp.authService', 'platformWebApp.mainMenuService', 'platformWebApp.pushNotificationService', 'platformWebApp.dialogService', '$window', '$animate', '$templateCache', 'gridsterConfig', 'taOptions', '$timeout', '$templateRequest', '$compile', 'platformWebApp.toolbarService', 'platformWebApp.loginOfBehalfUrlResolver',
        function ($location, $rootScope, $state, $stateParams, authService, mainMenuService, pushNotificationService, dialogService, $window, $animate, $templateCache, gridsterConfig, taOptions, $timeout, $templateRequest, $compile, toolbarService, loginOfBehalfUrlResolver) {

            //Disable animation
            $animate.enabled(false);

            $rootScope.$state = $state;
            $rootScope.$stateParams = $stateParams;
            $rootScope.$on('$stateChangeStart', function (event, toState) {
                if (toState.name === 'resetpasswordDialog') {
                    $rootScope.preventLoginDialog = true;
                } else if ($rootScope.preventLoginDialog && toState.name === 'loginDialog') {
                    event.preventDefault(); // Prevent state change
                }
            });

            var homeMenuItem = {
                path: 'home',
                title: 'platform.menu.home',
                icon: 'fa fa-home',
                action: function () { $state.go('workspace'); },
                // this item must always be at the top
                priority: 0,
                isAlwaysOnBar: true
            };
            mainMenuService.addMenuItem(homeMenuItem);

            var browseMenuItem = {
                path: 'browse',
                icon: 'fa fa-search',
                title: 'platform.menu.browse',
                priority: 90
            };
            mainMenuService.addMenuItem(browseMenuItem);

            var cfgMenuItem = {
                path: 'configuration',
                icon: 'fa fa-wrench',
                title: 'platform.menu.configuration',
                priority: 91
            };
            mainMenuService.addMenuItem(cfgMenuItem);

            var moreMenuItem = {
                path: 'more',
                title: 'platform.menu.more',
                headerTemplate: '$(Platform)/Scripts/app/navigation/menu/mainMenu-list-header.tpl.html',
                contentTemplate: '$(Platform)/Scripts/app/navigation/menu/mainMenu-list-content.tpl.html',
                // this item must always be at the bottom, so
                // don't use just 99 number: we have INFINITE list
                priority: Number.MAX_SAFE_INTEGER,
                isAlwaysOnBar: true
            };
            mainMenuService.addMenuItem(moreMenuItem);

            $rootScope.$on('unauthorized', function (event, rejection) {
                var url = $location.url();
                if (url.indexOf("resetpassword") !== -1) {
                    $state.go('resetPasswordDialog');
                } else if (!authService.isAuthenticated) {
                    $state.go('loginDialog');
                }
            });

            pushNotificationService.startListening();

            //server error handling
            //$rootScope.$on('httpError', function (event, rejection) {
            //    if (!(rejection.config.url.indexOf('api/platform/notification') + 1)) {
            //        pushNotificationService.error({ title: 'HTTP error', description: rejection.status + ' — ' + rejection.statusText, extendedData: rejection.data });
            //    }
            //});

            $rootScope.$on('loginStatusChanged', function (event, authContext) {
                //timeout need because $state not fully loading in run method and need to wait little time
                $timeout(function () {
                    var currentState = $state.current;
                    if (!authContext.isAuthenticated) {
                        $state.go('loginDialog');
                    } else if (authContext.passwordExpired) {
                        $state.go('changePasswordDialog', {
                            onClose: function () {
                                if (!currentState.abstract
                                    && currentState.name !== 'loginDialog'
                                    && currentState.name !== 'changePasswordDialog') {
                                    $state.go(currentState);
                                } else {
                                    $state.go('workspace');
                                }
                            }
                        });
                    } else if (!currentState.name || currentState.name === 'loginDialog') {
                        $state.go('workspace');
                    }
                }, 500);
            });

            authService.fillAuthData();

            // Modify multi-select (PT-799):
            // modify select-multiple.tpl.html to hide default placeholder
            var searchInputClass = "ng-class=\"{\'select2-display-none\': !$select.open}\"";
            $templateCache.put("select2/select-multiple.tpl.html", "<div class=\"ui-select-container ui-select-multiple select2 select2-container select2-container-multi\" ng-class=\"{\'select2-container-active select2-dropdown-open open\': $select.open, \'select2-container-disabled\': $select.disabled}\"><ul class=\"select2-choices\"><span class=\"ui-select-match\"></span><li class=\"select2-search-field\"><input type=\"search\" autocomplete=\"off\" autocorrect=\"off\" autocapitalize=\"off\" spellcheck=\"false\" role=\"combobox\" aria-expanded=\"true\" aria-owns=\"ui-select-choices-{{ $select.generatedId }}\" aria-label=\"{{ $select.baseTitle }}\" aria-activedescendant=\"ui-select-choices-row-{{ $select.generatedId }}-{{ $select.activeIndex }}\" class=\"select2-input ui-select-search\" " + searchInputClass + " placeholder=\"{{$selectMultiple.getPlaceholder()}}\" ng-disabled=\"$select.disabled\" ng-hide=\"$select.disabled\" ng-model=\"$select.search\" ng-click=\"$select.activate()\" style=\"width: 34px;\" ondrop=\"return false;\"></li></ul><div class=\"ui-select-dropdown select2-drop select2-with-searchbox select2-drop-active\" ng-class=\"{\'select2-display-none\': !$select.open || $select.items.length === 0}\"><div class=\"ui-select-choices\"></div></div></div>");
            // add "Add +" button
            var addButton = "<li ng-if='$select.items.length' class=\"ui-select-match-item select2-search-choice btn-default\" ng-click=\"$select.activate()\">Add</li>";
            var template = "<span class=\"ui-select-match\"><li class=\"ui-select-match-item select2-search-choice\" ng-repeat=\"$item in $select.selected track by $index\" ng-class=\"{\'select2-search-choice-focus\':$selectMultiple.activeMatchIndex === $index, \'select2-locked\':$select.isLocked(this, $index)}\" ui-select-sort=\"$select.selected\"><span uis-transclude-append=\"\"></span> <a href=\"javascript:;\" class=\"ui-select-match-close select2-search-choice-close\" ng-click=\"$selectMultiple.removeChoice($index)\" tabindex=\"-1\"></a></li>" + addButton + "</span>";
            $templateCache.put("select2/match-multiple.tpl.html", template);


            // cache application level templates
            $templateCache.put('pagerTemplate.html', '<div class="pagination"><pagination boundary-links="true" max-size="pageSettings.numPages" items-per-page="pageSettings.itemsPerPageCount" total-items="pageSettings.totalItems" ng-model="pageSettings.currentPage" class="pagination-sm" previous-text="&lsaquo;" next-text="&rsaquo;" first-text="&laquo;" last-text="&raquo;"></pagination></div>');

            gridsterConfig.columns = 4;
            gridsterConfig.colWidth = 130;
            gridsterConfig.defaultSizeX = 1;
            gridsterConfig.resizable = { enabled: false, handles: [] };
            gridsterConfig.maxRows = 8;
            gridsterConfig.mobileModeEnabled = false;
            gridsterConfig.outerMargin = false;

            String.prototype.hashCode = function () {
                var hash = 0, i, chr, len;
                if (this.length === 0) return hash;
                for (i = 0, len = this.length; i < len; i++) {
                    chr = this.charCodeAt(i);
                    hash = (hash << 5) - hash + chr;
                    hash |= 0; // Convert to 32bit integer
                }
                return hash;
            };

            String.prototype.capitalize = function () {
                return this.charAt(0).toUpperCase() + this.substr(1).toLowerCase();
            };

            if (!String.prototype.startsWith) {
                String.prototype.startsWith = function (searchString, position) {
                    if (searchString && searchString.toString() === '[object RegExp]') {
                        throw TypeError();
                    }
                    var length = this.length;
                    var startIndex = position ? Number(position) : 0;
                    if (isNaN(startIndex)) {
                        startIndex = 0;
                    }
                    var fromIndex = Math.min(Math.max(startIndex, 0), length);
                    if (fromIndex + searchString.length > length) {
                        return false;
                    }
                    return this.indexOf(searchString, startIndex) === fromIndex;
                };
            }

            if (!String.prototype.endsWith) {
                String.prototype.endsWith = function (searchString, position) {
                    if (searchString && searchString.toString() === '[object RegExp]') {
                        throw TypeError();
                    }
                    var length = this.length;
                    var endIndex = length;
                    if (position !== undefined) {
                        endIndex = position ? Number(position) : 0;
                        if (isNaN(endIndex)) {
                            endIndex = 0;
                        }
                    }
                    var toIndex = Math.min(Math.max(endIndex, 0), length);
                    var fromIndex = toIndex - searchString.length;
                    if (fromIndex < 0) {
                        return false;
                    }
                    return this.lastIndexOf(searchString, fromIndex) === fromIndex;
                };
            }

            if (!angular.isDefined(Number.MIN_SAFE_INTEGER)) {
                Number.MIN_SAFE_INTEGER = -9007199254740991;
            }
            if (!angular.isDefined(Number.MAX_SAFE_INTEGER)) {
                Number.MAX_SAFE_INTEGER = 9007199254740991;
            }

            // textAngular
            taOptions.toolbar = [
                ['bold', 'italics', 'underline', 'strikeThrough', 'ul', 'ol', 'redo', 'undo', 'clear', 'quote'],
                ['justifyLeft', 'justifyCenter', 'justifyRight', 'indent', 'outdent', 'html', 'insertImage', 'insertLink', 'insertVideo']];

            //register metaproperties templates
            $templateRequest('$(Platform)/Scripts/common/directives/genericValueInput.tpl.html').then(function (response) {
                var template = angular.element(response);
                $compile(template);
            });

            // register login-on-behalf command in platform account blade
            var loginOnBehalfCommand = {
                name: "platform.commands.login-on-behalf",
                icon: 'fas fa-key',
                executeMethod: function (blade) {
                    var showError = () => {
                        var dialog = {
                            id: "noUrlDialog",
                            title: 'platform.dialogs.impersonate-no-url.title',
                            message: 'platform.dialogs.impersonate-no-url.message'
                        };
                        dialogService.showErrorDialog(dialog);
                    };

                    var promise = loginOfBehalfUrlResolver.resolve(blade.currentEntity);
                    if (promise) {
                        blade.isLoading = true;
                        promise.then((url) => {
                            blade.isLoading = false;
                            if (url) {
                                if (url.endsWith("/")) {
                                    url = url.slice(0, -1);
                                }

                                url = url + '/account/impersonate/' + blade.currentEntity.id;
                                $window.open(url, '_blank');
                            } else {
                                showError();
                            }
                        });
                    } else {
                        showError();
                    }
                },
                canExecuteMethod: (blade) => blade.currentEntity,
                permission: 'platform:security:loginOnBehalf',
                index: 4
            };
            toolbarService.tryRegister(loginOnBehalfCommand, 'platformWebApp.accountDetailController');
        }]);

var AppDependencies = [
  'ui.router',
  'googlechart',
  'gridster',
  'ui.bootstrap',
  'ui.utils',
  'ui.sortable',
  'ui.select',
  'ngAnimate',
  'ngStorage',
  'ngResource',
  'fiestah.money',
  'ngCookies',
  'angularMoment',
  'angularFileUpload',
  'ngSanitize',
  'ng-context-menu',
  'ui.grid', 'ui.grid.autoResize', 'ui.grid.resizeColumns', 'ui.grid.moveColumns', 'ui.grid.saveState', 'ui.grid.selection', 'ui.grid.pagination', 'ui.grid.pinning', 'ui.grid.grouping',
  'ui.grid.draggable-rows',
  'ui.codemirror',
  'focusOn',
  'textAngular',
  'ngTagsInput',
  'tmh.dynamicLocale',
  'pascalprecht.translate',
  'angular.filter'
];

angular.module('platformWebApp', AppDependencies).
  controller('platformWebApp.appCtrl', ['$rootScope', '$scope', '$window', 'platformWebApp.mainMenuService', 'platformWebApp.pushNotificationService', '$translate', 'tmhDynamicLocale', 'amMoment', '$timeout', 'platformWebApp.modules', '$state', 'platformWebApp.bladeNavigationService', 'platformWebApp.userProfile', 'platformWebApp.settings', function ($rootScope, $scope, $window, mainMenuService, pushNotificationService, $translate, dynamicLocale, momentService, $timeout, modules, $state, bladeNavigationService, userProfile, settings) {
      pushNotificationService.run();

      $scope.closeError = function () {
          $scope.platformError = undefined;
      };
      modules.query().$promise.then(function (results) {
          var modulesWithErrors = _.filter(results, function (x) { return _.any(x.validationErrors); });
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


      $scope.$on('httpError', function (event, error) {
          if (bladeNavigationService.currentBlade) {
              bladeNavigationService.setError(error.status + ': ' + error.statusText, bladeNavigationService.currentBlade);
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
              userProfile.load().then(function () {
                  $translate.use(userProfile.language);
                  updateRtl(userProfile.language);
                  dynamicLocale.set(userProfile.regionalFormat);
                  momentService.changeLocale(userProfile.regionalFormat);
                  initializeMainMenu(userProfile);
              });
          };
      });

      $rootScope.$on('$translateChangeSuccess', function() {
          updateRtl($translate.use());
      });

      function updateRtl(currentLanguage) {
          var rtlLanguages = ['ar', 'arc', 'bcc', 'bqi', 'ckb', 'dv', 'fa', 'glk', 'he', 'lrc', 'mzn', 'pnb', 'ps', 'sd', 'ug', 'ur', 'yi'];
          $rootScope.isRTL = rtlLanguages.indexOf(currentLanguage) >= 0;
      }

      $scope.mainMenu = {};
      $scope.mainMenu.items = mainMenuService.menuItems;
      
      $scope.onMainMenuChanged = function (mainMenu) {
          if ($scope.isAuthenticated) {
              saveMainMenuState(mainMenu, userProfile);
          }
      }

      function initializeMainMenu(profile) {
          if (profile.mainMenuState) {
              $scope.mainMenu.isCollapsed = profile.mainMenuState.isCollapsed;
              angular.forEach(profile.mainMenuState.items, function(x) {
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
                          function(x) { return !x.isAlwaysOnBar; }),
                      function(x) { return { path: x.path, isCollapsed: x.isCollapsed, isFavorite: x.isFavorite, order: x.order }; })
              };
              profile.save();
          }
      }

      settings.getUiCustomizationSetting(function (uiCustomizationSetting) {
          if (uiCustomizationSetting.value) {
              $rootScope.uiCustomization = angular.fromJson(uiCustomizationSetting.value);
          }
      });

      // DO NOT CHANGE THE FUNCTION BELOW: COPYRIGHT VIOLATION
      $scope.initExpiration = function (x) {
          if (x && x.expirationDate) {
              x.hasExpired = new Date(x.expirationDate) < new Date();
          }
          return x;
      };

      $scope.showLicense = function () {
          $state.go('workspace.appLicense');
      };

  }])
// Specify SignalR server URL (application URL)
.factory('platformWebApp.signalRServerName', ['$location', function ($location) {
    var retVal = $location.url() ? $location.absUrl().slice(0, -$location.url().length - 1) : $location.absUrl();
    return retVal;
}])
.factory('platformWebApp.httpErrorInterceptor', ['$q', '$rootScope', function ($q, $rootScope) {
    var httpErrorInterceptor = {};

    httpErrorInterceptor.request = function (config) {
        // do something on success
        if (!config.cache) {
            $rootScope.$broadcast('httpRequestSuccess', config);
        }
        return config;
    };

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
.config(
  ['$stateProvider', '$httpProvider', 'uiSelectConfig', 'datepickerConfig', 'tmhDynamicLocaleProvider', '$translateProvider', '$compileProvider', function ($stateProvider, $httpProvider, uiSelectConfig, datepickerConfig, dynamicLocaleProvider, $translateProvider, $compileProvider) {
      $stateProvider.state('workspace', {
          url: '/workspace',
          templateUrl: '$(Platform)/Scripts/app/workspace.tpl.html'
      });

      //Add interceptor
      $httpProvider.interceptors.push('platformWebApp.httpErrorInterceptor');
      //ui-select set selectize as default theme
      uiSelectConfig.theme = 'select2';

      datepickerConfig.showWeeks = false;

      //Localization
      // https://github.com/lgalfaso/angular-dynamic-locale/#usage
      dynamicLocaleProvider.localeLocationPattern('$(Platform)/Scripts/i18n/angular-locale_{{locale}}.js');
      dynamicLocaleProvider.defaultLocale('en');
      dynamicLocaleProvider.useCookieStorage();
      // https://angular-translate.github.io/docs/#/guide
      $translateProvider.useUrlLoader('api/platform/localization')
        .useLoaderCache(true)
        .useSanitizeValueStrategy('escapeParameters')
        .preferredLanguage('en')
        .fallbackLanguage('en')
        .useLocalStorage();

      // Disable Debug Data in DOM ("significant performance boost").
      // Comment the following line while debugging or execute this in browser console: angular.reloadWithDebugInfo();
      $compileProvider.debugInfoEnabled(false);
  }])

.run(
  ['$rootScope', '$state', '$stateParams', 'platformWebApp.authService', 'platformWebApp.mainMenuService', 'platformWebApp.pushNotificationService', 'angularMomentConfig', "amMoment", '$animate', '$templateCache', 'gridsterConfig', 'taOptions', '$timeout',
    function ($rootScope, $state, $stateParams, authService, mainMenuService, pushNotificationService, momentConfig, momentService, $animate, $templateCache, gridsterConfig, taOptions, $timeout) {
        //Disable animation
        $animate.enabled(false);

        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

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

        // Localization
        // https://github.com/urish/angular-moment#usage
        //
        //  try to uncomment this code, if moment will not update locale correctly
        // https://github.com/urish/angular-moment/issues/212
        //momentConfig.preprocess = function (value) {
        //    return moment(value).locale(moment.locale());
        //}
        // set default locale to moment
        momentService.changeLocale("en");

        $rootScope.$on('unauthorized', function (event, rejection) {
            if (!authService.isAuthenticated) {
                $state.go('loginDialog');
            }
        });



        //server error handling
        //$rootScope.$on('httpError', function (event, rejection) {
        //    if (!(rejection.config.url.indexOf('api/platform/notification') + 1)) {
        //        pushNotificationService.error({ title: 'HTTP error', description: rejection.status + ' — ' + rejection.statusText, extendedData: rejection.data });
        //    }
        //});



        $rootScope.$on('loginStatusChanged', function (event, authContext) {
            //timeout need because $state not fully loading in run method and need to wait little time
            $timeout(function () {
                if (authContext.isAuthenticated) {
                    if (!$state.current.name || $state.current.name == 'loginDialog') {
                        $state.go('workspace');
                    }
                }
                else {
                    $state.go('loginDialog');
                }
            }, 500);

        });

        authService.fillAuthData();

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
            if (this.length == 0) return hash;
            for (i = 0, len = this.length; i < len; i++) {
                chr = this.charCodeAt(i);
                hash = ((hash << 5) - hash) + chr;
                hash |= 0; // Convert to 32bit integer
            }
            return hash;
        };

        if (!String.prototype.startsWith) {
            String.prototype.startsWith = function (searchString, position) {
                if (searchString && searchString.toString() == '[object RegExp]') {
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
                return this.indexOf(searchString, startIndex) == fromIndex;
            }
        }

        if (!String.prototype.endsWith) {
            String.prototype.endsWith = function (searchString, position) {
                if (searchString && searchString.toString() == '[object RegExp]') {
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
                return this.lastIndexOf(searchString, fromIndex) == fromIndex;
            }
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

    }]);

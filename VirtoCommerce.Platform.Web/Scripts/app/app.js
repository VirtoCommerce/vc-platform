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
  'pascalprecht.translate'
];

angular.module('platformWebApp', AppDependencies).
  controller('platformWebApp.appCtrl', ['$scope', '$window', 'platformWebApp.pushNotificationService', '$translate', '$timeout', function ($scope, $window, pushNotificationService, $translate, $timeout) {
      pushNotificationService.run();

      $timeout(function () {
          var currentLanguage = $translate.use();
          var rtlLanguages = ['ar', 'arc', 'bcc', 'bqi', 'ckb', 'dv', 'fa', 'glk', 'he', 'lrc', 'mzn', 'pnb', 'ps', 'sd', 'ug', 'ur', 'yi'];
          $scope.isRTL = rtlLanguages.indexOf(currentLanguage) >= 0;
      }, 100);
  }])
// Specify SignalR server URL (application URL)
.factory('platformWebApp.signalRServerName', ['$location', function ($location) {
    var retVal = $location.url() ? $location.absUrl().slice(0, -$location.url().length - 1) : $location.absUrl();
    return retVal;
}])
.factory('platformWebApp.httpErrorInterceptor', ['$q', '$rootScope', function ($q, $rootScope) {
    var httpErrorInterceptor = {};

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
  ['$stateProvider', '$httpProvider', 'uiSelectConfig', 'datepickerConfig', '$translateProvider', function ($stateProvider, $httpProvider, uiSelectConfig, datepickerConfig, $translateProvider) {
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
      // https://angular-translate.github.io/docs/#/guide
      $translateProvider.useUrlLoader('api/platform/localization')
        .useLoaderCache(true)
        .useSanitizeValueStrategy('escapeParameters')
        .preferredLanguage('en')
        .fallbackLanguage('en')
        .useLocalStorage();
  }])

.run(
  ['$rootScope', '$state', '$stateParams', 'platformWebApp.authService', 'platformWebApp.mainMenuService', 'platformWebApp.pushNotificationService', '$animate', '$templateCache', 'gridsterConfig', 'taOptions',
    function ($rootScope, $state, $stateParams, authService, mainMenuService, pushNotificationService, $animate, $templateCache, gridsterConfig, taOptions) {
        //Disable animation
        $animate.enabled(false);

        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

        var homeMenuItem = {
            path: 'home',
            title: 'platform.menu.home',
            icon: 'fa fa-home',
            action: function () { $state.go('workspace'); },
            priority: 0
        };
        mainMenuService.addMenuItem(homeMenuItem);

        var browseMenuItem = {
            path: 'browse',
            icon: 'fa fa-search',
            title: 'platform.menu.browse',
            priority: 90,
        };
        mainMenuService.addMenuItem(browseMenuItem);

        var cfgMenuItem = {
            path: 'configuration',
            icon: 'fa fa-wrench',
            title: 'platform.menu.configuration',
            priority: 91,
        };
        mainMenuService.addMenuItem(cfgMenuItem);

        $rootScope.$on('unauthorized', function (event, rejection) {
            if (!authService.isAuthenticated) {
                $state.go('loginDialog');
            }
        });

        //server error  handling
        //$rootScope.$on('httpError', function (event, rejection) {
        //    if (!(rejection.config.url.indexOf('api/platform/notification') + 1)) {
        //        pushNotificationService.error({ title: 'HTTP error', description: rejection.status + ' — ' + rejection.statusText, extendedData: rejection.data });
        //    }
        //});

        $rootScope.$on('loginStatusChanged', function (event, authContext) {
            if (authContext.isAuthenticated) {
                console.log('State - ' + $state.current.name);
                if (!$state.current.name || $state.current.name == 'loginDialog') {
                    homeMenuItem.action();
                }
            }
            else {
                $state.go('loginDialog');
            }
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

        String.prototype.endsWith = function (suffix) {
            return this.indexOf(suffix, this.length - suffix.length) !== -1;
        };

        // textAngular
        taOptions.toolbar = [
        ['bold', 'italics', 'underline', 'strikeThrough', 'ul', 'ol', 'redo', 'undo', 'clear', 'quote'],
        ['justifyLeft', 'justifyCenter', 'justifyRight', 'indent', 'outdent', 'html', 'insertImage', 'insertLink', 'insertVideo']];

    }]);

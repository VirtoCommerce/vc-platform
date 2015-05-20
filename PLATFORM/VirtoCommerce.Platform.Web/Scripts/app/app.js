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
  'xeditable',
  'fiestah.money',
  'ngCookies',
  'angularMoment',
  'angularFileUpload',
  'ngSanitize',
  'ng-context-menu',
  //'ui.grid',
  //'ui.grid.selection',
  'ui.codemirror',
  'ngTagsInput'
];

angular.module('platformWebApp', AppDependencies).
  controller('platformWebApp.appCtrl', ['$scope', '$window', '$state', 'platformWebApp.notificationService', function ($scope, $window, $state, notificationService) {
      $scope.platformVersion = $window.platformVersion;
      notificationService.run();

      $scope.curentStateName = function () {
          return $state.current.name;
      };
  }])
// Specify SignalR server URL (application URL)
.factory('platformWebApp.signalRServerName', ['$location', function apiTokenFactory($location) {
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
.config(
  ['$stateProvider', '$httpProvider', 'uiSelectConfig', function ($stateProvider, $httpProvider, uiSelectConfig) {
      $stateProvider.state('workspace', {
          templateUrl: 'Scripts/app/workspace.tpl.html'
      });

      //Add interseptor
      $httpProvider.interceptors.push('platformWebApp.httpErrorInterceptor');
      //ui-select set selectize as default theme
      uiSelectConfig.theme = 'select2';

  }
  ]
)
.run(
  ['$rootScope', '$state', '$stateParams', 'platformWebApp.authService', 'platformWebApp.mainMenuService', 'editableOptions', 'platformWebApp.notificationService', '$animate', '$templateCache', 'gridsterConfig', 'platformWebApp.widgetService',
    function ($rootScope, $state, $stateParams, authService, mainMenuService, editableOptions, notificationService, $animate, $templateCache, gridsterConfig, widgetService) {
        //Disable animation
        $animate.enabled(false);
        editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'

        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;
        var homeMenuItem = {
            path: 'home',
            title: 'Home',
            icon: 'fa fa-home',
            action: function () { $state.go('workspace') },
            priority: 0
        };
        mainMenuService.addMenuItem(homeMenuItem);

        var menuItem = {
            path: 'browse',
            icon: 'fa fa-search',
            title: 'Browse',
            priority: 90,
        };
        mainMenuService.addMenuItem(menuItem);

        var journeyMenuItem = {
            path: 'active',
            title: 'Active',
            icon: 'fa fa-tasks',
            priority: 999
        };
        mainMenuService.addMenuItem(journeyMenuItem);


        $rootScope.$on('unauthorized', function (event, rejection) {
            $state.go('loginDialog');
        });

        $rootScope.$on('httpError', function (event, rejection) {
            if (!(rejection.config.url.indexOf('api/notification') + 1)) {
                notificationService.error({ title: 'HTTP error', description: rejection.status + ' — ' + rejection.statusText, extendedData: rejection.data });
            }
        });

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

        // register dashboard demo widgets and templates
        widgetService.registerWidget({
            controller: 'platformWebApp.demo.dashboard.graphWidgetController',
            size: [4, 4],
            template: 'graph.html'
        }, 'mainDashboard');
        widgetService.registerWidget({
            controller: 'platformWebApp.demo.dashboard.catalogsWidgetController',
            template: 'tile-count.html'
        }, 'mainDashboard');
        widgetService.registerWidget({
            controller: 'platformWebApp.demo.dashboard.productsWidgetController',
            template: 'tile-count.html'
        }, 'mainDashboard');
        widgetService.registerWidget({
            controller: 'platformWebApp.demo.dashboard.notificationsWidgetController',
            template: 'tile-notifications.html'
        }, 'mainDashboard');

        $templateCache.put('graph.html', '<div google-chart chart="chartObject" style="height:100%"></div>');
        $templateCache.put('tile-count.html', '<div class="gridster-cnt __info"><div class="cnt-inner"><div class="list-count">{{data.count}}</div><div class="list-t">{{data.descr}}</div></div></div>');
        $templateCache.put('tile-notifications.html', '<ul class="list __notice">\
            <li class="list-item __info">\
                <a class="list-link" href="" ng-click="notification(\'info\')">\
                    <i class="list-ico fa fa-comments"></i>\
                Info\
            </a>\
        </li>\
        <li class="list-item __warning">\
            <a class="list-link" href="" ng-click="notification(\'warning\')">\
                <i class="list-ico fa fa-warning"></i>\
                Warning\
            </a>\
        </li>\
        <li class="list-item __task">\
            <a class="list-link" href="" ng-click="notification(\'task\')">\
                <i class="list-ico fa fa-tasks"></i>\
                Task\
            </a>\
        </li>\
        <li class="list-item __error">\
            <a class="list-link" href="" ng-click="notification(\'error\')">\
                <i class="list-ico fa fa-warning"></i>\
                Error\
            </a>\
        </li>\
    </ul>');
    }
  ]);

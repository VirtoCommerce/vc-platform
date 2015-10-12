angular.module('platformWebApp')
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('pushNotificationsHistory', {
              url: '/events',
              templateUrl: '$(Platform)/Scripts/app/pushNotifications/notification.tpl.html',
              controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                  var blade = {
                      id: 'events',
                      title: 'System events',
                      breadcrumbs: [],
                      subtitle: 'Events history',
                      controller: 'platformWebApp.pushNotificationsHistoryController',
                      template: '$(Platform)/Scripts/app/pushNotifications/blade/history.tpl.html',
                      isClosingDisabled: true
                  };
                  bladeNavigationService.showBlade(blade);
              }
              ]
          });
  }])
.factory('platformWebApp.pushNotificationTemplateResolver', ['platformWebApp.bladeNavigationService', '$state', function (bladeNavigationService, $state) {
    var notificationTemplates = [];

    var defaultTypes = ['error', 'info', 'warning'];
    function register(template) {
        notificationTemplates.push(template);
        notificationTemplates.sort(function (a, b) { return a.priority - b.priority; })
    };
    function resolve(notification, place) {
        return _.find(notificationTemplates, function (x) { return x.satisfy(notification, place); })
    };
    var retVal = {
        register: register,
        resolve: resolve,
    };

    //Recent events notification template (error, info, debug) 
    var menuDefaultTemplate =
		{
		    priority: 1000,
		    satisfy: function (notification, place) { return place == 'menu'; },
		    //template for display that notification in menu and list
		    template: '$(Platform)/Scripts/app/pushNotifications/menuDefault.tpl.html',
		    //action executed when notification selected
		    action: function (notify) { $state.go('pushNotificationsHistory', notify) }
		};

    //In history list notification template (error, info, debug)
    var historyDefaultTemplate =
		{
		    priority: 1000,
		    satisfy: function (notification, place) { return place == 'history'; },
		    //template for display that notification in menu and list
		    template: '$(Platform)/Scripts/app/pushNotifications/blade/historyDefault.tpl.html',
		    //action executed in event detail
		    action: function (notify) {
		        var blade = {
		            id: 'notifyDetail',
		            title: 'Event detail',
		            subtitle: 'Event detail',
		            template: '$(Platform)/Scripts/app/pushNotifications/blade/historyDetailDefault.tpl.html',
		            isClosingDisabled: false,
		            notify: notify
		        };
		        bladeNavigationService.showBlade(blade);
		    }
		};

    retVal.register(menuDefaultTemplate);
    retVal.register(historyDefaultTemplate);

    return retVal;
}])
.factory('platformWebApp.pushNotificationService', ['$rootScope', 'platformWebApp.signalRHubProxy', '$timeout', '$interval', '$state', 'platformWebApp.mainMenuService', 'platformWebApp.pushNotificationTemplateResolver', 'platformWebApp.pushNotifications', 'platformWebApp.signalRServerName',
    function ($rootScope, signalRHubProxy, $timeout, $interval, $state, mainMenuService, eventTemplateResolver, notifications, signalRServerName) {

        var clientPushHubProxy = signalRHubProxy(signalRServerName, 'clientPushHub', { logging: true });
        clientPushHubProxy.on('notification', function (data) {
            var notifyMenu = mainMenuService.findByPath('pushNotifications');
            var notificationTemplate = eventTemplateResolver.resolve(data, 'menu');
            //broadcast event
            $rootScope.$broadcast("new-notification-event", data);

            var menuItem = {
                parent: notifyMenu,
                path: 'pushNotifications/notifications',
                icon: 'fa fa-bell-o',
                title: data.title,
                priority: 2,
                permission: '',
                children: [],
                action: notificationTemplate.action,
                template: notificationTemplate.template,
                notify: data
            };

            var alreadyExitstItem = _.find(notifyMenu.children, function (x) { return x.notify.id == menuItem.notify.id; });
            if (alreadyExitstItem) {
                angular.copy(menuItem, alreadyExitstItem);
            }
            else {
                notifyMenu.children.push(menuItem);
                notifyMenu.newCount++;

                if (angular.isDefined(notifyMenu.intervalPromise)) {
                    $interval.cancel(notifyMenu.intervalPromise);
                }
                animateNotify();
                notifyMenu.intervalPromise = $interval(animateNotify, 30000);
            }
        });

        function animateNotify() {
            var notifyMenu = mainMenuService.findByPath('pushNotifications');
            notifyMenu.incremented = true;

            $timeout(function () {
                notifyMenu.incremented = false;
            }, 1500);
        }

        function markAllAsRead() {
            var notifyMenu = mainMenuService.findByPath('pushNotifications');
            if (angular.isDefined(notifyMenu.intervalPromise)) {
                $interval.cancel(notifyMenu.intervalPromise);
            }

            notifications.markAllAsRead(null, function (data, status, headers, config) {
                notifyMenu.incremented = false;
                notifyMenu.newCount = 0;
            }, function (error) {
                //bladeNavigationService.setError('Error ' + error.status, blade);
            });
        }

        var retVal = {
            run: function () {
                if (!this.running) {
                    var notifyMenu = mainMenuService.findByPath('pushNotifications');
                    if (!angular.isDefined(notifyMenu)) {
                        notifyMenu = {
                            path: 'pushNotifications',
                            icon: 'fa fa-bell-o',
                            title: 'Notifications',
                            priority: 2,
                            permission: '',
                            headerTemplate: '$(Platform)/Scripts/app/pushNotifications/menuHeader.tpl.html',
                            listTemplate: '$(Platform)/Scripts/app/pushNotifications/menuList.tpl.html',
                            template: '$(Platform)/Scripts/app/pushNotifications/menu.tpl.html',
                            action: function () { markAllAsRead(); if (this.children.length == 0) { this.showHistory(); } },
                            showHistory: function () { $state.go('pushNotificationsHistory'); },
                            clearRecent: function () { notifyMenu.children.splice(0, notifyMenu.children.length); },
                            children: [],
                            newCount: 0
                        };
                        mainMenuService.addMenuItem(notifyMenu);
                    }
                    this.running = true;
                };
            },
            running: false
        };
        return retVal;

    }]);
angular.module('platformWebApp')
    .controller('platformWebApp.pushNotificationsHistoryController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.pushNotificationTemplateResolver', 'platformWebApp.pushNotifications',
        function ($scope, bladeNavigationService, eventTemplateResolver, notifications) {
            var blade = $scope.blade;

            $scope.pageSettings = {};
            $scope.pageSettings.totalItems = 0;
            $scope.pageSettings.currentPage = 1;
            $scope.pageSettings.numPages = 5;
            $scope.pageSettings.itemsPerPageCount = 10;
            $scope.notifications = [];

            $scope.columns = [
                { title: "platform.blades.history.labels.type", orderBy: "NotifyType" },
                { title: "platform.blades.history.labels.title", orderBy: "Title" },
                { title: "platform.blades.history.labels.created", orderBy: "Created", checked: true, reverse: true }

            ];

            blade.refresh = function () {
                blade.isLoading = true;
                var start = $scope.pageSettings.currentPage * $scope.pageSettings.itemsPerPageCount - $scope.pageSettings.itemsPerPageCount;
                notifications.query({ skip: start, take: $scope.pageSettings.itemsPerPageCount, sort: getOrderByExpression() }, function (data, status, headers, config) {
                    angular.forEach(data.notifyEvents, function (x) {
                        const notificationTemplate = eventTemplateResolver.resolve(x, 'history');
                        x.template = notificationTemplate.template;
                        x.action = notificationTemplate.action;
                    });
                    $scope.notifications = data.notifyEvents;
                    $scope.pageSettings.totalItems = data.totalCount;
                    blade.isLoading = false;
                }, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, blade);
                });
            };

            function getOrderByExpression() {
                var retVal = '';
                var column = _.find($scope.columns, function (x) { return x.checked; });
                if (angular.isDefined(column)) {
                    retVal = column.orderBy;
                    if (column.reverse) {
                        retVal += ":desc";
                    }
                }

                return retVal;
            }

            $scope.setOrder = function (column) {
                //reset prev selection may be commented if need support multiple order clauses
                _.each($scope.columns, function (x) { x.checked = false });
                column.checked = true;
                column.reverse = !column.reverse;
                $scope.pageSettings.currentPage = 1;

                blade.refresh();
            }

            blade.toolbarCommands = [
                {
                    name: "platform.commands.refresh",
                    icon: 'fa fa-refresh',
                    executeMethod: blade.refresh,
                    canExecuteMethod: function () {
                        return true;
                    }
                }];

            $scope.$watch('pageSettings.currentPage', blade.refresh);

            // actions on load
            //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
            //blade.refresh();
        }]);

angular.module('platformWebApp')
    .controller('platformWebApp.accountSessionsWidgetController',
        ['$scope', '$timeout', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', function ($scope, $timeout, bladeNavigationService, accounts) {
            var blade = $scope.widget.blade;

            var userId = null;

            if (blade.data) {
                userId = blade.data.id
            }
            else if (blade.currentEntity) {
                var account = _.first(blade.currentEntity.securityAccounts)
                if (account) {
                    userId = account.id;
                }
            }

            function refresh() {
                blade.sessionsCount = 0;

                if (!userId) {
                    return;
                }

                accounts.searchSessions(
                    { userId: userId },
                    { take: 0 },
                    function (data) {
                        blade.sessionsCount = data.totalCount;
                    });
            }

            $scope.openBlade = function () {
                if (!userId) {
                    return;
                }

                var newBlade = {
                    id: "sessionsBlade",
                    userId: userId,
                    refreshSessionsCountCallback: function (newCount) {
                        blade.sessionsCount = newCount;
                    },
                    controller: 'platformWebApp.sessionsListController',
                    template: '$(Platform)/Scripts/app/security/blades/sessions-list.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            };

            $scope.$on('accountPasswordChanged', function () {
                $timeout(function () {
                    refresh();
                });
            });

            refresh();
        }]);

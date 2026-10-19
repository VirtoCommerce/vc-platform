angular.module('platformWebApp')
    .controller('platformWebApp.accountSignInLogWidgetController',
        ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts',
            function ($scope, bladeNavigationService, accounts) {
                var blade = $scope.widget.blade;

                var userId = null;

                if (blade.data) {
                    userId = blade.data.id;
                }
                else if (blade.currentEntity) {
                    var account = _.first(blade.currentEntity.securityAccounts);
                    if (account) {
                        userId = account.id;
                    }
                }

                function refresh() {
                    blade.signInLogCount = 0;

                    if (!userId) {
                        return;
                    }

                    accounts.searchSignInLog(
                        {},
                        { userId: userId, take: 0 },
                        function (data) {
                            blade.signInLogCount = data.totalCount;
                        });
                }

                $scope.openBlade = function () {
                    if (!userId) {
                        return;
                    }

                    // Same dashboard as the Security workspace, scoped to this account: the
                    // statistics answer "what has been happening to this user" before the
                    // record list answers "exactly which attempts".
                    var newBlade = {
                        id: "signInLogBlade",
                        userId: userId,
                        title: 'platform.blades.sign-in-log-dashboard.title',
                        subtitle: blade.currentEntity.userName || blade.title,
                        controller: 'platformWebApp.signInLogDashboardController',
                        template: '$(Platform)/Scripts/app/security/blades/sign-in-log-dashboard.html'
                    };
                    bladeNavigationService.showBlade(newBlade, $scope.blade);
                };

                refresh();
            }]);

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

                    var newBlade = {
                        id: "signInLogBlade",
                        userId: userId,
                        // The widget counts every attempt for this user, so the list must not
                        // silently narrow to the last 24 hours.
                        initialFilter: { period: '' },
                        refreshCountCallback: function (newCount) {
                            blade.signInLogCount = newCount;
                        },
                        controller: 'platformWebApp.signInLogListController',
                        template: '$(Platform)/Scripts/app/security/blades/sign-in-log-list.html'
                    };
                    bladeNavigationService.showBlade(newBlade, $scope.blade);
                };

                refresh();
            }]);

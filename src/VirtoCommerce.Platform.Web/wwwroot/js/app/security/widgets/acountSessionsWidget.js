angular.module('platformWebApp')
    .controller('platformWebApp.accountSessionsWidgetController',
        ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', function ($scope, bladeNavigationService, accounts) {
            var blade = $scope.widget.blade;

            var searchCriteria = {
                skip: 0,
                take: 0
            };

            if (blade.data) {
                searchCriteria.userId = blade.data.id
            }
            else if (blade.currentEntity) {
                var account = _.first(blade.currentEntity.securityAccounts)
                if (account) {
                    searchCriteria.userId = account.id;
                }
            }

            function refresh() {
                blade.sessionsCount = 0;

                if (!searchCriteria.userId) {
                    return;
                }

                accounts.sessions(searchCriteria, function (data) {
                    blade.sessionsCount = data.totalCount;
                });
            }

            $scope.openBlade = function () {
                if (!searchCriteria.userId) {
                    return;
                }

                var newBlade = {
                    id: "sessionsBlade",
                    userId: searchCriteria.userId,
                    controller: 'platformWebApp.sessionsListController',
                    template: '$(Platform)/Scripts/app/security/blades/sessions-list.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            };

            refresh();
        }]);

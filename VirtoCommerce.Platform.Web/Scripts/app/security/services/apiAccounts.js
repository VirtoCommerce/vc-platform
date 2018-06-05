angular.module('platformWebApp')
    .factory('platformWebApp.apiAccounts', ['platformWebApp.accounts', '$state', function (accounts, $state) {

        var retVal = {};

        retVal.run = function(userName) {
            chackUser("frontend");
            chackUser(userName);
        };

        function chackUser(name) {
            accounts.get({ id: name }).$promise.then(function (data) {
                angular.forEach(data.apiAccounts, function (value) {
                    if (value.forceSecretKeyChange) {
                        $state.go('changeApiSecretKey', { apiAccount: value, userName: data.userName });
                    }
                });
            });
        }

        return retVal;
    }]);

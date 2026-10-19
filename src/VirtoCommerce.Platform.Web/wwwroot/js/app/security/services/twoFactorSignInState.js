angular.module('platformWebApp')
    .factory('platformWebApp.twoFactorSignInState', ['$state', ($state) => {
        var isTwoFactorSignInState = (state) => !!(state && state.data && state.data.twoFactorSignIn);
        var get = () => _.find($state.get(), isTwoFactorSignInState);

        return {
            get: get,
            is: isTwoFactorSignInState,
            go: (params) => $state.go(get().name, params)
        };
    }]);

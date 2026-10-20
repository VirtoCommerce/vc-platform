angular.module('platformWebApp')
    .factory('platformWebApp.twoFactorSignInState', ['$state', ($state) => {
        var isTwoFactorSignInState = (state) => !!(state && state.data && state.data.twoFactorSignIn);
        var get = () => _.find($state.get(), isTwoFactorSignInState);

        return {
            exists: () => !!get(),
            getName: () => {
                var state = get();
                return state ? state.name : undefined;
            },
            go: (params) => $state.go(get().name, params)
        };
    }]);

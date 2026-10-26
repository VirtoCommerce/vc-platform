angular.module('platformWebApp')
    .factory('platformWebApp.signInLogTypes', [() => {
        var types = [
            { label: 'platform.blades.sign-in-log.filter.type-password', value: 'Password' },
            { label: 'platform.blades.sign-in-log.filter.type-external', value: 'External' },
            { label: 'platform.blades.sign-in-log.filter.type-impersonation', value: 'Impersonation' },
            { label: 'platform.blades.sign-in-log.filter.type-impersonation-revert', value: 'ImpersonationRevert' },
            { label: 'platform.blades.sign-in-log.filter.type-logout', value: 'Logout' }
        ];

        return {
            getAll: () => types.slice(),
            register: (type) => {
                if (!_.some(types, (x) => x.value === type.value)) {
                    types.push(type);
                }
            }
        };
    }]);

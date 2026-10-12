angular.module('platformWebApp')
    .factory('platformWebApp.signInLogOutcome', [() => {
        var requiresTwoFactor = 'RequiresTwoFactor';

        return {
            get: (record) => {
                if (record.succeeded) {
                    return { cssClass: '__success', label: 'platform.blades.sign-in-log.labels.succeeded', isFailure: false };
                }

                if (record.failureReason === requiresTwoFactor) {
                    return { cssClass: '__pending', label: 'platform.blades.sign-in-log.labels.awaiting-second-factor', isFailure: false };
                }

                return { cssClass: '__failed', label: 'platform.blades.sign-in-log.labels.failed', isFailure: true };
            }
        };
    }]);

angular.module('platformWebApp')
    .factory('platformWebApp.signInLogOutcome', [() => {
        var requiresTwoFactor = 'RequiresTwoFactor';

        var succeeded = Object.freeze({ cssClass: '__success', label: 'platform.blades.sign-in-log.labels.succeeded', isFailure: false });
        var awaitingSecondFactor = Object.freeze({ cssClass: '__pending', label: 'platform.blades.sign-in-log.labels.awaiting-second-factor', isFailure: false });
        var failed = Object.freeze({ cssClass: '__failed', label: 'platform.blades.sign-in-log.labels.failed', isFailure: true });

        return {
            get: (record) => {
                if (record.succeeded) {
                    return succeeded;
                }

                return record.failureReason === requiresTwoFactor ? awaitingSecondFactor : failed;
            }
        };
    }]);

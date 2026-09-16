angular.module('platformWebApp')
    .controller('platformWebApp.signInLogDetailController',
        ['$scope', function ($scope) {
            var blade = $scope.blade;
            var record = blade.record || {};

            blade.title = 'platform.blades.sign-in-log.detail.title';
            blade.subtitle = record.userName;
            blade.headIcon = 'fas fa-clipboard-list';
            blade.isLoading = false;

            $scope.record = record;

            $scope.isImpersonation = record.signInType === 'Impersonation' ||
                                     record.signInType === 'ImpersonationRevert';

            // An audit record is read-only by definition: rows are written once and never edited,
            // so this blade has no save, no toolbar and no editable field.
            $scope.sections = [
                {
                    title: 'platform.blades.sign-in-log.detail.section-what',
                    fields: [
                        { label: 'platform.blades.sign-in-log.labels.date', value: record.createdDate, isDate: true },
                        { label: 'platform.blades.sign-in-log.labels.outcome', value: null, isOutcome: true },
                        { label: 'platform.blades.sign-in-log.labels.failure-reason', value: record.failureReason },
                        { label: 'platform.blades.sign-in-log.labels.sign-in-type', value: record.signInType },
                        { label: 'platform.blades.sign-in-log.filter.type-external', value: record.provider }
                    ]
                },
                {
                    title: 'platform.blades.sign-in-log.detail.section-who',
                    fields: [
                        { label: 'platform.blades.sign-in-log.labels.user-name', value: record.userName },
                        { label: 'platform.blades.sign-in-log.detail.user-id', value: record.userId },
                        { label: 'platform.blades.sign-in-log.detail.operator-name', value: record.operatorUserName },
                        { label: 'platform.blades.sign-in-log.detail.operator-id', value: record.operatorUserId },
                        { label: 'platform.blades.sign-in-log.detail.member-id', value: record.memberId }
                    ]
                },
                {
                    title: 'platform.blades.sign-in-log.detail.section-where',
                    fields: [
                        { label: 'platform.blades.sign-in-log.labels.ip-address', value: record.ipAddress },
                        { label: 'platform.blades.sign-in-log.detail.user-agent', value: record.userAgent },
                        { label: 'platform.blades.sign-in-log.detail.client-id', value: record.clientId }
                    ]
                },
                {
                    title: 'platform.blades.sign-in-log.detail.section-context',
                    fields: [
                        { label: 'platform.blades.sign-in-log.filter.store', value: record.storeId },
                        { label: 'platform.blades.sign-in-log.detail.store-name', value: record.storeName },
                        { label: 'platform.blades.sign-in-log.labels.organization', value: record.organizationName },
                        { label: 'platform.blades.sign-in-log.detail.organization-id', value: record.organizationId },
                        { label: 'platform.blades.sign-in-log.detail.session-id', value: record.sessionId }
                    ]
                }
            ];
        }]);

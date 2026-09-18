angular.module('platformWebApp')
    .controller('platformWebApp.signInLogDetailController',
        ['$scope', 'platformWebApp.metaFormsService', function ($scope, metaFormsService) {
            var blade = $scope.blade;
            var record = blade.record || {};

            blade.title = 'platform.blades.sign-in-log.detail.title';
            blade.subtitle = record.userName;
            blade.headIcon = 'fas fa-clipboard-list';
            blade.isLoading = false;

            blade.currentEntity = record;

            $scope.isImpersonation = record.signInType === 'Impersonation' ||
                                     record.signInType === 'ImpersonationRevert';

            // va-generic-value-input reads its value out of the field's own values array, so each
            // registered field is copied and filled rather than mutated - the registry is shared.
            blade.metaFields = _.chain(metaFormsService.getMetaFields('signInLogDetails') || [])
                .map(function (field) {
                    var value = record[field.name];

                    if (value === null || value === undefined || value === '') {
                        return null;
                    }

                    return angular.extend({}, field, {
                        // Audit rows are written once. Nothing here is editable, and there is no save.
                        isReadOnly: true,
                        values: [{ value: value }]
                    });
                })
                .compact()
                .value();
        }]);

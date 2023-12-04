angular.module('platformWebApp')
    .factory('platformWebApp.localizableSettingsApi', ['$resource', function ($resource) {
        return $resource('api/platform/localizable-settings/:name/dictionary-items', { name: '@name' }, {
            getSettingsAndLanguages: { method: 'GET', url: 'api/platform/localizable-settings' },
            saveItems: { method: 'POST' },
            deleteItems: { method: 'DELETE' },
        });
    }]);

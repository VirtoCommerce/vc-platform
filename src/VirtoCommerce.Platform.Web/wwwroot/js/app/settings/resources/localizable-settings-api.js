angular.module('platformWebApp')
    .factory('platformWebApp.localizableSettingsApi', [
        '$resource', 'platformWebApp.i18n',
        function ($resource, i18n) {
            return $resource('api/platform/localizable-settings/:name/dictionary-items', { name: '@name' }, {
                getItemsAndLanguages: { method: 'GET' },
                saveItems: { method: 'POST' },
                deleteItems: { method: 'DELETE' },
                getLocalizableSettingNames: { method: 'GET', url: 'api/platform/localizable-settings/names', isArray: true },
                getValues: {
                    method: 'GET',
                    url: 'api/platform/localizable-settings/:name/dictionary-items/:language/values',
                    params: { language: i18n.getLanguage },
                    isArray: true
                }
            });
        }]);

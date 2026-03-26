angular.module('platformWebApp')
    .factory('platformWebApp.settingsV2', ['$resource', function ($resource) {
        return $resource(null, null, {
            getGlobalSchema: { url: 'api/platform/settings/v2/global/schema', method: 'GET', isArray: true },
            getGlobalValues: { url: 'api/platform/settings/v2/global/values', method: 'GET' },
            saveGlobalValues: { url: 'api/platform/settings/v2/global/values', method: 'POST' },
            getTenantSchema: { url: 'api/platform/settings/v2/tenant/:tenantType/schema', method: 'GET', isArray: true },
            getTenantValues: { url: 'api/platform/settings/v2/tenant/:tenantType/:tenantId/values', method: 'GET' },
            saveTenantValues: { url: 'api/platform/settings/v2/tenant/:tenantType/:tenantId/values', method: 'POST' }
        });
    }]);

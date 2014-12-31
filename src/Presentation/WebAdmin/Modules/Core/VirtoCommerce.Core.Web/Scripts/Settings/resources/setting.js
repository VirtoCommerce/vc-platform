angular.module('virtoCommerce.coreModule.settings.resources.setting', [])
.factory('settings', ['$resource', function ($resource) {
    return {
        getSettingsSections: getSettingsSectionsMock,
        getSettings: getSettingsMock,
        update: updateMock
    };
    //return $resource('api/settings/:id', { id: '@Id' }, {
    //    getSettingsSections: { method: 'GET', url: 'api/settings/settings', isArray: true }
    //});

    function getSettingsSectionsMock(param1, param2) {
        var retVal = [
            {
                id: 'id1',
                title: 'id1 title'
            },
            {
                id: 'id2',
                title: 'id2 title'
            }
        ];
        param2(retVal);
    }

    function getSettingsMock(param1, param2) {
        var retVal = [
            {
                name: 'VirtoCommerce.Core.Test.General.String',
                groupName: '-',
                valueType: 'string',
                defaultValue: 'qwerty',
                value: 'qwerty',
                title: 'String ' + param1.moduleId,
                description: 'A text setting'
            },
            {
                name: 'VirtoCommerce.Core.Test.General.Password',
                groupName: '-',
                valueType: 'secureString',
                defaultValue: 'qwerty',
                value: '123',
                title: 'Password ' + param1.moduleId,
                description: 'A secure text setting'
            },
            {
                name: 'VirtoCommerce.Core.Test.Advanced.Boolean1',
                groupName: 'Advanced',
                valueType: 'boolean',
                defaultValue: 'true',
                value: 'false',
                title: 'Boolean1 ' + param1.moduleId,
                description: 'A boolean setting'
            },
            {
                name: 'VirtoCommerce.Core.Test.Advanced.Boolean2',
                groupName: 'Advanced',
                valueType: 'boolean',
                defaultValue: 'true',
                value: 'false',
                title: 'Boolean2 ' + param1.moduleId,
                description: 'A boolean setting'
            }
        ];
        param2(retVal);
    }


    function updateMock(param0, param1, param2) {
        param2();
    }
}]);
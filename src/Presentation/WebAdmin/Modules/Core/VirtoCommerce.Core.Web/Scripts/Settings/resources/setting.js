angular.module('virtoCommerce.coreModule.settings.resources.setting', [])
.factory('settings', ['$resource', function ($resource) {
    //return $resource('api/settings/:id', { id: '@Id' }, {
    //    getSettingsSections: { url: 'api/settings/settingsSections', isArray: true }
    //    getSettings: { url: 'api/settings/settings', isArray: true }
    //    update: { method: 'POST', url: 'api/settings' },
    //});

    return {
        getSettingsSections: getSettingsSectionsMock,
        getSettings: getSettingsMock,
        update: updateMock
    };

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
                name: 'Sample.Managed.General.Integer',
                groupName: '-',
                valueType: 'integer',
                defaultValue: '123',
                value: '12',
                title: 'Integer ' + param1.moduleId,
                description: 'An integer setting'
            },
            {
                name: 'Sample.Managed.General.Decimal',
                groupName: '-',
                valueType: 'decimal',
                defaultValue: '123.45',
                value: '12.98',
                title: 'Decimal ' + param1.moduleId,
                description: 'A decimal setting'
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
                value: 'true',
                title: 'Boolean2 ' + param1.moduleId,
                description: 'A boolean setting'
            },
            {
                name: 'Sample.Managed.Advanced.Select',
                groupName: 'Advanced',
                valueType: 'string',
                defaultValue: 'zxcvb mlp',
                value: 'asdfgh',
                title: 'Select ' + param1.moduleId,
                description: 'Select one of the allowed values',
                allowedValues: ['qwerty', 'asdfgh', 'zxcvb mlp']
            }
        ];
        param2(retVal);
    }


    function updateMock(param0, param1, param2) {
        console.log('updateMock: ' + angular.toJson(param1, true));
        param2();
    }
}]);
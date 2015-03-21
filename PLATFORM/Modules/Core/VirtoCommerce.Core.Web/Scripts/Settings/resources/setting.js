angular.module('virtoCommerce.coreModule.settings')
.factory('settings', ['$resource', function ($resource) {
    return $resource('api/settings/:id', { id: '@Id' }, {
        getModules: { url: 'api/settings/modules', isArray: true },
        getSettings: { url: 'api/settings/modules/:id', isArray: true },
        getValues: { url: 'api/settings/values/:id', isArray: true },
        update: { method: 'POST', url: 'api/settings' }
    });

    //return {
    //    getModules: getModulesMock,
    //    getSettings: getSettingsMock,
    //    update: updateMock
    //};

    //function getModulesMock(param1, param2) {
    //    var retVal = [
    //        {
    //            id: 'id1',
    //            title: 'id1 title'
    //        },
    //        {
    //            id: 'id2',
    //            title: 'id2 title'
    //        }
    //    ];
    //    param2(retVal);
    //}

    //function getSettingsMock(param1, param2) {
    //    var retVal = [
    //        {
    //            name: 'VirtoCommerce.Core.Test.General.String',
    //            valueType: 'string',
    //            defaultValue: 'qwerty',
    //            value: 'qwerty',
    //            title: 'String ' + param1.id,
    //            description: 'A text setting'
    //        },
    //        {
    //            name: 'VirtoCommerce.Core.Test.General.Password',
    //            valueType: 'secureString',
    //            defaultValue: 'qwerty',
    //            value: '123',
    //            title: 'Password ' + param1.id,
    //            description: 'A secure text setting'
    //        },
    //        {
    //            name: 'Sample.Managed.General.Integer',
    //            valueType: 'integer',
    //            defaultValue: '123',
    //            value: '12',
    //            title: 'Integer ' + param1.id,
    //            description: 'An integer setting'
    //        },
    //        {
    //            name: 'Sample.Managed.General.Decimal',
    //            valueType: 'decimal',
    //            defaultValue: '123.45',
    //            value: '12.98',
    //            title: 'Decimal ' + param1.id,
    //            description: 'A decimal setting'
    //        },
    //        {
    //            name: 'VirtoCommerce.Core.Test.Advanced.Boolean1',
    //            groupName: 'Advanced',
    //            valueType: 'boolean',
    //            defaultValue: 'True',
    //            value: 'false',
    //            title: 'Boolean1 ' + param1.id,
    //            description: 'A boolean setting'
    //        },
    //        {
    //            name: 'VirtoCommerce.Core.Test.Advanced.Boolean2',
    //            groupName: 'Advanced',
    //            valueType: 'boolean',
    //            defaultValue: 'True',
    //            value: 'True',
    //            title: 'Boolean2 ' + param1.id,
    //            description: 'A boolean setting'
    //        },
    //        {
    //            name: 'Sample.Managed.Advanced.Select',
    //            groupName: 'Advanced',
    //            valueType: 'string',
    //            defaultValue: 'zxcvb mlp',
    //            value: 'asdfgh',
    //            title: 'Select ' + param1.id,
    //            description: 'Select one of the allowed values',
    //            allowedValues: ['qwerty', 'asdfgh', 'zxcvb mlp']
    //        }
    //    ];
    //    param2(retVal);
    //}


    //function updateMock(param0, param1, param2) {
    //    console.log('updateMock: ' + angular.toJson(param1, true));
    //    param2();
    //}
}]);
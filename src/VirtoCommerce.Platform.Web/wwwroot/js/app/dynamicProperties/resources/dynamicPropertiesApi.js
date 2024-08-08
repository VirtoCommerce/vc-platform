angular.module('platformWebApp')
.factory('platformWebApp.dynamicProperties.api', ['$resource', function ($resource) {
    return $resource('api/platform/dynamic/properties', {}, {
        queryTypes: { url: 'api/platform/dynamic/types', isArray: true },
        update: { method: 'PUT' },
        search: { method: 'POST', url: 'api/platform/dynamic/properties/search' },
    });
}])
.factory('platformWebApp.dynamicProperties.dictionaryItemsApi', ['$resource', function ($resource) {
    return $resource('api/platform/dynamic/dictionaryitems', {}, {
        search: { method: 'POST', url: 'api/platform/dynamic/dictionaryitems/search' },
    });
}])
.factory('platformWebApp.dynamicProperties.valueTypesService', function () {
    var propertyTypes = [
        {
            valueType: "ShortText",
            title: "platform.properties.short-text.title",
            description: "platform.properties.short-text.description"
        },
        {
            valueType: "LongText",
            title: "platform.properties.long-text.title",
            description: "platform.properties.long-text.description"
        },
        {
            valueType: "Integer",
            title: "platform.properties.integer.title",
            description: "platform.properties.integer.description"
        },
        {
            valueType: "Decimal",
            title: "platform.properties.decimal.title",
            description: "platform.properties.decimal.description"
        },
        {
            valueType: "DateTime",
            title: "platform.properties.date-time.title",
            description: "platform.properties.date-time.description"
        },
        {
            valueType: "Boolean",
            title: "platform.properties.boolean.title",
            description: "platform.properties.boolean.description"
        },
        {
            valueType: "Html",
            title: "platform.properties.html.title",
            description: "platform.properties.html.description"
        },
        {
            valueType: "Image",
            title: "platform.properties.image.title",
            description: "platform.properties.image.description"
        }
    ];

    return {
        query: function() {
            return propertyTypes;
        }
    };
});

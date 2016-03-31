angular.module('virtoCommerce.customerModule')
.factory('virtoCommerce.customerModule.memberTypesResolverService', function () {
    return {
        objects: [],
        registerType: function (memberTypeDefinition) {
            var memberTypeName = memberTypeDefinition.memberType.toLowerCase();

            if (_.isUndefined(memberTypeDefinition.titleAdd)) {
                memberTypeDefinition.titleAdd = 'customer.blades.member-add.' + memberTypeName + '.title';
            }
            if (_.isUndefined(memberTypeDefinition.descriptionAdd)) {
                memberTypeDefinition.descriptionAdd = 'customer.blades.member-add.' + memberTypeName + '.description';
            }
            if (_.isUndefined(memberTypeDefinition.newInstanceBladeTitle)) {
                memberTypeDefinition.newInstanceBladeTitle = 'customer.blades.new-' + memberTypeName + '.title';
            }
            if (_.isUndefined(memberTypeDefinition.subtitle)) {
                memberTypeDefinition.subtitle = 'customer.blades.' + memberTypeName + '-detail.subtitle';
            }
            if (!memberTypeDefinition.controller) {
                memberTypeDefinition.controller = 'virtoCommerce.customerModule.memberDetailController';
            }

            this.objects.push(memberTypeDefinition);
        }
    };
});
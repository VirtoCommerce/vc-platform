angular.module('virtoCommerce.customerModule')
.factory('virtoCommerce.customerModule.memberTypesResolverService', function () {
    return {
        objects: [],
        registerType: function (memberTypeDefinition) {
            var memberTypeName = memberTypeDefinition.memberType.toLowerCase();

            if (!memberTypeDefinition.titleAdd) {
                memberTypeDefinition.titleAdd = 'customer.blades.member-add.' + memberTypeName + '.title';
            }
            if (!memberTypeDefinition.descriptionAdd) {
                memberTypeDefinition.descriptionAdd = 'customer.blades.member-add.' + memberTypeName + '.description';
            }
            if (!memberTypeDefinition.newInstanceBladeTitle) {
                memberTypeDefinition.newInstanceBladeTitle = 'customer.blades.new-' + memberTypeName + '.title';
            }


            if (!memberTypeDefinition.subtitle) {
                memberTypeDefinition.subtitle = 'customer.blades.' + memberTypeName + '-detail.subtitle';
            }
            if (!memberTypeDefinition.controller) {
                memberTypeDefinition.controller = 'virtoCommerce.customerModule.memberDetailController';
            }

            this.objects.push(memberTypeDefinition);
        }
    };
});
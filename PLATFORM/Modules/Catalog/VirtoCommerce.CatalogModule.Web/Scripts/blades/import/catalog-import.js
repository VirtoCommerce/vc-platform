angular.module('virtoCommerce.catalogModule')
    .factory('virtoCommerce.catalogModule.catalogImportService', function () {
        var retVal = {
            registrationsList: [],
            register: function (registration) {
                this.registrationsList.push(registration);
            }
        };
        return retVal;
    });
angular.module('virtoCommerce.catalogModule')
    .factory('virtoCommerce.catalogModule.catalogExportService', function () {
        var retVal = {
            registrationsList: [],
            register: function (registration) {
                this.registrationsList.push(registration);
            }
        };
        return retVal;
    });
angular.module('virtoCommerce.coreModule.currency')
.factory('virtoCommerce.coreModule.currency.currencyApi', ['$resource', function ($resource) {
    return $resource('api/currencies', null, {
        update: { method: 'PUT' }
    });

    // resource mock test
    //var entries = [
    //  { id: 1, code: "USD", rate: 1, isPrimary: true },
    //  { id: 2, code: "EUR", rate: 0.95 },
    //  { id: 3, code: "JPY", rate: 76 },
    //];

    //return {
    //    query: function (prms, callback) { callback(entries); },
    //    get: function (prms, callback) {
    //        callback(_.findWhere(entries, { id: prms.id }));
    //    },
    //    update: function (prms, callback) { callback(); }
    //};
}]);
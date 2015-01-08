angular.module('virtoCommerce.cartModule.resources.carts', [])
.factory('carts', ['$resource', function ($resource) {
    //return $resource('api/carts/:id', { id: '@Id' }, {
    //    getCarts: { url: 'api/carts/modules/:id', isArray: true },
    //    update: { method: 'POST', url: 'api/carts' }
    //});

    return {
        getCarts: getCartsMock,
        // update: updateMock
    };

    function getCartsMock(param1, param2) {
        var entries = [
            {
                id: 'id1',
                name: 'id1 name',
                customer: { name: 'customer name' },
                items: [{ id: 'i01' }, { id: 'i88' }, { id: 'i05' }],
                totals: '123.45 USD',
                created: '1278903921551',
                modified: '2015-01-05T16:16:08.726Z'
            },
            {
                id: 'id2',
                name: 'id2 name',
                customer: { name: 'john doe name' },
                items: [{ id: 'i01' }],
                totals: '7123.45 USD',
                created: '1228903921551',
                modified: '2015-01-08T16:16:08.726Z'
            }

        ];

        var retVal = {
            totalCount: entries.length,
            listEntries: entries
        }
        param2(retVal);
    }

}]);